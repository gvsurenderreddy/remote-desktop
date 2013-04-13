using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows.Forms;
using BusinessLogicLayer;
using GenericObjects;
using Utils;
using Abstraction;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region private methods

        /// <summary>
        /// flag used to tell the capturing thread to wait before ending it's activity
        /// </summary>
        bool _videoCapturePending;

        /// <summary>
        /// object used to synchronize the capture sent to partners
        /// </summary>
        readonly object _syncVideoCaptureSending = new object();
        readonly object _syncVideoStartStop = new object();

        ManualResetEvent _syncVideoCaptureActivity = new ManualResetEvent(true);

        #endregion

        #region public methods

        /// <summary>
        /// method used to send video captures to specific partner
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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

                        MemoryStream memoryStream = new MemoryStream();
                        args.CapturedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

                        //get the buffer 
                        byte[] buffer = memoryStream.GetBuffer();

                        // send the webcaptures to active video room
                        string receiverIdentity = ((ActiveRooms)_view.RoomManager.ActiveRooms).VideoRoomIdentity;
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

                            _model.ClientController.SendVideoCapture(buffer, receiverIdentity,
                                ((Identity)_model.Identity).MyIdentity);
                        }

                        transferStatus.Video = false;
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
            }
            finally
            {
                _videoCapturePending = false;
            }
        }

        /// <summary>
        /// method used to stop video conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StopVideo(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                lock (_syncVideoStartStop)
                {
                    _syncVideoCaptureActivity.Reset();

                    bool sendStopSignal = true;
                    if (sender.GetType().IsEquivalentTo(typeof(IMViewerService)))
                    {
                        sendStopSignal = false;
                    }
                    string identity = e.Identity;
                    // tell the partner to pause capturing & sending while processing room Stop command

                    _model.ClientController.WaitRoomButtonAction(identity, ((Identity)_model.Identity).MyIdentity, GenericEnums.RoomType.Video,
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
                    if (peers.VideoSessionState != GenericEnums.SessionState.Closed
                        && peers.VideoSessionState != GenericEnums.SessionState.Undefined)
                    {
                        peers.VideoSessionState = GenericEnums.SessionState.Closed;

                        if (sendStopSignal)
                        {
                            // send the stop signal to the server session
                            _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, identity,
                                GenericEnums.RoomType.Video, GenericEnums.SignalType.Stop);
                        }

                        // remove the connected client session
                        _model.SessionManager.RemoveSession(identity);
                        _view.RoomManager.CloseRoom(identity, GenericEnums.RoomType.Video);
                        _view.RoomManager.RemoveRoom(identity, GenericEnums.RoomType.Video);

                        _view.UpdateLabels(e.Identity, e.RoomType);
                    }

                    _model.ClientController.WaitRoomButtonAction(identity, ((Identity)_model.Identity).MyIdentity, 
                        GenericEnums.RoomType.Video,
                        false);

                    // close the webcapture form if there s no room left
                    if (!_view.RoomManager.RoomsLeft(GenericEnums.RoomType.Video))
                    {
                        _view.ResetLabels(e.RoomType);
                        StopVideoCapturing();
                    }

                    transfer.IsVideoUpdating = false;
                    _syncVideoCaptureActivity.Set();

                    if (sender.GetType().IsEquivalentTo(typeof(UIControls.ActionsControl)))
                    {
                        this.StopAudio(this, new RoomActionEventArgs()
                        {
                            Identity = e.Identity,
                            RoomType = GenericEnums.RoomType.Audio,
                            SignalType = GenericEnums.SignalType.Stop
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to pause video conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void PauseVideo(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                _syncVideoCaptureActivity.Reset();

                // use the peer status of the selected room
                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.VideoSessionState = GenericEnums.SessionState.Paused; // pause the video 

                _syncVideoCaptureActivity.Set();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to resume video conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ResumeVideo(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                _syncVideoCaptureActivity.Reset();

                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.VideoSessionState = GenericEnums.SessionState.Opened; // resume the video 

                _syncVideoCaptureActivity.Set();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: optional - convert this to an event handler , use it in the Webcam Form as observer
        /// <summary>
        /// method used to trigger video capture control start
        /// </summary>
        /// <param name="webcamControl"></param>
        public void StartVideo(IWebcamCapture webcamControl)
        {
            try
            {
                // create Presenter and start the presentation
                // initialize the presenter that will send webcam captures to all Server Sessions
                SystemConfiguration.Instance.PresenterSettings.VideoCaptureControl = webcamControl;
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartVideoPresentation();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to initiate video conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StartVideo(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                lock (_syncVideoStartStop)
                {
                    // create client session
                    Session clientSession = new ClientSession(e.Identity, e.RoomType);
                    // save the proxy to which we are sending the webcam captures
                    _model.SessionManager.AddSession(clientSession, GenericEnums.RoomType.Video);

                    // open new Video  form to receive the captures
                    OpenVideoForm(e.Identity);

                    // I am going to send my captures by using the below client
                    _model.ClientController.AddClient(e.Identity);
                    _model.ClientController.StartClient(e.Identity);

                    // initialize the webcamCapture form
                    // this form will be used to capture the images and send them to all Server Sessions _presenter.StopPresentation();
                    _view.ShowMyWebcamForm(true);

                    Thread.Sleep(1000);

                    this.StartAudio(this, new RoomActionEventArgs()
                    {
                        Identity = e.Identity,
                        RoomType = GenericEnums.RoomType.Audio,
                        SignalType = GenericEnums.SignalType.Start
                    });
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
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
            try
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

                    _syncVideoCaptureActivity.WaitOne();
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void StopVideoCapturing()
        {
            try
            {
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopVideoPresentation();
                while (_videoCapturePending)
                {
                    Thread.Sleep(200);
                }
                _view.ShowMyWebcamForm(false);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void OpenVideoForm(string identity)
        {
            try
            {
                _syncVideoCaptureActivity.Reset();
                if (!_view.IsRoomActivated(identity, GenericEnums.RoomType.Video))
                {
                    bool formOpened = false;
                    Thread t = new Thread(delegate()
                    {
                        try
                        {
                            //IntPtr handle = IntPtr.Zero;
                            FormVideoRoom videoRoom = new FormVideoRoom(identity);
                            _view.RoomManager.AddRoom(identity, videoRoom);
                            // initialize new video  form

                            PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                            peers.VideoSessionState = GenericEnums.SessionState.Opened;

                            ContactBase contact = _model.GetContact(identity);
                            // get friendly name from contacts list
                            _view.RoomManager.SetPartnerName(identity, GenericEnums.RoomType.Video, ((Contact)contact).FriendlyName);
                            // finally, show the video  form where we'll see the webcam captures
                            formOpened = true;
                            _view.RoomManager.ShowRoom(identity, GenericEnums.RoomType.Video);
                        }
                        catch (Exception ex)
                        {
                            Tools.Instance.Logger.LogError(ex.ToString());
                        }
                    }
                    );
                    t.IsBackground = true;
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    while (formOpened == false)
                    {
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(200);
                    _syncVideoCaptureActivity.Set();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
