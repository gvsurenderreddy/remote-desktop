using BusinessLogicLayer;
using GenericObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace MViewer
{
    public partial class Controller : IController
    {
        readonly object _syncAudioCaptureSending = new object();
        ManualResetEvent _syncAudioCaptureActivity = new ManualResetEvent(true);

        void AudioCaptureObserver(object sender, EventArgs e)
        {
            AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;
            PeerStates peer = _model.SessionManager.GetPeerStatus(args.Identity);

            if (peer.AudioSessionState == GenericEnums.SessionState.Undefined ||
                peer.AudioSessionState == GenericEnums.SessionState.Pending)
            {
                // receiving captures for the first time, have to initalize a form
                ClientConnectedObserver(this,
                       new RoomActionEventArgs()
                       {
                           RoomType = GenericEnums.RoomType.Audio,
                           SignalType = GenericEnums.SignalType.Start,
                           Identity = args.Identity
                       });
                while (peer.AudioSessionState != GenericEnums.SessionState.Opened)
                {
                    Thread.Sleep(2000);
                    peer = _model.SessionManager.GetPeerStatus(args.Identity); // update the peer status
                }
            }

            // check the Audio status before playing the sound
            if (peer.AudioSessionState == GenericEnums.SessionState.Opened)
            {
                _view.RoomManager.PlayAudioCapture(args.Identity, args.Capture);
            }
        }

        public void OnAudioCaptured(object sender, EventArgs e)
        {
            try
            {
                _syncAudioCaptureActivity.WaitOne(); // wait for any room action to end

                if (PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).AudioCaptureClosed() == false)
                {
                    lock (_syncAudioCaptureSending)
                    {
                        AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;

                        // broadcast the audio captures to all connected peers
                        IList<string> connectedSessions = _model.SessionManager.GetConnectedSessions(GenericEnums.RoomType.Audio);
                        if (connectedSessions.Count == 0)
                        {
                            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
                        }
                        else
                        {
                            foreach (string receiverIdentity in connectedSessions)
                            {
                                TransferStatusUptading transfer = _model.SessionManager.GetTransferActivity(receiverIdentity);
                                while (transfer.IsAudioUpdating)
                                {
                                    Thread.Sleep(200);
                                }

                                PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(receiverIdentity);
                                // check if the stop signal has been sent from the UI

                                // check if the stop signal has been sent by the partner

                                PeerStates peers = _model.SessionManager.GetPeerStatus(receiverIdentity);
                                if (peers.AudioSessionState == GenericEnums.SessionState.Opened
                                    || peers.AudioSessionState == GenericEnums.SessionState.Pending)
                                {
                                    // send the capture if the session isn't paused
                                    transferStatus.Audio = true;

                                    _model.ClientController.SendAudioCapture(args.Capture, receiverIdentity,
                                        _model.Identity.MyIdentity);
                                }

                                transferStatus.Audio = false;
                               
                            }
                        }
                    }
                }
                else
                {
                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void OpenAudioForm(string identity)
        {
            _syncAudioCaptureActivity.Reset();

            if (!_view.IsRoomActivated(identity, GenericEnums.RoomType.Audio))
            {
                Thread t = new Thread(delegate()
                {
                    //IntPtr handle = IntPtr.Zero;
                    FormAudioRoom AudioRoom = new FormAudioRoom(identity, this.AudioCaptureObserver);
                    _view.RoomManager.AddRoom(identity, AudioRoom);
                    // initialize new Audio  form

                    PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                    peers.AudioSessionState = GenericEnums.SessionState.Opened;

                    Contact contact = _model.GetContact(identity);
                    // get friendly name from contacts list
                    _view.RoomManager.SetPartnerName(identity, contact.FriendlyName);
                    // finally, show the Audio  form where we'll see the webcam captures
                    _view.RoomManager.ShowRoom(identity);

                }
                );
                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                Thread.Sleep(500);
                _syncAudioCaptureActivity.Set();
            }
        }

        public void PauseAudio(object sender, RoomActionEventArgs args)
        {
            _syncAudioCaptureActivity.Reset();

            // use the peer status of the selected room
            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.AudioSessionState = GenericEnums.SessionState.Paused; // pause the Audio 

            _syncAudioCaptureActivity.Set();
        }

        public void ResumeAudio(object sender, RoomActionEventArgs args)
        {
            _syncAudioCaptureActivity.Reset();

            PeerStates peers = _model.SessionManager.GetPeerStatus(args.Identity);
            peers.AudioSessionState = GenericEnums.SessionState.Opened; // resume the Audio 

            _syncAudioCaptureActivity.Set();
        }

        public void StartAudio(object sender, RoomActionEventArgs args)
        {
            // add client session
            OpenAudioForm(args.Identity);

            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }

        public void StopAudio(object sender, RoomActionEventArgs args)
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();

            // todo: remove all active clients
        }
    }
}
