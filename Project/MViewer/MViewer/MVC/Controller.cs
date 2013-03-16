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
        bool _remotingCapturePending;

        /// <summary>
        /// object used to synchronize the capture sent to partners
        /// </summary>
        readonly object _syncVideoCaptureSending = new object();
        readonly object _syncRemotingCaptureSending = new object();

        ManualResetEvent _syncVideoCaptureActivity = new ManualResetEvent(true);
        ManualResetEvent _syncRemotingCaptureActivity = new ManualResetEvent(true);

        IRoomCommandInvoker _roomCommandInvoker;

        IView _view;
        IModel _model;

        #endregion

        #region c-tor

        public Controller()
        {
            // initialize the model
            _model = new Model();
            // initalize the view and bind it to the model
            _view = new View(_model);
        }

        #endregion

        #region event handlers

        public void InitializeSettings()
        {
            _roomCommandInvoker = new RoomCommandInvoker(SystemConfiguration.Instance.RoomHandlers);

            ControllerEventHandlers handlers = new ControllerEventHandlers()
            {
                ClientConnectedObserver = this.ClientConnectedObserver,
                VideoCaptureObserver = this.VideoCaptureObserver,
                ContactsObserver = this.ContactRequestObserver,
                RoomButtonObserver = this.RoomButtonAction,
                WaitRoomActionObserver = this.WaitRoomButtonActionObserver,
                FileTransferObserver = this.FileTransferObserver,
                FilePermissionObserver = this.FileTransferPermission,
                RemotingCaptureObserver = this.RemotingCaptureObserver
            };
            _model.IntializeModel(handlers);
        }

        public void RemotingImageCaptured(object source, EventArgs e)
        {
            // binded RemotingImageCaptured to the remoting capture object
            try
            {
                _syncRemotingCaptureActivity.WaitOne(); // wait for any room action to end

                _remotingCapturePending = true;
                if (PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).RemotingCaptureClosed() == false)
                {
                    lock (_syncRemotingCaptureSending)
                    {
                        // broadcast the webcaptures to all connected peers
                        RemotingCaptureEventArgs args = (RemotingCaptureEventArgs)e;
                        IList<string> connectedSessions = _model.SessionManager.GetConnectedSessions(GenericEnums.RoomType.Remoting);
                        if (connectedSessions.Count == 0)
                        {
                            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();
                        }
                        else
                        {
                            foreach (string receiverIdentity in connectedSessions)
                            {
                                TransferStatusUptading transfer = _model.SessionManager.GetTransferActivity(receiverIdentity);
                                while (transfer.IsRemotingUpdating)
                                {
                                    Thread.Sleep(200);
                                }

                                PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(receiverIdentity);
                                // check if the stop signal has been sent from the UI

                                // check if the stop signal has been sent by the partner

                                PeerStates peers = _model.SessionManager.GetPeerStatus(receiverIdentity);
                                if (peers.RemotingSessionState == GenericEnums.SessionState.Opened
                                    || peers.RemotingSessionState == GenericEnums.SessionState.Pending)
                                {
                                    // send the capture if the session isn't paused
                                    transferStatus.Remoting = true;

                                    _model.ClientController.SendRemotingCapture(args.Capture, receiverIdentity,
                                        _model.Identity.MyIdentity);
                                }

                                transferStatus.Remoting = false;
                            }
                        }
                    }
                }
                else
                {
                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                _remotingCapturePending = false;
            }
        }

        public void VideoImageCaptured(object source, EventArgs e)
        {
            try
            {
                _syncVideoCaptureActivity.WaitOne(); // wait for any room action to end

                _videoCapturePending = true;
                if (_view.VideoCaptureClosed == false)
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
                            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopVideoPresentation();
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

                                    _model.ClientController.SendVideoCapture(bitmapBytes, receiverIdentity,
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
                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopVideoPresentation();
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

            SystemConfiguration.Instance.PresenterSettings.VideoCaptureControl = webcamControl;
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartVideoPresentation();

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

        public void StopVideChat(object sender, RoomActionEventArgs args)
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

            if (_view.RoomManager.VideoRoomsLeft() == false)
            {
                _view.ResetLabels(GenericEnums.RoomType.Video);
            }

            // close the webcapture form if there s no room left
            if (!_view.RoomManager.VideoRoomsLeft())
            {
                StopVideoCapturing();
            }

            transfer.IsVideoUpdating = false;
            _syncVideoCaptureActivity.Set();

        }

        public void PauseVideoChat(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            // use the peer status of the selected chatroom
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.VideoSessionState = GenericEnums.SessionState.Paused; // pause the video chat

            _syncVideoCaptureActivity.Set();

        }

        public void ResumeVideoChat(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.VideoSessionState = GenericEnums.SessionState.Opened; // resume the video chat

            _syncVideoCaptureActivity.Set();

        }

        public void SendFileHandler(object sender, RoomActionEventArgs args)
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

        //todo: complete ResumeRemoting
        public void ResumeRemotingChat(object sender, RoomActionEventArgs args)
        {
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.RemotingSessionState = GenericEnums.SessionState.Opened; // resume the remoting

        }

        //todo: complete PauseRemoting
        public void PauseRemotingChat(object sender, RoomActionEventArgs args)
        {
            // use the peer status of the selected chatroom
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.RemotingSessionState = GenericEnums.SessionState.Paused; // pause the remoting

        }

        public void StartRemotingChat(object sender, RoomActionEventArgs args)
        {
            // I am going to send my captures by using the below client
            _model.ClientController.AddClient(args.Identity);
            _model.ClientController.StartClient(args.Identity);

            // create client session
            Session clientSession = new ClientSession(args.Identity, args.RoomType);
            // save the proxy to which we are sending the remoting captures
            _model.SessionManager.AddSession(clientSession);

            // initialize the remoting tool and start it's timer
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartRemotingPresentation();
        }

        public void StopRemotingChat(object sender, RoomActionEventArgs args)
        {
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.RemotingSessionState = GenericEnums.SessionState.Closed;
            _model.SessionManager.RemoveSession(args.Identity);

            _model.RemoveClient(args.Identity);

            if (!_model.SessionManager.RemotingRoomsLeft())
            {
                // check if any remoting session is still active
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();
            }
        }

        public void StartVideoChat(object sender, RoomActionEventArgs args)
        {
            // open new Video Chat form to receive the captures
            OpenVideoForm(args.Identity);

            // I am going to send my captures by using the below client
            _model.ClientController.AddClient(args.Identity);
            _model.ClientController.StartClient(args.Identity);

            // create client session
            Session clientSession = new ClientSession(args.Identity, args.RoomType);
            // save the proxy to which we are sending the webcam captures
            _model.SessionManager.AddSession(clientSession);

            // initialize the webcamCapture form
            // this form will be used to capture the images and send them to all Server Sessions _presenter.StopPresentation();
            _view.ShowMyWebcamForm(true);
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
                    StopVideChat(this, new RoomActionEventArgs()
                        {
                            Identity = identity,
                            RoomType = GenericEnums.RoomType.Video,
                            SignalType = GenericEnums.SignalType.Stop 
                        });
                }
                // stop my webcapture form
                StopVideoCapturing();

                // stop the audio rooms also
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
            
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();

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

        private void RemotingCaptureObserver(object sender, EventArgs e)
        {
            RemotingCaptureEventArgs args = (RemotingCaptureEventArgs)e;
            PeerStates peer = _model.SessionManager.GetPeerStatus(args.Identity);

            if (peer.RemotingSessionState == GenericEnums.SessionState.Undefined ||
                peer.RemotingSessionState == GenericEnums.SessionState.Pending)
            {
                // receiving captures for the first time, have to initalize a form
                ClientConnectedObserver(null,
                       new RoomActionEventArgs()
                       {
                           RoomType = GenericEnums.RoomType.Remoting,
                           SignalType = GenericEnums.SignalType.Start,
                           Identity = args.Identity
                       });
                while (peer.RemotingSessionState != GenericEnums.SessionState.Opened)
                {
                    Thread.Sleep(2000);
                    peer = _model.SessionManager.GetPeerStatus(args.Identity); // update the peer status
                }
            }

            // check the videochat status before displaying the picture
            if (peer.RemotingSessionState == GenericEnums.SessionState.Opened)
            {
                //todo : display the remoting capture in the opened form
            }
        }

        void ClientConnectedObserver(object sender, EventArgs e)
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

                    // add a progress bar (into a TransfersForm)
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

        void OpenVideoForm(string identity)
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
        void VideoCaptureObserver(object sender, EventArgs e)
        {
            VideoCaptureEventArgs args = (VideoCaptureEventArgs)e;
            PeerStates peer = _model.SessionManager.GetPeerStatus(args.Identity);

            if (peer.VideoSessionState == GenericEnums.SessionState.Undefined ||
                peer.VideoSessionState == GenericEnums.SessionState.Pending)
            {
                // receiving captures for the first time, have to initalize a form
                ClientConnectedObserver(null,
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
        void ContactRequestObserver(object sender, EventArgs e)
        {
            PerformContactsOperation(sender, (ContactsEventArgs)e);
        }

        void StopVideoCapturing()
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopVideoPresentation();
            
            Thread.Sleep(500);
            while (_videoCapturePending)
            {
                Thread.Sleep(200);
            }
            _view.ShowMyWebcamForm(false);
        }

        #endregion

        #region proprieties

        public string MyIdentity()
        {
            return _model.Identity.MyIdentity;
        }

        #endregion
    }
}
