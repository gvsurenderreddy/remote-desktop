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
using UIControls.Forms;

namespace MViewer
{
    public class Controller : IController
    {
        #region private members

        /// <summary>
        /// flag used to tell the capturing thread to wait before ending it's activity
        /// </summary>
        bool _videoCapturePending;

        /// <summary>
        /// object used to synchronize the capture sent to partners
        /// </summary>
        readonly object _syncVideoCaptureSending = new object();

        ManualResetEvent _syncVideoCaptureActivity = new ManualResetEvent(true);

        PresenterSettings _presenterSettings;

        IRoomCommandInvoker _roomCommandInvoker;

        ControllerRoomHandlers _roomHandlers;

        IView _view;
        IModel _model;

        #endregion

        #region c-tor

        public Controller()
        {
            ControllerEventHandlers handlers = new ControllerEventHandlers()
            {
                ClientConnectedObserver = this.ClientConnected,
                VideoCaptureObserver = this.NotifyVideoCaptureObserver,
                ContactsObserver = this.ContactRequest,
                RoomClosingObserver = this.RoomButtonAction,
                WaitRoomActionObserver = this.WaitRoomButtonActionObserver,
                FileTransferObserver = this.FileTransferObserver,
                FilePermissionObserver = this.FileTransferPermission
            };

            InitializeRoomHandlers();

           // initialize the model
            _model = new Model(handlers);
            // initalize the view and bind it to the model
            _view = new View(_model);

            InitializePresenterSettings();
            
        }

        #endregion

        #region event handlers

        private void WebCamImageCaptured(object source, EventArgs e)
        {
            try
            {
                _syncVideoCaptureActivity.WaitOne(); // wait for any room action to end

                _videoCapturePending = true;
                if (_view.WebcaptureClosed == false)
                {
                    lock (_syncVideoCaptureSending)
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

                        // broadcast the webcaptures to all connected peers

                        IList<string> connectedSessions = _model.SessionManager.GetConnectedSessions(GenericEnums.RoomType.Video);
                        if (connectedSessions.Count == 0)
                        {
                            PresenterManager.Instance(_presenterSettings).StopVideoPresentation();
                        }
                        else
                        {
                            foreach (string receiverIdentity in connectedSessions)
                            {
                                TransferStatusUptading transfer = _model.SessionManager.GetTransferActivity(receiverIdentity);
                                while (transfer.IsVideoUpdating)
                                {
                                    Thread.Sleep(200);
                                }

                                PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(receiverIdentity);
                                // check if the stop signal has been sent from the UI

                                // check if the stop signal has been sent by the partner

                                PeerStates peers = _model.SessionManager.GetPeerStatus(receiverIdentity);
                                if (peers.VideoSessionState == GenericEnums.SessionState.Opened
                                    || peers.VideoSessionState == GenericEnums.SessionState.Pending)
                                {
                                    // send the capture if the session isn't paused
                                    transferStatus.Video = true;

                                    // todo: enforce web timer start for partner side

                                    _model.ClientController.SendCapture(bitmapBytes, receiverIdentity,
                                        _model.Identity.MyIdentity);
                                }

                                transferStatus.Video = false;
                                if (peers.AudioSessionState == GenericEnums.SessionState.Opened
                                    || peers.AudioSessionState == GenericEnums.SessionState.Pending)
                                {
                                    transferStatus.Audio = true;

                                    // todo: send audio capture

                                    transferStatus.Audio = false;
                                }

                            }
                        }
                    }
                }
                else
                {
                    PresenterManager.Instance(_presenterSettings).StopVideoPresentation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //_webcamCapture.StopCapturing();
            }
            finally
            {
                _videoCapturePending = false;
            }
        }

        #endregion

        #region public methods

        // todo: convert this to an event handler , use it in the Video Form as observer
        public void ActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType)
        {
            // update the active room identity
            _view.RoomManager.ActiveRoom = newIdentity;
            // update the actions form button labels
            _view.UpdateLabels(newIdentity, roomType);
        }

        // todo: convert this to an event handler , use it in the Main Form as observer
        public void IdentityObserver(object sender, IdentityEventArgs e)
        {
            _model.Identity.UpdateFriendlyName(e.FriendlyName);
            // notify online contacts of updated friendly name
            _model.NotifyContacts(e.FriendlyName);
        }

        // todo: convert this to an event handler , use it in the Webcam Form as observer
        public void StartVideoChat(WebcamCapture webcamControl)
        {
            // create Presenter and start the presentation
            // initialize the presenter that will send webcam captures to all Server Sessions

            _presenterSettings.captureControl = webcamControl;
            PresenterManager.Instance(_presenterSettings).StartVideoPresentation();

        }

