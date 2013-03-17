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
        IMouseCommandInvoker _mouseInvoker;

        public RemoteControl()
        {
            InitializeComponent();
        }

        public void BindMouseHandlers(ControllerMouseHandlers mouseHandlers)
        {
            _mouseInvoker = new MouseCommandInvoker(mouseHandlers);
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
                
            if(screenCapture != null)
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

        bool InPictureBoxArea(int x, int y)
        {
            bool inPictureBoxArea = false;

            var screenPosition1 = pbRemote.PointToScreen(new Point(0, 0));
            var screenPosition2 = pbRemote.PointToScreen(new Point(pbRemote.Bounds.Width, pbRemote.Bounds.Height));


            if (x >= screenPosition1.X && y >= screenPosition1.Y 
                && x <= screenPosition2.X && y <= screenPosition2.Y)
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

        #region Event handlers of particular events

        private void KeyDown(object sender, KeyEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            //textBoxLog.ScrollToCaret();
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("KeyUp - {0}\n", e.KeyCode));
            //textBoxLog.ScrollToCaret();
        }


        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
            //textBoxLog.ScrollToCaret();
        }


        private void MouseMove(object sender, MouseEventArgs e)
        {
            //labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000}", e.X, e.Y);
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (InPictureBoxArea(e.X, e.Y))
            {

            }
            //textBoxLog.AppendText(string.Format("MouseClick - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();
        }


        private void MouseUp(object sender, MouseEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("MouseUp - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();
        }


        private void MouseDown(object sender, MouseEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();
        }


        private void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("MouseDoubleClick - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();
        }


        private void MouseWheel(object sender, MouseEventArgs e)
        {
            //labelWheel.Text = string.Format("Wheel={0:000}", e.Delta);
        }

        #endregion

        
    }
}
