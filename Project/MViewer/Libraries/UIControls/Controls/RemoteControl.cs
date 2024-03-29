﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using UIControls;
using GenericObjects;
using CommandHookMonitor;
using System.Threading;
using System.Timers;
using Structures;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UIControls
{
    public partial class RemoteControl : UserControl, IDisposable
    {
        #region private members

        Delegates.HookCommandDelegate _remotingCommand;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseDoubleClick;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseClick;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseDown;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseUp;
        System.Timers.Timer _timer;
        IList<MouseMoveArgs> _commands;
        ManualResetEvent _syncCommands;
        string _partnerIdentity;
        readonly object _syncPictures = new object();
        IDictionary<DateTime, RemoteCapture> _captures;

        #endregion

        #region c-tor

        public RemoteControl() { }

        public RemoteControl(string partnerIdentity)
        {
            try
            {
                InitializeComponent();
                InitializeCommandTypes();
                _partnerIdentity = partnerIdentity;
                _timer = new System.Timers.Timer(1000);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(this.MouseMoveTimerTick);
                _commands = new List<MouseMoveArgs>();
                _captures = new Dictionary<DateTime, RemoteCapture>();
                _syncCommands = new ManualResetEvent(true);
                _timer.Start();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void BindCommandHandler(Delegates.HookCommandDelegate remotingCommand)
        {
            _remotingCommand = remotingCommand;
        }

        public void SetPartnerName(string friendlyName)
        {
            txtPartner.Text = friendlyName;
        }

        public void WireUpEventProvider()
        {
            try
            {
                HookManager.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
                HookManager.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
                HookManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDown);
                HookManager.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
                HookManager.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
                HookManager.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUp);
                HookManager.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
                HookManager.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheel);
            }
            catch(Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void WireDownEventProvider()
        {
            try
            {
                HookManager.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.MouseMove);
                HookManager.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.MouseClick);
                HookManager.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
                HookManager.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.KeyDown);
                HookManager.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.MouseDown);
                HookManager.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.MouseUp);
                HookManager.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.KeyUp);
                HookManager.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
                HookManager.MouseWheel -= new System.Windows.Forms.MouseEventHandler(this.MouseWheel);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateScreen(byte[] screenCapture, byte[] mouseCapture)
        {
            Image screenImage = null;
            
            lock (_syncPictures)
            {
                RemoteCapture toDisplay = new RemoteCapture()
                {
                    ScreeNCapture = screenCapture,
                    MouseCapture = mouseCapture
                };
                if (_captures.Count == 0)
                {
                    this.AddPicture(toDisplay);
                }
                else
                {
                    toDisplay = PopOldestPicture();
                    RemoteCapture newCapture = new RemoteCapture()
                    {
                        ScreeNCapture = screenCapture,
                        MouseCapture = mouseCapture
                    };
                    this.AddPicture(newCapture);
                }
                byte[] uncompressedScreen = Tools.Instance.DataCompression.Decompress(toDisplay.ScreeNCapture);
                byte[] uncompressedMouse = Tools.Instance.DataCompression.Decompress(toDisplay.MouseCapture);

                if (uncompressedScreen != null)
                {
                    Rectangle screenBounds = new Rectangle();
                    Tools.Instance.RemotingUtils.DeserializeDesktopCapture(uncompressedScreen, out screenImage, out screenBounds);
                }
                Image finalDisplay = null;
                if (uncompressedMouse != null)
                {
                    // unpack the data
                    Image cursor;
                    int cursorX, cursorY;
                    Tools.Instance.RemotingUtils.DeserializeMouseCapture(uncompressedMouse, out cursor, out cursorX, out cursorY);

                    finalDisplay = Tools.Instance.RemotingUtils.AppendMouseToDesktop(screenImage,
                            cursor, cursorX, cursorY);
                }

                if (pbRemote.Width > 0 && pbRemote.Height > 0)
                {
                    Image resized = Tools.Instance.ImageConverter.ResizeImage(finalDisplay, pbRemote.Width, pbRemote.Height);
                    pbRemote.Image = resized;
                }
            }
        }

        #endregion

        #region private methods

        void AddPicture(RemoteCapture toAdd)
        {
            _captures.Add(DateTime.Now, toAdd);
        }

        RemoteCapture PopOldestPicture()
        {
            RemoteCapture oldest = _captures[_captures.Keys.Min()];
            _captures.Remove(_captures.Keys.Min());
            return oldest;
        }

        void MouseMoveTimerTick(object sender, ElapsedEventArgs args)
        {
            try
            {
                _timer.Stop();
                _syncCommands.Reset();
                if (_commands != null && _commands.Count > 0)
                {
                    // send serialized mouse move commands
                    MemoryStream stream = new MemoryStream();

                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, _commands);

                    byte[] mouseMoves = stream.GetBuffer();
                    _remotingCommand.BeginInvoke(this, new RemotingCommandEventArgs()
                    {
                        RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                        MouseMoves = mouseMoves,
                        MouseCommandType = GenericEnums.MouseCommandType.Move,
                        Identity = this._partnerIdentity
                    }
                    ,null,null
                    );

                    _commands.Clear();
                }
                _syncCommands.Set();
                _timer.Start();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void InitializeCommandTypes()
        {
            _mouseDoubleClick = new Dictionary<MouseButtons, GenericEnums.MouseCommandType>();
            _mouseDoubleClick.Add(System.Windows.Forms.MouseButtons.Left, GenericEnums.MouseCommandType.DoubleLeftClick);
            _mouseDoubleClick.Add(System.Windows.Forms.MouseButtons.Right, GenericEnums.MouseCommandType.DoubleRightClick);
            _mouseDoubleClick.Add(System.Windows.Forms.MouseButtons.Middle, GenericEnums.MouseCommandType.DoubleMiddleClick);

            _mouseClick = new Dictionary<MouseButtons, GenericEnums.MouseCommandType>();
            _mouseClick.Add(System.Windows.Forms.MouseButtons.Left, GenericEnums.MouseCommandType.LeftClick);
            _mouseClick.Add(System.Windows.Forms.MouseButtons.Right, GenericEnums.MouseCommandType.RightClick);
            _mouseClick.Add(System.Windows.Forms.MouseButtons.Middle, GenericEnums.MouseCommandType.MiddleClick);

            _mouseDown = new Dictionary<MouseButtons, GenericEnums.MouseCommandType>();
            _mouseDown.Add(System.Windows.Forms.MouseButtons.Left, GenericEnums.MouseCommandType.LeftMouseDown);
            _mouseDown.Add(System.Windows.Forms.MouseButtons.Right, GenericEnums.MouseCommandType.RightMouseDown);
            _mouseDown.Add(System.Windows.Forms.MouseButtons.Middle, GenericEnums.MouseCommandType.MiddleMouseDown);

            _mouseUp = new Dictionary<MouseButtons, GenericEnums.MouseCommandType>();
            _mouseUp.Add(System.Windows.Forms.MouseButtons.Left, GenericEnums.MouseCommandType.LeftMouseUp);
            _mouseUp.Add(System.Windows.Forms.MouseButtons.Right, GenericEnums.MouseCommandType.RightMouseUp);
            _mouseUp.Add(System.Windows.Forms.MouseButtons.Middle, GenericEnums.MouseCommandType.MiddleMouseUp);
        }

        bool InPictureBoxArea(int x, int y)
        {
            bool inPictureBoxArea = false;

            var leftUpperCornerScreenPosition = pbRemote.PointToScreen(new Point(0, 0));
            var rightBottomCornerScreenPosition = pbRemote.PointToScreen(new Point(pbRemote.Bounds.Width, pbRemote.Bounds.Height));

            if (x >= leftUpperCornerScreenPosition.X && y >= leftUpperCornerScreenPosition.Y
                && x <= rightBottomCornerScreenPosition.X && y <= rightBottomCornerScreenPosition.Y)
            {
                inPictureBoxArea = true;
            }

            return inPictureBoxArea;
        }

        private void RemoteControl_Resize(object sender, EventArgs e)
        {
            try
            {
                //pnl remote width = remote control width - (547 - 539) = remote control width - 8
                //pnl remote height = remote control height - (416 - 373) = remote control height - 43

                //picture box width = pnl remote width - (539 - 514) = pnl remote width - 25
                //picture box height = pnl remote height - (373 - 349) = pnl remote height - 24

                pnlRemote.Width = this.Width - 8;
                pnlRemote.Height = this.Height - 43;

                pbRemote.Width = pnlRemote.Width - 25;
                pbRemote.Height = pnlRemote.Height - 24;
                if (pbRemote.Width > 0 && pbRemote.Height > 0)
                {
                    pbRemote.Image = Tools.Instance.ImageConverter.ResizeImage(pbRemote.Image, pbRemote.Width, pbRemote.Height);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region Event handlers of particular events

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _remotingCommand.Invoke(this,
                    new RemotingCommandEventArgs()
                    {
                        RemotingCommandType = GenericEnums.RemotingCommandType.Keyboard,
                        KeyboardCommandType = GenericEnums.KeyboardCommandType.KeyDown, // send specific command
                        KeyCode = e.KeyCode,
                        Identity = this._partnerIdentity
                    });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _remotingCommand.Invoke(this,
                    new RemotingCommandEventArgs()
                    {
                        RemotingCommandType = GenericEnums.RemotingCommandType.Keyboard,
                        KeyboardCommandType = GenericEnums.KeyboardCommandType.KeyUp, // send specific command
                        KeyCode = e.KeyCode,
                        Identity = this._partnerIdentity
                    });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                _remotingCommand.Invoke(this,
                    new RemotingCommandEventArgs()
                    {
                        RemotingCommandType = GenericEnums.RemotingCommandType.Keyboard,
                        KeyboardCommandType = GenericEnums.KeyboardCommandType.KeyPress, // send specific command
                        KeyChar = e.KeyChar,
                        Identity = this._partnerIdentity
                    });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (InPictureBoxArea(e.X, e.Y))
                {
                    _syncCommands.WaitOne();
                    HookManager.MouseMove -= new MouseEventHandler(this.MouseMove);
                    double x = 0, y = 0;
                    this.GetRemotePosition(ref x, ref y, e.X, e.Y);
                    _commands.Add(new MouseMoveArgs() { X= x, Y = y});
                    HookManager.MouseMove += new MouseEventHandler(this.MouseMove);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (InPictureBoxArea(e.X, e.Y))
                {
                    double x = 0, y = 0;
                    GetRemotePosition(ref x, ref y, e.X, e.Y);
                    _remotingCommand.Invoke(this,
                          new RemotingCommandEventArgs()
                          {
                              RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                              MouseCommandType = _mouseClick[e.Button], // send specific command
                              X = x,
                              Y = y,
                              Identity = this._partnerIdentity
                          });
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (InPictureBoxArea(e.X, e.Y))
                {
                    double x = 0, y = 0;
                    GetRemotePosition(ref x, ref y, e.X, e.Y);
                    _remotingCommand.Invoke(this,
                          new RemotingCommandEventArgs()
                          {
                              RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                              MouseCommandType = _mouseUp[e.Button], // send specific command
                              X = x,
                              Y = y,
                              Identity = this._partnerIdentity
                          });
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (InPictureBoxArea(e.X, e.Y))
                {
                    double x = 0, y = 0;
                    GetRemotePosition(ref x, ref y, e.X, e.Y);
                    _remotingCommand.Invoke(this,
                          new RemotingCommandEventArgs()
                          {
                              RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                              MouseCommandType = _mouseDown[e.Button], // send specific command
                              X = x,
                              Y = y,
                              Identity = this._partnerIdentity
                          });
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (InPictureBoxArea(e.X, e.Y))
                {
                    double x = 0, y = 0;
                    GetRemotePosition(ref x, ref y, e.X, e.Y);
                    _remotingCommand.Invoke(this,
                          new RemotingCommandEventArgs()
                          {
                              RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                              MouseCommandType = _mouseDoubleClick[e.Button], // send specific command
                              X = x,
                              Y = y,
                              Identity = this._partnerIdentity
                          });
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private new void MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (InPictureBoxArea(e.X, e.Y))
                {
                    double x = 0, y = 0;
                    GetRemotePosition(ref x, ref y, e.X, e.Y);
                    _remotingCommand.Invoke(this,
                          new RemotingCommandEventArgs()
                          {
                              RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                              MouseCommandType = GenericEnums.MouseCommandType.Wheel, // send specific command
                              X = x,
                              Y = y,
                              Delta = e.Delta,
                              Identity = this._partnerIdentity
                          });
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void GetRemotePosition(ref double x, ref double y, int localX, int localY)
        {
            try
            {
                var leftUpperCornerScreenPosition = pbRemote.PointToScreen(new Point(0, 0));
                x = Tools.Instance.RemotingUtils.ConvertXToRemote(
                    localX - leftUpperCornerScreenPosition.X,
                    pbRemote.Width);
                y = Tools.Instance.RemotingUtils.ConvertYToRemote(
                    localY - leftUpperCornerScreenPosition.Y,
                    pbRemote.Height);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        new void Dispose()
        {
            this.WireDownEventProvider();
            _timer.Stop();
            _timer.Elapsed -= new System.Timers.ElapsedEventHandler(this.MouseMoveTimerTick); 
            base.Dispose();
            Tools.Instance.Logger.LogInfo("Disposed remoting user control");
        }

        #endregion
    }
}