        void SendFileHandler(object sender, RoomActionEventArgs args)
        {
            if (_model.ClientController.IsContactOnline(args.Identity))
            {
                Contact contact = _model.GetContact(args.Identity);
                string filePath = string.Empty;
                FileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = fileDialog.FileName;

                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length <= 10485760)
                    {

                        FormFileProgress fileProgressFrom = null;

                        Thread t = new Thread(delegate()
                        {
                            fileProgressFrom = new FormFileProgress(
                                Path.GetFileName(filePath), contact.FriendlyName);

                            Application.Run(fileProgressFrom);

                        });
                        t.Start();
                        Thread.Sleep(500);
                        Thread t2 = new Thread(delegate()
                        {
                            fileProgressFrom.StartPB();
                        });
                        t2.Start();
                        _model.SendFile(filePath, args.Identity);

                        if (fileProgressFrom != null)
                        {
                            fileProgressFrom.StopProgress();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, cannot transfer files larger than 10 MB in MViewer-lite", "Transfer not possible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                   
                }
            }
            else
            {
                MessageBox.Show("Selected person is offline", "Cannot send", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // todo: convert this to an event handler , use it in the View as observer
        public void RoomButtonAction(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            _roomCommandInvoker.PerformCommand(sender, args);
        }

        // todo: convert this to an event handler , use it in the Main Form as observer
        public Contact PerformContactsOperation(object sender, ContactsEventArgs e)
        {
            Contact contact = null;
            if (e.Operation == GenericEnums.ContactsOperation.Load)
            {
                // don't need to send signal to the Model
                _view.NotifyContactsObserver();
            }
            else
            {
                // add/remove/get/status/name update
                contact = _model.PerformContactOperation(e);
                _view.NotifyContactsObserver();
            }
            return contact;
        }

        public void StartApplication()
        {
            // bind the observers
            _view.BindObservers(true);

            // open main form
            _view.ShowMainForm(false);
            _view.NotifyContactsObserver();

            // todo: use manual reset event instead of thread.sleep(0)
            Thread.Sleep(2000);

            _view.NotifyIdentityObserver();
            _model.ServerController.StartServer();

            Thread.Sleep(2000);

            // ping every single contact in the list and update it's status
            _model.PingContacts(null);

            // notify all online contacts that you came on too
            _model.NotifyContacts(GenericEnums.ContactStatus.Online);
        }

        public void StopApplication()
        {
            // todo: update the StopApplication method with other actions

            // check for running video/audio/remoting chats
            bool canExit = _view.ExitConfirmation();
            if (canExit)
            {
                // stop all active rooms
                IList<string> partnerIdentities = _model.SessionManager.GetConnectedSessions(GenericEnums.RoomType.Video);
                foreach (string identity in partnerIdentities)
                {
                    StopVideChat(null, new RoomActionEventArgs()
                        {
                            Identity = identity,
                            RoomType = GenericEnums.RoomType.Video,
                            SignalType = GenericEnums.SignalType.Stop 
                        });
                }
                // stop my webcapture form
                StopWebCapturing();

                // todo: stop the audio & remoting rooms also

                // unbind the observers
                _view.BindObservers(false);

                _model.ServerController.StopServer();

                // notify all contacts that you exited the chat
                _model.NotifyContacts(GenericEnums.ContactStatus.Offline);

                // exit the environment
                Environment.Exit(0);
            }
        }

        #endregion

        #region private methods

        void VideoStart(object sender, RoomActionEventArgs args)
        {
            // open new Video Chat form to receive the captures
            StartVideoChat(args.Identity);

            // I am going to send my captures by using the below client
            _model.ClientController.AddClient(args.Identity);
            _model.ClientController.StartClient(args.Identity);

            // create client session
            Session clientSession = new ClientSession(args.Identity);
            switch (args.RoomType)
            {
                case GenericEnums.RoomType.Audio:
                    clientSession.AudioSessionState = GenericEnums.SessionState.Pending;
                    break;
                case GenericEnums.RoomType.Video:
                    clientSession.AudioSessionState = GenericEnums.SessionState.Pending;
                    clientSession.VideoSessionState = GenericEnums.SessionState.Pending;
                    break;
            }
            // save the proxy to which we are sending the webcam captures
            _model.SessionManager.AddSession(clientSession);

            // initialize the webcamCapture form
            // this form will be used to capture the images and send them to all Server Sessions _presenter.StopPresentation();
            _view.ShowMyWebcamForm(true);
        }

        void ClientConnected(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            _roomCommandInvoker.PerformCommand(sender, args);
        }

        void FileTransferObserver(object sender, EventArgs e)
        {
            Thread t = new Thread(delegate()
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;

                byte[] buffer = (byte[])sender; // this is the file sent

                // open file path dialog
                string extension = Path.GetExtension(args.TransferInfo.FileName);// get file extension

                // Displays a SaveFileDialog so the user can save the Image
                // assigned to Button2.
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "File|*." + extension + "";
                saveFileDialog1.Title = "Save File";
                saveFileDialog1.FileName = args.TransferInfo.FileName;
                DialogResult dialogResult = saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (dialogResult == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    // remove the existing file if the user confirmed
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }

                    // todo: add a progress bar (into a TransfersForm)
                    FormFileProgress fileProgressFrom = null;
                    Contact contact = _model.GetContact(args.Identity);
                    Thread t3 = new Thread(delegate()
                    {
                        fileProgressFrom = new FormFileProgress(
                            Path.GetFileName(saveFileDialog1.FileName), contact.FriendlyName);

                        Application.Run(fileProgressFrom);

                    });
                    t3.Start();
                    Thread.Sleep(500);
                    Thread t2 = new Thread(delegate()
                    {
                        fileProgressFrom.StartPB();
                    });
                    t2.Start();
                    
                    // Saves the Image via a FileStream created by the OpenFile method.
                    System.IO.FileStream fs =
                       (System.IO.FileStream)saveFileDialog1.OpenFile();

                    fs.Write(buffer, 0, buffer.Length);

                    fs.Close();

                    if (fileProgressFrom != null)
                    {
                        fileProgressFrom.StopProgress();
                    }  
                }

            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        void FileTransferPermission(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            bool canSend = _view.RequestTransferPermission(args.Identity, args.TransferInfo.FileName, args.TransferInfo.FileSize);
            args.TransferInfo.HasPermission = canSend;
        }

        void InitializeRoomHandlers()
        {
            Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> videoDelegates = new Dictionary<GenericEnums.SignalType,Delegates.CommandDelegate>();
            videoDelegates.Add(GenericEnums.SignalType.Start, this.VideoStart);
            videoDelegates.Add(GenericEnums.SignalType.Stop, this.StopVideChat);
            videoDelegates.Add(GenericEnums.SignalType.Pause, this.PauseVideo);
            videoDelegates.Add(GenericEnums.SignalType.Resume, this.ResumeVideo);

            Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> transferDelegates = new Dictionary<GenericEnums.SignalType,Delegates.CommandDelegate>();
            transferDelegates.Add(GenericEnums.SignalType.Start, this.SendFileHandler);

            _roomHandlers = new ControllerRoomHandlers()
            {
                // todo: add audio & remoting handlers handlers by signal type
                Video = videoDelegates,
                Transfer = transferDelegates
            };
       
            _roomCommandInvoker = new RoomCommandInvoker(_roomHandlers);
        }

        void InitializePresenterSettings()
        {
            int timerInterval = 100;
            int height = 354, width = 360;

            _presenterSettings = new PresenterSettings()
            {
                identity = _model.Identity.MyIdentity,
                    timerInterval = timerInterval, 
                    videoSize =
                    new Structures.ScreenSize()
                    {
                        Height = height,
                        Width = width
                    },
                    webCamImageCaptured =
                    new EventHandler(this.WebCamImageCaptured)
            };
        }

        void StopVideChat(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            bool sendStopSignal = true;
            if (sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
            {
                sendStopSignal = false;
            }
            string identity = args.Identity;
            // tell the partner to pause capturing & sending while processing room Stop command

            _model.ClientController.WaitRoomButtonAction(identity, _model.Identity.MyIdentity, GenericEnums.RoomType.Video,
                true);

            TransferStatusUptading transfer = _model.SessionManager.GetTransferActivity(identity);
            transfer.IsVideoUpdating = true;

            // check if the webcapture is pending for being sent
            PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(identity);
            while (transferStatus.Video)
            {
                // wait for it to finish and block the next sending
                Thread.Sleep(200);
            }

            PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
            // update the session status to closed
            if (peers.VideoSessionState != GenericEnums.SessionState.Closed)
            {
                peers.AudioSessionState = GenericEnums.SessionState.Closed;
                peers.VideoSessionState = GenericEnums.SessionState.Closed;
                _model.SessionManager.UpdateSession(identity, peers);

                if (sendStopSignal)
                {
                    // send the stop signal to the server session
                    _model.ClientController.SendRoomCommand(_model.Identity.MyIdentity, identity,
                        GenericEnums.RoomType.Video, GenericEnums.SignalType.Stop);
                }

                // remove the connected client session
                _model.SessionManager.RemoveSession(identity);
                _view.RoomManager.CloseRoom(identity);
                _view.RoomManager.RemoveRoom(identity);
            }

            _model.ClientController.WaitRoomButtonAction(identity, _model.Identity.MyIdentity, GenericEnums.RoomType.Video,
                false);

            if (_view.RoomManager.RoomsLeft() == false)
            {
                _view.ResetLabels(GenericEnums.RoomType.Video);
            }

            // close the webcapture form if there s no room left
            if (!_view.RoomManager.RoomsLeft())
            {
                StopWebCapturing();
            }

            _syncVideoCaptureActivity.Set();

        }

        void StartVideoChat(string identity)
        {
            _syncVideoCaptureActivity.Reset();
           
            if (!_view.IsRoomActivated(identity, GenericEnums.RoomType.Video))
            {
                Thread t = new Thread(delegate()
                {
                    //IntPtr handle = IntPtr.Zero;
                    FormVideoRoom videoRoom = new FormVideoRoom(identity);
                    _view.RoomManager.AddRoom(identity, videoRoom);
                    // initialize new video chat form
                    Thread.Sleep(1000);

                    PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                    peers.VideoSessionState = GenericEnums.SessionState.Opened;
                    peers.AudioSessionState = GenericEnums.SessionState.Opened;

                    _model.SessionManager.UpdateSession(identity, peers);

                    peers = _model.SessionManager.GetPeerStatus(identity);

                    Contact contact = _model.GetContact(identity);
                    // get friendly name from contacts list
                    _view.RoomManager.SetPartnerName(identity, contact.FriendlyName);
                    // finally, show the video chat form where we'll see the webcam captures
                    _view.RoomManager.ShowRoom(identity);

                    
                }
                );
                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                Thread.Sleep(500);
                _syncVideoCaptureActivity.Set();


            }
        }

        void WaitRoomButtonActionObserver(object sender, EventArgs e)
        {
            // todo: complete implemention of WaitRoomButtonAction
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            switch (args.RoomType)
            {
                case GenericEnums.RoomType.Video:
                    bool freezeVideo = false;
                    if (args.SignalType == GenericEnums.SignalType.Wait)
                    {
                        freezeVideo = true;
                    }
                    // send the freeze signal to the webcapture obj
                    _view.WaitRoomButtonAction(freezeVideo);
                    break;
            }
        }

        /// <summary>
        /// method used to diplay a received video capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NotifyVideoCaptureObserver(object sender, EventArgs e)
        {
            VideoCaptureEventArgs args = (VideoCaptureEventArgs)e;
            PeerStates peer = _model.SessionManager.GetPeerStatus(args.Identity);

            if (peer.VideoSessionState == GenericEnums.SessionState.Undefined ||
                peer.VideoSessionState == GenericEnums.SessionState.Pending)
            {
                // receiving captures for the first time, have to initalize a form
                ClientConnected(null,
                       new RoomActionEventArgs()
                       {
                           RoomType = GenericEnums.RoomType.Video,
                           SignalType = GenericEnums.SignalType.Start,
                           Identity = args.Identity
                       });
                while (peer.VideoSessionState != GenericEnums.SessionState.Opened)
                {
                    Thread.Sleep(2000);
                    peer = _model.SessionManager.GetPeerStatus(args.Identity); // update the peer status
                }
            }

            // check the videochat status before displaying the picture
            if (peer.VideoSessionState == GenericEnums.SessionState.Opened)
            {
                _view.RoomManager.ShowPicture(args.Identity, args.CapturedImage);
            }
        }

        // don't remove this one yet because PerformContactsOperation has a return type (cannot be used as event handler)
        void ContactRequest(object sender, EventArgs e)
        {
            PerformContactsOperation(sender, (ContactsEventArgs)e);
        }

        void StopWebCapturing()
        {
            PresenterManager.Instance(_presenterSettings).StopVideoPresentation();
            
            Thread.Sleep(500);
            while (_videoCapturePending)
            {
                Thread.Sleep(200);
            }
            _view.ShowMyWebcamForm(false);
        }

        void ResumeVideo(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.VideoSessionState = GenericEnums.SessionState.Opened; // resume the video chat
            _model.SessionManager.UpdateSession(args.Identity, peers);

            _syncVideoCaptureActivity.Set();

        }

        void PauseVideo(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            // use the peer status of the selected chatroom
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.VideoSessionState = GenericEnums.SessionState.Paused; // pause the video chat
            _model.SessionManager.UpdateSession(args.Identity, peers);

            _syncVideoCaptureActivity.Set();

        }

        #endregion
    }
}
