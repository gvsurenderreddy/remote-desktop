using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BusinessLogicLayer;
using GenericObjects;
using StrategyPattern;
using Utils;
using WindowsInput;
using Abstraction;
using System.IO;
using System.Runtime.Serialization;
using Structures;
using System.Runtime.Serialization.Formatters.Binary;
using Communicator;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region private members

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        readonly object _syncRemotingStartStop = new object();
        readonly object _syncRemotingCaptureSending = new object();
        ManualResetEvent _syncRemotingCaptureActivity = new ManualResetEvent(true);
        KeyCodeParser _keyCodeParser = new KeyCodeParser();
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSE_WHEEL = 0x800;

        #endregion

        #region public methods

        public void KeyDown(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                VirtualKeyCode virtualKeyCode = _keyCodeParser.ParseKeyCode(e.KeyCode);
                WindowsInput.InputSimulator.SimulateKeyDown(virtualKeyCode);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void KeyPress(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                VirtualKeyCode virtualKeyCode = _keyCodeParser.ParseKeyCode(e.KeyCode);
                WindowsInput.InputSimulator.SimulateKeyPress(virtualKeyCode);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void KeyUp(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                VirtualKeyCode virtualKeyCode = _keyCodeParser.ParseKeyCode(e.KeyCode);
                WindowsInput.InputSimulator.SimulateKeyUp(virtualKeyCode);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void LeftClickCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                    SetCursorPos((int)x, (int)y);
                    //  perform right click
                    mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        public void DoubleRightClickCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                SetCursorPos((int)x, (int)y);
                mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void DoubleLeftClickCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                SetCursorPos((int)x, (int)y);
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void MiddleClickCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                SetCursorPos((int)x, (int)y);
                mouse_event(MOUSEEVENTF_MIDDLEDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_MIDDLEUP, x, y, 0, 0);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void DoubleMiddleClickCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                SetCursorPos((int)x, (int)y);
                mouse_event(MOUSEEVENTF_MIDDLEDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_MIDDLEUP, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_MIDDLEDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_MIDDLEUP, x, y, 0, 0);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void MouseWheelCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                SetCursorPos((int)x, (int)y);
                mouse_event(MOUSE_WHEEL, x, y, e.Delta, 0);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void MouseMoveCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            try
            {
                MemoryStream stream = new MemoryStream(e.MouseMoves);
                BinaryFormatter formatter = new BinaryFormatter();

                IList<MouseMoveArgs> mouseMoves = (IList<MouseMoveArgs>)formatter.Deserialize(stream);
                foreach (MouseMoveArgs move in mouseMoves)
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(move.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(move.Y);
                    SetCursorPos((int)x, (int)y);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void RightMouseDownCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                    SetCursorPos((int)x, (int)y);
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        public void RightMouseUpCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);
                    SetCursorPos((int)x, (int)y);
                    mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        public void MiddleMouseDownCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);

                    SetCursorPos((int)x, (int)y);
                    mouse_event(MOUSEEVENTF_MIDDLEDOWN, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        public void MiddleMouseUpCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);
                    SetCursorPos((int)x, (int)y);
                    mouse_event(MOUSEEVENTF_MIDDLEUP, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        public void LeftMouseDownCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);
                    SetCursorPos((int)x, (int)y);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        public void LeftMouseUpCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            Thread t = new Thread(delegate()
            {
                try
                {
                    int x = (int)Tools.Instance.RemotingUtils.ConvertXToAbsolute(e.X);
                    int y = (int)Tools.Instance.RemotingUtils.ConvertYToAbsolute(e.Y);
                    SetCursorPos((int)x, (int)y);
                    mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        /// <summary>
        /// method used to execute received remoting command from specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExecuteRemotingCommand(object sender, EventArgs e)
        {
            RemotingCommandEventArgs args = (RemotingCommandEventArgs)e;
            _commandInvoker.PerformCommand(sender, args);
        }

        /// <summary>
        /// method used to send mouse/keyboard remoting command to specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SendRemotingCommand(object sender, EventArgs args)
        {
            try
            {
                _model.ClientController.AddClient(((ActiveRooms)_view.RoomManager.ActiveRooms).RemotingRoomIdentity);
                _model.ClientController.StartClient(((ActiveRooms)_view.RoomManager.ActiveRooms).RemotingRoomIdentity);
                _model.ClientController.SendRemotingCommand(((ActiveRooms)_view.RoomManager.ActiveRooms).RemotingRoomIdentity, args);
                _model.ClientController.RemoveClient(((ActiveRooms)_view.RoomManager.ActiveRooms).RemotingRoomIdentity);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to initiate remoting conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StartRemoting(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                lock (_syncRemotingStartStop)
                {
                    _syncRemotingCaptureActivity.Reset();

                    // conference start permission logic
                    bool hasPermission = true;
                    if (sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
                    {
                        hasPermission = _model.ClientController.ConferencePermission(e.Identity,
                            ((Identity)_model.Identity).MyIdentity, e.RoomType);
                    }
                    if (hasPermission)
                    {
                        // I am going to send my captures by using the below client
                        _model.ClientController.AddClient(e.Identity);
                        _model.ClientController.StartClient(e.Identity);

                        // create client session
                        ClientSession clientSession = new ClientSession(e.Identity, e.RoomType);
                        // save the proxy to which we are sending the remoting captures
                        _model.SessionManager.AddSession(clientSession, GenericEnums.RoomType.Remoting);

                        // initialize the remoting tool and start it's timer
                        PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartRemotingPresentation();
                        clientSession.Peers.RemotingSessionState = GenericEnums.SessionState.Opened;
                        _syncRemotingCaptureActivity.Set();
                    }
                    else
                    {
                        _view.SetMessageText("Remoting conference permission denied");
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to stop remoting conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StopRemoting(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                lock (_syncRemotingStartStop)
                {
                    _syncRemotingCaptureActivity.Reset();

                    TransferStatusUptading transfer = _model.SessionManager.GetTransferActivity(e.Identity);
                    transfer.IsRemotingUpdating = true;

                    // check if the screen capture is pending for being sent
                    PendingTransfer transferStatus = _model.SessionManager.GetTransferStatus(e.Identity);
                    while (transferStatus.Remoting)
                    {
                        // wait for it to finish and block the next sending
                        Thread.Sleep(200);
                    }

                    if (!sender.GetType().IsEquivalentTo(typeof(MViewerServer)))
                    {
                        // send the stop command to the partner
                        _model.ClientController.SendRoomCommand(((Identity)_model.Identity).MyIdentity, 
                            e.Identity, e.RoomType, e.SignalType);
                    }

                    PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                    peers.RemotingSessionState = GenericEnums.SessionState.Closed;
                    _model.SessionManager.RemoveSession(e.Identity);

                    _model.RemoveClient(e.Identity);

                    _view.RoomManager.CloseRoom(e.Identity, GenericEnums.RoomType.Remoting);
                    _view.RoomManager.RemoveRoom(e.Identity, GenericEnums.RoomType.Remoting);

                    if (!_model.SessionManager.RemotingRoomsLeft())
                    {
                        // check if any remoting session is still active
                        PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();
                        _view.ResetLabels(e.RoomType);
                    }

                    // unblock the capture sending
                    transfer.IsRemotingUpdating = false;

                    OnActiveRoomChanged(string.Empty, GenericEnums.RoomType.Undefined);

                    _syncRemotingCaptureActivity.Set();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to resume remoting conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ResumeRemoting(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.RemotingSessionState = GenericEnums.SessionState.Opened; // resume the remoting
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to pause remoting conference with specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void PauseRemoting(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                // use the peer status of the selected room
                PeerStates peers = _model.SessionManager.GetPeerStatus(e.Identity);
                peers.RemotingSessionState = GenericEnums.SessionState.Paused; // pause the remoting
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to send desktop & mouse capture to partner
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void OnRemotingImageCaptured(object source, EventArgs e)
        {
            // binded RemotingImageCaptured to the remoting capture object
            try
            {
                _syncRemotingCaptureActivity.WaitOne(); // wait for any room action to end

                if (PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).RemotingCaptureClosed() == false)
                {
                    lock (_syncRemotingCaptureSending)
                    {
                        // send the remoting capture to active remoting room
                        RemotingCaptureEventArgs args = (RemotingCaptureEventArgs)e;
                        string receiverIdentity= ((ActiveRooms)_view.RoomManager.ActiveRooms).RemotingRoomIdentity;
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

                            _model.ClientController.SendRemotingCapture(args.ScreenCapture,
                                args.MouseCapture, receiverIdentity,
                                ((Identity)_model.Identity).MyIdentity);
                        }

                        transferStatus.Remoting = false;
                    }
                }
                else
                {
                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        void OpenRemotingForm(string identity)
        {
            _syncRemotingCaptureActivity.Reset();

            if (!_view.IsRoomActivated(identity, GenericEnums.RoomType.Remoting))
            {
                Thread t = new Thread(delegate()
                {
                    try
                    {
                        //IntPtr handle = IntPtr.Zero;
                        FormRemotingRoom remotingRoom = new FormRemotingRoom(identity);
                        _view.RoomManager.AddRoom(identity, remotingRoom);
                        remotingRoom.BindCommandHandlers(SystemConfiguration.Instance.RemotingCommand);
                        // initialize new remoting  form

                        Contact contact = (Contact)_model.GetContact(identity);
                        // get friendly name from contacts list
                        if (contact != null)
                        {
                            _view.RoomManager.SetPartnerName(identity, GenericEnums.RoomType.Remoting, contact.FriendlyName);
                        }
                        // I am going to send my captures by using the below client

                        // create session
                        Session clientSession = new ClientSession(identity, GenericEnums.RoomType.Remoting);
                        _model.SessionManager.AddSession(clientSession, GenericEnums.RoomType.Remoting);

                        PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                        peers.RemotingSessionState = GenericEnums.SessionState.Opened;

                        // finally, show the video  form where we'll see the webcam captures
                        _view.RoomManager.ShowRoom(identity, GenericEnums.RoomType.Remoting);
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
                _syncRemotingCaptureActivity.Set();

            }
        }

        /// <summary>
        /// method used to display received mouse & desktop remoting capture from specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemotingCaptureObserver(object sender, EventArgs e)
        {
            try
            {
                RemotingCaptureEventArgs args = (RemotingCaptureEventArgs)e;
                PeerStates peer = _model.SessionManager.GetPeerStatus(args.Identity);

                if (peer.RemotingSessionState == GenericEnums.SessionState.Undefined ||
                    peer.RemotingSessionState == GenericEnums.SessionState.Pending)
                {
                    // receiving captures for the first time, have to initalize a form
                    OpenRemotingForm(args.Identity);
                    while (peer.RemotingSessionState != GenericEnums.SessionState.Opened)
                    {
                        Thread.Sleep(2000);
                        peer = _model.SessionManager.GetPeerStatus(args.Identity); // update the peer status
                    }
                }

                // check the video status before displaying the picture
                if (peer.RemotingSessionState == GenericEnums.SessionState.Opened)
                {
                    //display the remoting capture in the opened form
                    _view.RoomManager.ShowRemotingCapture(args.Identity, args.ScreenCapture, args.MouseCapture);
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
