using Abstraction;
using BusinessLogicLayer;
using GenericObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;
using Communicator;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region private members

        readonly object _syncAudioCaptureSending = new object();
        ManualResetEvent _syncAudioConferenceStatus = new ManualResetEvent(true);
        readonly object _syncAudioStartStop = new object();

        #endregion

        #region private methods

        void BroadcastAudioCaptures(IList<string> connectedSessions, byte[] capture, DateTime timestamp)
        {
            if (connectedSessions.Count == 0)
            {
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
            }
            else
            {
                foreach (string receiverIdentity in connectedSessions)
                {
                    Thread t = new Thread(delegate()
                    {
                        try
                        {
                            ConferenceStatus transfer = _model.SessionManager.GetConferenceStatus(receiverIdentity);
                            while (transfer.IsAudioStatusUpdating)
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

                                TimeSpan timespan = TimeSpan.FromMilliseconds(SystemConfiguration.Instance.PresenterSettings.AudioTimerInterval);
                                _model.ClientController.SendAudioCapture(capture, timestamp,
                                    receiverIdentity, _model.Identity.MyIdentity,
                                    timespan.TotalSeconds * 2);
                            }

                            transferStatus.Audio = false;
                        }
                        catch (Exception ex)
                        {
                            Tools.Instance.Logger.LogError(ex.ToString());
                        }
                    });
                    t.Start();
                }
            }
        }

        void OpenAudioForm(string identity)
        {
            if (!_view.IsRoomActivated(identity, GenericEnums.RoomType.Audio))
            {
                _syncAudioConferenceStatus.Reset();

                Thread t = new Thread(delegate()
                {
                    
                    try
                    {
                        Session clientSession = new ClientSession(identity, GenericEnums.RoomType.Audio);
                        _model.SessionManager.AddSession(clientSession, GenericEnums.RoomType.Audio);

                        //IntPtr handle = IntPtr.Zero;
                        FormAudioRoom audioRoom = new FormAudioRoom(identity, this.PlayAudioCapture);
                        _view.RoomManager.AddRoom(identity, audioRoom);
                        // initialize new Audio  form

                        PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                        peers.AudioSessionState = GenericEnums.SessionState.Opened;

                        audioRoom.ToggleAudioStatus();

                        ContactBase contact = _model.GetContact(identity);
                        // get friendly name from contacts list
                        _view.RoomManager.SetPartnerName(identity, GenericEnums.RoomType.Audio, ((Contact)contact).FriendlyName);
                        // finally, show the Audio  form where we'll see the webcam captures
                        _view.RoomManager.ShowRoom(identity, GenericEnums.RoomType.Audio);
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

                Thread.Sleep(500);
                _syncAudioConferenceStatus.Set();
            }
        }

        void PlayAudioCapture(object sender, EventArgs e)
        {
            try
            {
                AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).
                    PlayAudioCapture(args.Capture, args.Identity, args.CaptureLengthInSeconds);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void OnAudioCaptureReceived(object sender, EventArgs e)
        {
            try
            {
                AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;
                PeerStates peer = _model.SessionManager.GetPeerStatus(args.Identity);

                if (peer.AudioSessionState == GenericEnums.SessionState.Undefined ||
                    peer.AudioSessionState == GenericEnums.SessionState.Pending)
                {
                    // receiving captures for the first time, have to initalize a form
                    ClientConnectedObserver(sender,
                           new RoomActionEventArgs()
                           {
                               RoomType = GenericEnums.RoomType.Audio,
                               SignalType = GenericEnums.SignalType.Start,
                               Identity = args.Identity
                           });
                    while (peer.AudioSessionState != GenericEnums.SessionState.Opened)
                    {
                        Thread.Sleep(200);
                        peer = _model.SessionManager.GetPeerStatus(args.Identity); // update the peer status
                    }
                }

                // check the Audio status before playing the sound
                if (peer.AudioSessionState == GenericEnums.SessionState.Opened)
                {
                    _view.RoomManager.PlayAudioCapture(args.Identity, args.Capture, args.CaptureTimestamp,
                        args.CaptureLengthInSeconds);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// method used to send audio capture to partners
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAudioCaptured(object sender, EventArgs e)
        {
            try
            {
                _syncAudioConferenceStatus.WaitOne(); // wait for any room action to end
                AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;

                if (PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).AudioCaptureClosed == false)
                {
                    lock (_syncAudioCaptureSending)
                    {
                        if (SystemConfiguration.Instance.PresenterSettings.PrivateConference)
                        {
                           
                            // send the audio capture to active audio room
                            string receiverIdentity = ((ActiveRooms)_view.RoomManager.ActiveRooms).AudioRoomIdentity;
                            ConferenceStatus transfer = _model.SessionManager.GetConferenceStatus(receiverIdentity);
                            while (transfer.IsAudioStatusUpdating)
                            {
                                Thread.Sleep(200);
                            }

                            PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(receiverIdentity);
                            // check if the stop signal has been sent from the UI

                            // check if the stop signal has been sent by the partner

                            PeerStates peers = _model.SessionManager.GetPeerStatus(receiverIdentity);
                            if (peers.AudioSessionState == GenericEnums.SessionState.Opened
                                || peers.AudioSessionState == GenericEnums.SessionState.Pending && args.Capture.Length > 0)
                            {
                                // send the capture if the session isn't paused
                                transferStatus.Audio = true;

                                TimeSpan timespan = TimeSpan.FromMilliseconds(SystemConfiguration.Instance.PresenterSettings.AudioTimerInterval);
                                _model.ClientController.SendAudioCapture(args.Capture, args.CaptureTimestamp,
                                    receiverIdentity, ((Identity)_model.Identity).MyIdentity,
                                     timespan.TotalSeconds * 2);
                            }

                            transferStatus.Audio = false;
                        }
                        else
                        {
                            IList<string> connectedSessions = _model.SessionManager.GetConnectedSessions(GenericEnums.RoomType.Audio);
                            BroadcastAudioCaptures(connectedSessions, args.Capture, args.CaptureTimestamp);
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
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to pause the audio conference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void PauseAudio(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                _syncAudioConferenceStatus.Reset();

                // use the peer status of the selected room
                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.AudioSessionState = GenericEnums.SessionState.Paused; // pause the Audio 

                _view.RoomManager.ToggleAudioStatus(e.Identity);
                if (!sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
                {
                    // send the stop signal to the server session
                    _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, e.Identity,
                        GenericEnums.RoomType.Audio, GenericEnums.SignalType.Pause);
                }
                _syncAudioConferenceStatus.Set();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to resume audio conference that has been paused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ResumeAudio(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                _syncAudioConferenceStatus.Reset();

                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.AudioSessionState = GenericEnums.SessionState.Opened; // resume the Audio 
                _view.RoomManager.ToggleAudioStatus(e.Identity);

                if (!sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
                {
                    // send the stop signal to the server session
                    _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, e.Identity,
                        GenericEnums.RoomType.Audio, GenericEnums.SignalType.Resume);
                }
                _syncAudioConferenceStatus.Set();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to initiate audio conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StartAudio(object sender, EventArgs args)
        {
            try
            {
                lock (_syncAudioStartStop)
                {
                    RoomActionEventArgs e = (RoomActionEventArgs)args;
                    // conference start permission logic
                    bool hasPermission = true;
                    if (sender.GetType().IsEquivalentTo(typeof(UIControls.ActionsControl)))
                    {
                        hasPermission = _model.ClientController.ConferencePermission(e.Identity,
                            ((Identity)_model.Identity).MyIdentity, e.RoomType);
                    }
                    if (hasPermission)
                    {
                        // add client session
                        OpenAudioForm(((RoomActionEventArgs)args).Identity);
                        PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
                    }
                    else
                    {
                        _view.SetMessageText("Audio conference permission denied");
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to stop audio conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StopAudio(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                lock (_syncAudioStartStop)
                {
                    _syncAudioConferenceStatus.Reset(); // tell my application to pause sending captures

                    string identity = e.Identity;
                    
                    // tell the partner to pause capturing & sending while processing Room Stop command
                    _model.ClientController.WaitRoomButtonAction(identity, ((Identity)_model.Identity).MyIdentity, 
                        GenericEnums.RoomType.Audio, true);
                    ConferenceStatus conference = _model.SessionManager.GetConferenceStatus(identity);
                    conference.IsAudioStatusUpdating = true;

                    // check if any webcapture is pending for being sent
                    PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(identity);
                    while (transferStatus.Audio)
                    {
                        // wait for it to finish and block the next sending
                        Thread.Sleep(200);
                    }

                    PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                    // update the session status to closed
                    if (peers.AudioSessionState != GenericEnums.SessionState.Closed
                        && peers.AudioSessionState != GenericEnums.SessionState.Undefined)
                    {
                        peers.AudioSessionState = GenericEnums.SessionState.Closed;
                        bool sendStopSignal = true;
                        if (sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
                        {
                            sendStopSignal = false;
                        }
                        if (sendStopSignal)
                        {
                            // send the stop signal to the partner
                            _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, identity,
                                GenericEnums.RoomType.Audio, GenericEnums.SignalType.Stop);
                        }

                        // remove the connected client session
                        _model.SessionManager.RemoveSession(identity);
                        _view.UpdateLabels(e.Identity, e.RoomType);
                    }
                    
                    //close the conference form only after the last data chunk has been sent
                    _view.RoomManager.CloseRoom(identity, GenericEnums.RoomType.Audio);
                    _view.RoomManager.RemoveRoom(identity, GenericEnums.RoomType.Audio);
                    
                    // tell the partner to resume capturing & sending (to the other guys in the conference)
                    _model.ClientController.WaitRoomButtonAction(identity, ((Identity)_model.Identity).MyIdentity,
                        GenericEnums.RoomType.Audio, false);

                    if (_view.RoomManager.RoomsLeft(GenericEnums.RoomType.Audio) == false)
                    {
                        // close the audio capturing if there's no partner left in the conference
                        PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
                        _view.ResetLabels(e.RoomType);
                    }

                    this.OnActiveRoomChanged(string.Empty, GenericEnums.RoomType.Undefined);
                    conference.IsAudioStatusUpdating = false;
                    _syncAudioConferenceStatus.Set();
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
