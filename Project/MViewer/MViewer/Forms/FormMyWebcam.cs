using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericDataLayer;

namespace MViewer
{
    public partial class FormMyWebcam : Form
    {
        WebcamCapture _webcamCapture;


        public FormMyWebcam(RoomActionEventArgs e)
        {
            InitializeComponent();
            _webcamCapture = new WebcamCapture(20, this.Handle.ToInt32());
            Program.Controller.StartVideoChat(_webcamCapture, e);
        }

        public void SetPicture(Image image)
        {
            pbWebcam.Image = image;
        }

        #region prorieties

        public WebcamCapture CaptureControl
        {
            get
            {
                return _webcamCapture;
            }
        }

        #endregion
    }
}
