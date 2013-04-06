using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using GenericObjects;
using System.Drawing.Imaging;
using System.IO;
using Utils;
using System.Collections;

namespace GenericObjects
{
    [Designer("Sytem.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(System.ComponentModel.Design.IDesigner))] // make composite
    public class WebcamCapture : System.Windows.Forms.UserControl
    {
        #region private members

        IContainer _components;
        System.Windows.Forms.Timer _timer;
        Form _parentForm;
        bool _timerRunning;
        int _captureTimespan;
        int _width;
        int _height;
        IntPtr _captureWindowHandler;
        IntPtr _windowHandle;
        VideoCaptureEventArgs _eventArgs;
        bool _threadAborted;
        bool _webcamClosed;
        int _interval;
        ManualResetEvent _syncCaptures = new ManualResetEvent(false);

        #endregion

        #region c-tor 

        public WebcamCapture(int interval, IntPtr windowHandle)
        {
            try
            {
                _components = new Container();
                // set the timer interval
                _interval = interval;
                InitializeTimer(interval);

                _timerRunning = false;
                _threadAborted = false;
                _webcamClosed = false;

                _windowHandle = windowHandle;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        void StartCaptureProcess(bool firstTimeCapturing)
        {
            try
            {
                InitializeTimer(_interval);
                // setup a capture window
                _captureWindowHandler = Win32APIMethods.capCreateCaptureWindowA("WebCap", 0, 0, 0, _width, _height, _windowHandle, 0);

                // connect this application to the capture device
                int connectAttempts = 0;
                while (Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_CONNECT, IntPtr.Zero, IntPtr.Zero) == IntPtr.Zero)
                {
                    connectAttempts++;
                    Thread.Sleep(1000);
                }
                IntPtr x = Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_SET_PREVIEW, IntPtr.Zero, IntPtr.Zero);
                _webcamClosed = false;
                _threadAborted = false;

                // set the timer information
                _timerRunning = true;
                _timer.Start();
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void StartCapturing(bool firstTimeCapturing)
        {
            try
            {
                // make sure that the capturing is stopped
                StopCapturing();
                if (firstTimeCapturing)
                {
                    StartCaptureProcess(firstTimeCapturing);
                }
                else
                {
                    this.ParentForm.Invoke(new Delegates.StartPresenting(StartCaptureProcess), firstTimeCapturing);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void WaitRoomButtonAction(bool wait)
        {
            try
            {
                if (wait)
                {
                    _syncCaptures.Reset();
                }
                else
                {
                    _syncCaptures.Set();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopCapturing()
        {
            try
            {
                _syncCaptures.Reset();
                // stop the timer
                if (_timerRunning || _timer.Enabled)
                {
                    _timer.Stop();
                    _threadAborted = true;
                }
                _timerRunning = false;
            }
            catch (ThreadAbortException)
            {
                _threadAborted = true;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                try
                {
                    if (!_webcamClosed)
                    {
                        // disconnect from the video capturing device
                        Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_DISCONNECT, IntPtr.Zero, IntPtr.Zero);
                        _webcamClosed = true;
                    }
                }
                catch { }
            }
            _syncCaptures.Set();
        }

        #endregion

        #region private methods

        void InitializeTimer(int interval)
        {
            try
            {
                if (_timer == null)
                {
                    _timer = new System.Windows.Forms.Timer(_components);
                    _timer.Interval = interval;
                    _timer.Tag = this;
                    _timer.Tick += new EventHandler(TimerTick);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// capture the next image from the video capturing device
        /// </summary>
        [STAThread]
        private void TimerTick(object sender, System.EventArgs e)
        {
            try
            {
                // pause the timer
                _timer.Stop();

                _syncCaptures.WaitOne();

                if (!_threadAborted && !_webcamClosed)
                {
                    if (!_threadAborted && !_webcamClosed)
                    {
                        while (ParentForm.Visible == false)
                        {
                            Thread.Sleep(1000);
                        }
                        _timerRunning = false;

                        // get the next image
                        Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_GET_FRAME, IntPtr.Zero, IntPtr.Zero);

                        // copy the image to the clipboard
                        Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_COPY, IntPtr.Zero, IntPtr.Zero);
                        
                        // push the image into the capture event args
                        if (ImageCaptured != null)
                        {
                            // get image from the clipboard
                            System.Windows.Forms.IDataObject tempObject = Clipboard.GetDataObject();
                            Image tempImage2 = (System.Drawing.Bitmap)tempObject.GetData(DataFormats.Bitmap);
                            if (tempObject.GetDataPresent(DataFormats.Bitmap, true))
                            {
                                Image tempImage = (Image)tempObject.GetData(DataFormats.Bitmap, true);
                                _eventArgs = new VideoCaptureEventArgs();
                                // resize the image to the required size (the API isn't doing that)

                                // todo: add timestamp to be used for synchron with audio

                                // todo: resize to lower resolution for bandwidth saving
                                _eventArgs.CapturedImage = Tools.Instance.ImageConverter.ResizeImage(tempImage, this._width, this._height);

                                // raise the capture event
                                this.ImageCaptured(this, _eventArgs);
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException taex)
            {
                _threadAborted = true;
                StopCapturing();
                Tools.Instance.Logger.LogError(taex.ToString());
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                try
                {
                    if (_timerRunning == false && !_threadAborted && !_webcamClosed)
                    {
                        _timerRunning = true;
                        _timer.Start();
                    }
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            }
        }

        #endregion

        #region proprieties

        public event Delegates.WebCamEventHandler ImageCaptured;

        public new Form ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public bool WebcaptureClosed
        {
            get { return _webcamClosed; }
        }

        public bool ThreadAborted
        {
            get { return _threadAborted; }
        }

        public int CaptureTimespan
        {
            set { _captureTimespan = value; }
        }

        public int ImageHeight
        {
            set { _height = value; }
        }

        public int ImageWidth
        {
            set { _width = value; }
        }

        #endregion
    }
}
