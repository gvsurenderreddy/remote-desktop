using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UIControls;
using Utils;
using GenericDataLayer;
using BusinessLogicLayer;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace MViewer
{
    public class Controller : IController
    {
        #region private members

        readonly object _syncRoomObservers = new object();


        IView _view;
        IModel _model;

        #endregion

        #region c-tor

        public Controller()
        {
            // initialize the model

            ControllerEventHandlers handlers = new ControllerEventHandlers()
            {
                ClientConnectedObserver = this.ClientConnected,
                VideoCaptureObserver = this.NotifyVideoCaptureObserver,
                ContactsObserver = this.ContactRequest,
                RoomClosingObserver = this.RoomClosingObserver
            };

            _model = new Model(handlers);
            // initalize the view
            _view = new View(_model);
        }

        #endregion

        #region event handlers

        private void WebCamImageCaptured(object source, EventArgs e)
        {
            try
            {
                VideoCaptureEventArgs args = (VideoCaptureEventArgs)e;
                // display the captured picture
                _view.UpdateWebcapture(args.CapturedImage);

                //while (!_audioCapture.AudioCaptureReady)
                //{
                //    Thread.Sleep(200);
                //}

                //we want to get a byte[] representation ... a MemoryStreams buffer will do
                MemoryStream ms = new MemoryStream();
                //save image to stream ... the stream will write it into the buffer
                args.CapturedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                //get the buffer 
                byte[] bitmapBytes = ms.GetBuffer();

                // todo: send to server
                // loop on all connected peers and send them the captures
                // broadcast the webcaptures

                lock (_syncRoomObservers)
                {
                    IList<string> connectedSessions = _model.SessionManager.GetConnectedSessions(GenericEnums.SessionType.ClientSession, GenericEnums.RoomActionType.Video);
                    foreach (string receiverIdentity in connectedSessions)
                    {
                        //Thread.Sleep(10 * 1000);

                        // todo: check if the stop signal has been sent from the UI
                        // todo: send the last capture and stop the process

                        ConnectedPeers peers = _model.SessionManager.GetPeers(receiverIdentity);
                        if (peers.Video == true)
                        {
                            _model.ClientController.SendCapture(bitmapBytes, receiverIdentity,
                                _model.Identity.MyIdentity);
                        }
                        if (peers.Audio == true)
                        {
                            // todo: send audio capture
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //_webcamCapture.StopCapturing();
            }
        }

        #endregion

        #region public methods

        public void GetContactsStatus()
        {
            _model.PingContacts();
        }

        public void IdentityObserver(object sender, IdentityEventArgs e)
        {
            _model.Identity.UpdateFriendlyName(e.FriendlyName);
        }

        public void StartVideoChat(WebcamCapture webcamControl)
        {
            // create Presenter and start the presentation
            int timerInterval = 100;
            int height = 354, width = 360;

            // initialize the presenter that will send webcam captures to all Server Sessions
            PresenterManager.StartPresentation(webcamControl, _model.Identity.MyIdentity,
                timerInterval, height, width,
                new EventHandler(this.WebCamImageCaptured));
        }

        public void RoomClosingObserver(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            switch (args.ActionType)
            {
                case GenericEnums.RoomActionType.Audio:

                    break;
                case GenericEnums.RoomActionType.Video:
                    lock (_syncRoomObservers)
                    {
                        // todo: close the video chat form
                        PerformRoomAction(sender, args);

                        // todo: send close signal to other side

                        // todo: remove the client session (client used to send my webcaptures)

                    }
                    break;
                case GenericEnums.RoomActionType.Remoting:

                    break;
            }

        }

        public void ClientConnected(object sender, EventArgs e)
        {
            // todo: implement ClientConnected
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            switch (args.ActionType)
            {
                case GenericEnums.RoomActionType.Audio:

                    break;
                case GenericEnums.RoomActionType.Video:
                    // open new Video Chat form to receive the captures
                    Thread t = new Thread(delegate()
                    {
                        StartVideoChat(args.Identity);
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    // open my webcam form and send my captures to the connected contact
                    Program.Controller.PerformRoomAction(sender, args);

                    break;
                case GenericEnums.RoomActionType.Remoting:

                    break;
                case GenericEnums.RoomActionType.Send:

                    break;
            }
        }

        public void StopVideChat(string identity)
        {
            // todo: check if the webcapture is pending for being sent
            // todo: wait for it to finish and block the next sending

            ConnectedPeers peers = _model.SessionManager.GetPeers(identity);
            // todo: update the session
            peers.Audio = false;
            peers.Video = false;
            _model.SessionManager.UpdateSession(identity,
                peers, GenericEnums.SessionState.Closed);
            
            // todo: unblock the sending process; it must know that the chat has been stopped
            
            // send the stop signal to the server session
            _model.ClientController.SendRoomCommand(identity, GenericEnums.RoomActionType.Video, GenericEnums.SignalType.Stop);

            // remove the connected client session
            _model.SessionManager.RemoveSession(identity);
            _view.RoomManager.CloseRoom(identity);
            _view.RoomManager.RemoveRoom(identity);
        }

        public void StartVideoChat(string identity)
        {
            //Session clientSession = new ClientSession(identity);
            //clientSession.SessionState = GenericEnums.SessionState.Opened;
            //// save the connected client session
            //_model.SessionManager.AddSession(clientSession);
            //_model.SessionManager.UpdateSession(identity, 
            //    new ConnectedPeers() 
            //    { 
            //        Video = true ,

            //        // todo: perform specific actions when audio chat is started
            //        //Audio = true 
            //    }, 
            //    clientSession.SessionState);

            if (!_view.IsRoomActivated(identity, GenericEnums.RoomActionType.Video))
            {
                Thread t = new Thread(delegate()
                {
                    //IntPtr handle = IntPtr.Zero;
                    FormVideoRoom videoRoom = new FormVideoRoom();
                    _view.RoomManager.AddRoom(identity, videoRoom);
                    // initialize new video chat form
                    Thread.Sleep(1000);

                    Contact contact = _model.GetContact(identity);
                    // get friendly name from contacts list
                    _view.RoomManager.SetPartnerName(identity, contact.FriendlyName);
                    // finally, show the video chat form where we'll see the webcam captures
                    _view.RoomManager.ShowRoom(identity);
                }
                );
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
        }

        public void PerformRoomAction(object sender, RoomActionEventArgs e)
        {
            // todo: perform specific action when action event has been triggered
            switch (e.ActionType)
            {
                case GenericEnums.RoomActionType.Audio:

                    break;
                case GenericEnums.RoomActionType.Remoting:

                    break;
                case GenericEnums.RoomActionType.Send:

                    break;
                case GenericEnums.RoomActionType.Video:
                    switch (e.SignalType)
                    {
                        case GenericEnums.SignalType.Start:
                            // start the video chat
                            Thread t = new Thread(delegate()
                            {
                                PerformVideoChatAction(sender, e);
                            });
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            break;
                        case GenericEnums.SignalType.Pause:

                            break;
                        case GenericEnums.SignalType.Stop:
                            PerformVideoChatAction(sender, e);
                            break;
                    }
                    break;
            }
        }

        public Contact PerformContactsOperation(object sender, ContactsEventArgs e)
        {
            Contact contact = null;
            if (e.Operation == GenericEnums.ContactsOperation.Load)
            {
                NotifyContactsObserver();
            }
            else
            {
                contact = _model.PerformContactOperation(e);
                NotifyContactsObserver();
            }
            return contact;
        }

        public void NotifyVideoCaptureObserver(object sender, EventArgs e)
        {
            lock (_syncRoomObservers)
            {
                VideoCaptureEventArgs args = (VideoCaptureEventArgs)e;
                ConnectedPeers peers = _model.SessionManager.GetPeers(args.Identity);
                if (peers.Video == true)
                {
                    InitializeRoom(args.Identity, GenericEnums.RoomActionType.Video);
                    _view.RoomManager.ShowPicture(args.Identity, args.CapturedImage);
                }
            }
        }

        public void StartApplication()
        {
            // bind the observers
            _view.BindObservers(true);

            // open main form
            _view.ShowMainForm(false);
            NotifyContactsObserver();

            // todo: use manual reset event instead of thread.sleep(0)
            Thread.Sleep(2000);

            _view.NotifyIdentityObserver();

            // todo: bind client connected observer

            _model.ServerController.StartServer();

            Thread.Sleep(2000);

            // ping every single contact in the list and update it's status
            GetContactsStatus();

            // todo: notify all online contacts that you came on too

        }

        public void StopApplication()
        {
            // unbind the observers
            _view.BindObservers(false);

            // todo: update the StopApplication method
            _model.ServerController.StopServer();

            // exit the environment
            Environment.Exit(0);
        }

        public void NotifyContactsObserver()
        {
            //_model.ContactsUpdated();
            _view.NotifyContactsObserver();
        }

        public void NotifyIdentityObserver()
        {
            //_model.ContactsUpdated();
            _view.NotifyIdentityObserver();
        }

        public void NotifyActionsObserver()
        {
            //_model.ContactsUpdated();
            _view.NotifyActionsObserver();
        }

        #endregion

        #region private methods

        void ContactRequest(object sender, EventArgs e)
        {
            // todo: implement ContactRequest
            PerformContactsOperation(sender, (ContactsEventArgs)e);
        }

        void InitializeRoom(string identity, GenericEnums.RoomActionType roomType)
        {
            switch (roomType)
            {
                case GenericEnums.RoomActionType.Video:
                    // initialize video chat form to receive captures from the client
                    ClientConnected(null, new RoomActionEventArgs()
                    {
                        ActionType = GenericEnums.RoomActionType.Video,
                        SignalType = GenericEnums.SignalType.Start,
                        Identity = identity
                    });

                    // todo: initialize my webcam form so that I can send my captures to the connected contact


                    break;
                case GenericEnums.RoomActionType.Audio:

                    break;
                case GenericEnums.RoomActionType.Remoting:

                    break;
                case GenericEnums.RoomActionType.Send:

                    break;
            }
        }

        void PerformVideoChatAction(object sender, RoomActionEventArgs eArgs)
        {
            switch (eArgs.SignalType)
            {
                case GenericEnums.SignalType.Start:

                    // I am going to send my captures by using the below client

                    _model.ClientController.AddClient(eArgs.Identity);
                    _model.ClientController.StartClient(eArgs.Identity);

                    // create client session
                    Session clientSession = new ClientSession(eArgs.Identity);
                    clientSession.SessionState = GenericEnums.SessionState.Opened;
                    switch (eArgs.ActionType)
                    {
                        case GenericEnums.RoomActionType.Audio:
                            clientSession.Peers = new ConnectedPeers()
                            {
                                Audio = true,
                                Video = false,
                                Remoting = false
                            };
                            break;
                        case GenericEnums.RoomActionType.Video:
                            clientSession.Peers = new ConnectedPeers()
                            {
                                Audio = true,
                                Video = true,
                                Remoting = false
                            };
                            break;
                        case GenericEnums.RoomActionType.Remoting:
                            clientSession.Peers = new ConnectedPeers()
                            {
                                Audio = false,
                                Video = false,
                                Remoting = true
                            };
                            break;
                    }
                    // save the proxy to which we are sending the webcam captures
                    _model.SessionManager.AddSession(clientSession);

                    // initialize the webcamCapture form
                    // this form will be used to capture the images and send them to all Server Sessions
                    _view.ShowMyWebcamForm();
                    break;
                case GenericEnums.SignalType.Stop:

                    StopVideChat(eArgs.Identity);

                    break;
                case GenericEnums.SignalType.Pause:

                    break;
            }
        }

        #endregion

        #region proprieties



        #endregion
    }
}
