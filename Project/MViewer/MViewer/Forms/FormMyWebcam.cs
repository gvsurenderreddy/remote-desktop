using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using GenericObjects;
using Utils;

namespace MViewer
{
    public partial class FormMyWebcam : Form
    {
        WebcamCapture _webcamCapture;
        int _timerInterval;

        /// <summary>
        /// flag used to tell the webcapturing thread to end it's activity
        /// </summary>
        public bool WebcaptureClosed
        {
            get { return _webcamCapture.WebcaptureClosed; }
        }

        public FormMyWebcam(int timerInterval)
        {
            _timerInterval = timerInterval;
            InitializeComponent();
            _webcamCapture = new WebcamCapture(_timerInterval, this.Handle.ToInt32());
            _webcamCapture.ParentForm = this;
            Program.Controller.StartVideo(_webcamCapture);
        }

        #region public methods

        public void SetPicture(Image image)
        {
            if (!_webcamCapture.ThreadAborted)
            {
                Image resized = Tools.Instance.ImageConverter.ResizeImage(image, pbWebcam.Width, pbWebcam.Height);
                pbWebcam.Image = resized;
            }
            else
            {
                _webcamCapture.StopCapturing();
                this.Close();
            }
        }

        public void StopCapturing()
        {
            _webcamCapture.StopCapturing();
        }

        public void WaitRoomButtonAction(bool wait)
        {
            _webcamCapture.WaitRoomButtonAction(wait);
        }

        public void StartCapturing()
        {
            if (_webcamCapture == null)
            {
                _webcamCapture = new WebcamCapture(_timerInterval, this.Handle.ToInt32());
            }
            Program.Controller.StartVideo(_webcamCapture);
        }

        #endregion

        #region prorieties

        public WebcamCapture CaptureControl
        {
            get
            {
                return _webcamCapture;
            }
        }

        #endregion

        #region callbacks

        private void FormMyWebcam_Resize(object sender, EventArgs e)
        {
            pnlMain.Width = this.Width - 42 - 3;
            pnlMain.Height = this.Height - 61 - 5;

            pbWebcam.Width = pnlMain.Width - 22;
            pbWebcam.Height = pnlMain.Height - 22;

            pbWebcam.Image = Tools.Instance.ImageConverter.ResizeImage(pbWebcam.Image, pbWebcam.Width, pbWebcam.Height);
        }

        #endregion

        
    }
}
