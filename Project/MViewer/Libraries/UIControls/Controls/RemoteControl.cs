using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using UIControls;
using GenericDataLayer;
using CommandHookMonitor;

namespace UIControls
{
    public partial class RemoteControl : UserControl
    {
        Delegates.HookCommandDelegate _remotingCommand;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseDoubleClick;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseClick;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseDown;
        Dictionary<MouseButtons, GenericEnums.MouseCommandType> _mouseUp;


        #region c-tor

        public RemoteControl()
        {
            InitializeComponent();
            InitializeCommandTypes();
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
            HookManager.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
            HookManager.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClick);
            HookManager.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            HookManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDown);
            HookManager.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
            HookManager.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
            HookManager.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUp);
            HookManager.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
        }

        public void WireDownEventProvider()
        {
            HookManager.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.MouseMove);
            HookManager.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.MouseClick);
            HookManager.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            HookManager.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.KeyDown);
            HookManager.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.MouseDown);
            HookManager.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.MouseUp);
            HookManager.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.KeyUp);
            HookManager.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(this.MouseDoubleClick);
        }

        public void UpdateScreen(byte[] screenCapture, byte[] mouseCapture)
        {
            Image screenImage = null;

            if (screenCapture != null)
            {
                Rectangle screenBounds = new Rectangle();
                Guid screenID = new Guid();
                Tools.Instance.RemotingUtils.Deserialize(screenCapture, out screenImage, out screenBounds, out screenID);

            }
            Image finalDisplay = null;
            if (mouseCapture != null)
            {
                // Unpack the data.
                //
                Image cursor;
                int cursorX, cursorY;
                Guid id;
                Tools.Instance.RemotingUtils.Deserialize(mouseCapture, out cursor, out cursorX, out cursorY, out id);

                finalDisplay = Tools.Instance.RemotingUtils.AppendMouseToDesktop(screenImage,
                        cursor, cursorX, cursorY);
            }

            Image resized = Tools.Instance.ImageConverter.ResizeImage(finalDisplay, pbRemote.Width, pbRemote.Height);
            pbRemote.Image = resized;
        }

        #endregion

        #region private methods

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
            //pnl remote width = remote control width - (547 - 539) = remote control width - 8
            //pnl remote height = remote control height - (416 - 373) = remote control height - 43

            //picture box width = pnl remote width - (539 - 514) = pnl remote width - 25
            //picture box height = pnl remote height - (373 - 349) = pnl remote height - 24

            pnlRemote.Width = this.Width - 8;
            pnlRemote.Height = this.Height - 43;

            pbRemote.Width = pnlRemote.Width - 25;
            pbRemote.Height = pnlRemote.Height - 24;

            pbRemote.Image = Tools.Instance.ImageConverter.ResizeImage(pbRemote.Image, pbRemote.Width, pbRemote.Height);
        }

        #endregion

        #region Event handlers of particular events

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            _remotingCommand.Invoke(this,
                new RemotingCommandEventArgs()
                {
                    RemotingCommandType = GenericEnums.RemotingCommandType.Keyboard,
                    KeyboardCommandType = GenericEnums.KeyboardCommandType.KeyDown, // send specific command
                    KeyCode = e.KeyCode
                });
            //textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            //textBoxLog.ScrollToCaret();

        }

        private new void KeyUp(object sender, KeyEventArgs e)
        {
            _remotingCommand.Invoke(this,
                new RemotingCommandEventArgs()
                {
                    RemotingCommandType = GenericEnums.RemotingCommandType.Keyboard,
                    KeyboardCommandType = GenericEnums.KeyboardCommandType.KeyUp, // send specific command
                    KeyCode = e.KeyCode
                });
            //textBoxLog.AppendText(string.Format("KeyUp - {0}\n", e.KeyCode));
            //textBoxLog.ScrollToCaret();

        }

        private new void KeyPress(object sender, KeyPressEventArgs e)
        {
            _remotingCommand.Invoke(this,
                new RemotingCommandEventArgs()
                {
                    RemotingCommandType = GenericEnums.RemotingCommandType.Keyboard,
                    KeyboardCommandType = GenericEnums.KeyboardCommandType.KeyPress, // send specific command
                    KeyChar = e.KeyChar
                });
            //textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
            //textBoxLog.ScrollToCaret();

        }


        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (InPictureBoxArea(e.X, e.Y))
            {
                double x = 0, y = 0;
                GetRemotePosition(ref x, ref y, e.X, e.Y);
                _remotingCommand.Invoke(this,
                   new RemotingCommandEventArgs()
                   {
                       RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                       MouseCommandType = GenericEnums.MouseCommandType.Move, // send specific command
                       X = x,
                       Y = y
                   });
                //labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000}", e.X, e.Y);
            }
        }

        private new void MouseClick(object sender, MouseEventArgs e)
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
                          Y = y
                      });
                //textBoxLog.AppendText(string.Format("MouseClick - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }
        }


        private new void MouseUp(object sender, MouseEventArgs e)
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
                          Y = y
                      });
                //textBoxLog.AppendText(string.Format("MouseUp - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }
        }


        private new void MouseDown(object sender, MouseEventArgs e)
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
                          Y = y
                      });
                //textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }
        }

        private new void MouseDoubleClick(object sender, MouseEventArgs e)
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
                          Y = y
                      });
                //textBoxLog.AppendText(string.Format("MouseDoubleClick - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }
        }


        private new void MouseWheel(object sender, MouseEventArgs e)
        {
            if (InPictureBoxArea(e.X, e.Y))
            {
                _remotingCommand.Invoke(this,
                      new RemotingCommandEventArgs()
                      {
                          RemotingCommandType = GenericEnums.RemotingCommandType.Mouse,
                          MouseCommandType = GenericEnums.MouseCommandType.Wheel, // send specific command
                          Delta = e.Delta
                      });
                //labelWheel.Text = string.Format("Wheel={0:000}", e.Delta);
            }
        }

        void GetRemotePosition(ref double x, ref double y, int localX, int localY)
        {
            var leftUpperCornerScreenPosition = pbRemote.PointToScreen(new Point(0, 0));

            x = Tools.Instance.RemotingUtils.ConvertXToRemote(
                localX - leftUpperCornerScreenPosition.X,
                pbRemote.Width);
            y = Tools.Instance.RemotingUtils.ConvertYToRemote(
                localY - leftUpperCornerScreenPosition.Y,
                pbRemote.Height);
        }

        #endregion


    }
}
