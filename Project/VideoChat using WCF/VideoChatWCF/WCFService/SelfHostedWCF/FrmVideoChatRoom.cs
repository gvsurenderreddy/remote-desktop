using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SelfHostedWCF
{
    public partial class FrmVideoChatRoom : Form
    {
        public FrmVideoChatRoom()
        {
            InitializeComponent();
        }

        #region public methods

        public void DisplayCapture(byte[] capture)
        {
            if (File.Exists("c:\\receive.bmp"))
            {
                File.Delete("c:\\receive.bmp");
            }
            Image returnImage = ImageConverter.byteArrayToImage(capture);
            //returnImage.Save("c:\\receive.bmp");
            // set the picturebox picture
            this.pbChatRoom.Image = returnImage;
        }

        #endregion

        #region callbacks

        private void FrmVideoChatRoom_Resize(object sender, EventArgs e)
        {
            pbChatRoom.Size = new Size(this.Size.Width - 50, this.Size.Height - 70);
        }

        #endregion
    }
}
