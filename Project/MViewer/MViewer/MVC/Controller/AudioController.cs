﻿using Abstraction;
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
        ManualResetEvent _syncAudioCaptureActivity = new ManualResetEvent(true);
        readonly object _syncAudioStartStop = new object();

        #endregion

        #region private methods

        void OpenAudioForm(string identity)
        {
            _syncAudioCaptureActivity.Reset();

            if (!_view.IsRoomActivated(identity, GenericEnums.RoomType.Audio))
            {
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
                _syncAudioCaptureActivity.Set();
            }
        }

        void PlayAudioCapture(object sender, EventArgs e)
        {
            try
            {
                AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;
                PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).PlayAudioCapture(args.Capture);
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

                //todo: remove this log
                Tools.Instance.Logger.LogInfo("received capture of " + args.Capture.Length + " bytes");

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
                        Thread.Sleep(2000);
                        peer = _model.SessionManager.GetPeerStatus(args.Identity); // update the peer status
                    }
                }

                // check the Audio status before playing the sound
                if (peer.AudioSessionState == GenericEnums.SessionState.Opened)
                {
                    _view.RoomManager.PlayAudioCapture(args.Identity, args.Capture, args.CaptureTimestamp);
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
                _syncAudioCaptureActivity.WaitOne(); // wait for any room action to end

                if (PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).AudioCaptureClosed == false)
                {
                    lock (_syncAudioCaptureSending)
                    {
                        AudioCaptureEventArgs args = (AudioCaptureEventArgs)e;

                        // send the audio capture to active audio room
                        string receiverIdentity = ((ActiveRooms)_view.RoomManager.ActiveRooms).AudioRoomIdentity;
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
                            || peers.AudioSessionState == GenericEnums.SessionState.Pending && args.Capture.Length > 0)
                        {
                            // send the capture if the session isn't paused
                            transferStatus.Audio = true;

                            //todo: remove this log
                            Tools.Instance.Logger.LogInfo("sending capture of " + args.Capture.Length + " bytes");

                            _model.ClientController.SendAudioCapture(args.Capture, args.CaptureTimestamp,
                                receiverIdentity, ((Identity)_model.Identity).MyIdentity);
                        }

                        transferStatus.Audio = false;
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
                _syncAudioCaptureActivity.Reset();

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
                _syncAudioCaptureActivity.Set();
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
                _syncAudioCaptureActivity.Reset();

                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.AudioSessionState = GenericEnums.SessionState.Opened; // resume the Audio 
                _view.RoomManager.ToggleAudioStatus(e.Identity);

                if (!sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
                {
                    // send the stop signal to the server session
                    _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, e.Identity,
                        GenericEnums.RoomType.Audio, GenericEnums.SignalType.Resume);
                }
                _syncAudioCaptureActivity.Set();
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
                    _syncAudioCaptureActivity.Reset();

                    string identity = e.Identity;

                    _view.RoomManager.CloseRoom(identity, GenericEnums.RoomType.Audio);
                    _view.RoomManager.RemoveRoom(identity, GenericEnums.RoomType.Audio);

                    // tell the partner to pause capturing & sending while processing room Stop command
                    _model.ClientController.WaitRoomButtonAction(identity, ((Identity)_model.Identity).MyIdentity, 
                        GenericEnums.RoomType.Audio, true);

                    TransferStatusUptading transfer = _model.SessionManager.GetTransferActivity(identity);
                    transfer.IsAudioUpdating = true;

                    // check if the webcapture is pending for being sent
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
                            // send the stop signal to the server session
                            _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, identity,
                                GenericEnums.RoomType.Audio, GenericEnums.SignalType.Stop);
                        }

                        // remove the connected client session
                        _model.SessionManager.RemoveSession(identity);

                        _view.UpdateLabels(e.Identity, e.RoomType);
                    }

                    // tell the partner to pause capturing & sending while processing room Stop command
                    _model.ClientController.WaitRoomButtonAction(identity, ((Identity)_model.Identity).MyIdentity,
                        GenericEnums.RoomType.Audio, false);

                    if (_view.RoomManager.RoomsLeft(GenericEnums.RoomType.Audio) == false)
                    {
                        PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();
                        _view.ResetLabels(e.RoomType);
                    }

                    OnActiveRoomChanged(string.Empty, GenericEnums.RoomType.Undefined);
                    transfer.IsAudioUpdating = false;
                    _syncAudioCaptureActivity.Set();
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
