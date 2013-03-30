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
        void AudioCaptureObserver(object sender, EventArgs e)
        {
            // todo: implement AudioCaptureObserver
        }

        public void OnAudioCaptured(object sender, EventArgs e)
        {
            // todo: implement OnAudioCaptured
            try
            {
                _syncAudioCaptureActivity.WaitOne(); // wait for any room action to end

                _audioCapturePending = true;
                if (PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).AudioCaptureClosed() == false)
                {
                    lock (_syncAudioCaptureSending)
                    {
                        AudioEventArgs args = (AudioEventArgs)e;

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
                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //_webcamCapture.StopCapturing();
            }
            finally
            {
                _audioCapturePending = false;
            }
        }

        public void PauseAudio(object sender, RoomActionEventArgs args)
        {
            // todo: implement PauseAudio
        }

        public void ResumeAudio(object sender, RoomActionEventArgs args)
        {
            // todo: implement ResumeAudio
        }

        public void StartAudio(object sender, RoomActionEventArgs args)
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }

        public void StopAudio(object sender, RoomActionEventArgs args)
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }
    }
}
