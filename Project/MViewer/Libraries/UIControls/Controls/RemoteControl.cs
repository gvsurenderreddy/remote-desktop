using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public void SetPicture(Image picture)
        {
            Image resized = ImageConverter.ResizeImage(picture, pbRemote.Width, pbRemote.Height);
            pbRemote.Image = resized;
        }
    }
}
