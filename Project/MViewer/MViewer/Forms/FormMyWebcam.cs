using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using GenericDataLayer;

namespace MViewer
{
    public partial class FormMyWebcam : Form
    {
        WebcamCapture _webcamCapture;
        System.Timers.Timer _visibleTimer;

        /// <summary>
        /// flag used to tell the webcapturing thread to end it's activity
        /// </summary>
        public bool WebcaptureClosed
        {
            get { return _webcamCapture.WebcaptureClosed; }
            //set { _webcamCapture.WebcaptureClosed = value; }
        }

        public FormMyWebcam()
        {
            _visibleTimer = new System.Timers.Timer(3000);
            _visibleTimer.Elapsed += new ElapsedEventHandler(VisibleTimerCallback);
            //_visibleTimer.Start();

            InitializeComponent();
            _webcamCapture = new WebcamCapture(SystemConfiguration.TimerInterval, this.Handle.ToInt32());
            _webcamCapture.ParentForm = this;
            Program.Controller.StartVideoChat(_webcamCapture);

        }

        #region public methods

        //public void WebcaptureClosing(object sender, EventArgs args)
        //{
        //    _formClosingEvent.Invoke(null, null);
        //}

        public void SetPicture(Image image)
        {
            if (!_webcamCapture.ThreadAborted)
            {
                Image resized = ImageConverter.ResizeImage(image, pbWebcam.Width, pbWebcam.Height);
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
                _webcamCapture = new WebcamCapture(SystemConfiguration.TimerInterval, this.Handle.ToInt32());
            }
            Program.Controller.StartVideoChat(_webcamCapture);
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

        private void VisibleTimerCallback(object sender, ElapsedEventArgs e)
        {
            _visibleTimer.Stop();

            if (_webcamCapture.WebcaptureClosed)
            {
                if (this.Visible == true)
                {
                    this.Hide();
                }
            }
            else
            {
                if (this.Visible == false)
                {
                    this.Show();
                }
            }
            _visibleTimer.Start();
        }

        private void FormMyWebcam_Resize(object sender, EventArgs e)
        {
            pnlMain.Width = this.Width - 42 - 3;
            pnlMain.Height = this.Height - 61 - 5;

            pbWebcam.Width = pnlMain.Width - 22;
            pbWebcam.Height = pnlMain.Height - 22;

            pbWebcam.Image = ImageConverter.ResizeImage(pbWebcam.Image, pbWebcam.Width, pbWebcam.Height);
        }

        #endregion

        
    }
}
