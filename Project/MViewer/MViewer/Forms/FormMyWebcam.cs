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
        #region private members

        WebcamCapture _webcamCapture;
        int _timerInterval;

        #endregion

        #region c-tor

        public FormMyWebcam(int timerInterval)
        {
            try
            {
                _timerInterval = timerInterval;
                InitializeComponent();
                _webcamCapture = new WebcamCapture(_timerInterval, this.Handle);
                _webcamCapture.ParentForm = this;
                Program.Controller.StartVideo(_webcamCapture);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        /// <summary>
        /// flag used to tell the webcapturing thread to end it's activity
        /// </summary>
        public bool WebcaptureClosed
        {
            get { return _webcamCapture.WebcaptureClosed; }
        }

        #region public methods

        public void SetPicture(Image image)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
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
            try
            {
                if (_webcamCapture == null)
                {
                    _webcamCapture = new WebcamCapture(_timerInterval, this.Handle);
                }
                Program.Controller.StartVideo(_webcamCapture);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
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
            try
            {
                pnlMain.Width = this.Width - 42 - 3;
                pnlMain.Height = this.Height - 61 - 5;

                pbWebcam.Width = pnlMain.Width - 22;
                pbWebcam.Height = pnlMain.Height - 22;

                pbWebcam.Image = Tools.Instance.ImageConverter.ResizeImage(pbWebcam.Image, pbWebcam.Width, pbWebcam.Height);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        
    }
}
