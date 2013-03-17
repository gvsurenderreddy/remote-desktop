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

        private void pbRemote_MouseEvent(object sender, MouseEventArgs e)
        {
            // todo: implement pbRemote_MouseMove
            int cursorX = e.X * pbRemote.BackgroundImage.Width / pbRemote.Width;
            int cursorY = e.Y * pbRemote.BackgroundImage.Height / pbRemote.Height;

            // todo: send the command to the controller

            //CommandInfo cmd = new CommandInfo(CommandInfo.CommandTypeOption.MouseMove, data);
            //ViewerService.Commands.Add(cmd);
        }

        private void pbRemote_DragDrop_or_DragOver(object sender, DragEventArgs e)
        {
            // todo: optional - implement drag&drop send feature
        }

        private void pbRemote_MouseEnter_or_MouseHover_or_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pbRemote_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void pbRemote_DragLeave(object sender, EventArgs e)
        {

        }

        private void pbRemote_Click_or_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
