using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace UIControls
{
    public partial class RemoteControl : UserControl
    {
        public RemoteControl()
        {
            InitializeComponent();
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
                DesktopViewerUtils.Deserialize(screenCapture, out screenImage, out screenBounds, out screenID);
            
            }
            Image finalDisplay = null;
            if (mouseCapture != null)
            {
                // Unpack the data.
                //
                Image cursor;
                int cursorX, cursorY;
                Guid id;
                DesktopViewerUtils.Deserialize(mouseCapture, out cursor, out cursorX, out cursorY, out id);

                finalDisplay = DesktopViewerUtils.AppendMouseToDesktop(screenImage,
                        cursor, cursorX, cursorY);
            }

            Image resized = ImageConverter.ResizeImage(finalDisplay, pbRemote.Width, pbRemote.Height);
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

            pbRemote.Image = ImageConverter.ResizeImage(pbRemote.Image, pbRemote.Width, pbRemote.Height);
        }
    }
}
