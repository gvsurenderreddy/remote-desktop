using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;
using GenericObjects;
using Utils;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region private methods

        /// <summary>
        /// flag used to tell the capturing thread to wait before ending it's activity
        /// </summary>
        bool _videoCapturePending;
        bool _audioCapturePending;

        /// <summary>
        /// object used to synchronize the capture sent to partners
        /// </summary>
        readonly object _syncVideoCaptureSending = new object();
        readonly object _syncAudioCaptureSending = new object();

        ManualResetEvent _syncAudioCaptureActivity = new ManualResetEvent(true);
        ManualResetEvent _syncVideoCaptureActivity = new ManualResetEvent(true);

        #endregion

        #region public methods

        public void OnVideoImageCaptured(object source, EventArgs e)
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

        public void StopVideo(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            bool sendStopSignal = true;
            if (sender.GetType().IsInstanceOfType(typeof(MViewerServer)))
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

        public void PauseVideo(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            // use the peer status of the selected room
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.VideoSessionState = GenericEnums.SessionState.Paused; // pause the video 

            _syncVideoCaptureActivity.Set();

        }

        public void ResumeVideo(object sender, RoomActionEventArgs args)
        {
            _syncVideoCaptureActivity.Reset();

            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.VideoSessionState = GenericEnums.SessionState.Opened; // resume the video 

            _syncVideoCaptureActivity.Set();

        }

        // todo: convert this to an event handler , use it in the Webcam Form as observer
        public void StartVideo(WebcamCapture webcamControl)
        {
            // create Presenter and start the presentation
            // initialize the presenter that will send webcam captures to all Server Sessions

            SystemConfiguration.Instance.PresenterSettings.VideoCaptureControl = webcamControl;
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartVideoPresentation();

        }

        public void StartVideo(object sender, RoomActionEventArgs args)
        {
            // open new Video  form to receive the captures
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

        #endregion

        #region private methods

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
                ClientConnectedObserver(this,
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

            // check the video status before displaying the picture
            if (peer.VideoSessionState == GenericEnums.SessionState.Opened)
            {
                _view.RoomManager.ShowVideoCapture(args.Identity, args.CapturedImage);
            }
        }

        void StopVideoCapturing()
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopVideoPresentation();

            while (_videoCapturePending)
            {
                Thread.Sleep(200);
            }
            _view.ShowMyWebcamForm(false);
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
                    // initialize new video  form

                    PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                    peers.VideoSessionState = GenericEnums.SessionState.Opened;
                    peers.AudioSessionState = GenericEnums.SessionState.Opened;

                    Contact contact = _model.GetContact(identity);
                    // get friendly name from contacts list
                    _view.RoomManager.SetPartnerName(identity, contact.FriendlyName);
                    // finally, show the video  form where we'll see the webcam captures
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

        #endregion
    }
}
