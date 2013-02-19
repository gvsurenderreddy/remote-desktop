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
using GenericDataLayer;
using System.Drawing.Imaging;
using System.IO;

namespace GenericDataLayer
{
    [System.Drawing.ToolboxBitmap(typeof(WebcamCapture), "CAMERA.ICO")] // toolbox bitmap
    [Designer("Sytem.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(System.ComponentModel.Design.IDesigner))] // make composite
    public class WebcamCapture : System.Windows.Forms.UserControl
    {
        #region private members

        IContainer _components;
        System.Windows.Forms.Timer _timer;


        bool _timerRunning;
        int _captureTimespan;
        int _width;
        int _height;
        int _captureWindowHandler;
        int _windowHandle;
        VideoCaptureEventArgs _eventArgs;
        bool _threadAborted;
        bool _webcamDisconnected;
        bool _webcamPaused;

        ManualResetEvent _sync = new ManualResetEvent(false);
        readonly object _syncDisconnected = new object();
        readonly object _syncPaused = new object();

        //Mutex _mutex = new Mutex(true, "WebCapture");

        #endregion

        #region public members

        // event delegate fired when a new image is captured by the webcam device
        public delegate void WebCamEventHandler(object source, VideoCaptureEventArgs e);
        public event WebCamEventHandler ImageCaptured;

        EventHandler _closingEvent;
        int _interval;

        #endregion

        #region c-tor & d-tor

        public WebcamCapture(int interval, int windowHandle, EventHandler closingEvent)
        {
            _closingEvent = closingEvent;
            _components = new Container();
            // set the timer interval
            _interval = interval;
            InitializeTimer(interval);

            _timerRunning = false;
            _threadAborted = false;
            _webcamDisconnected = false;

            _windowHandle = windowHandle;

        }

        #endregion

        #region public methods

        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern bool SendMessage(int hWnd, uint wMsg, int wParam, int lParam);

        delegate void StartPres(bool firstTimeCapturing);

        void StartCaptureProcess(bool firstTimeCapturing)
        {
            InitializeTimer(_interval);
            // setup a capture window
            _captureWindowHandler = Win32APIMethods.capCreateCaptureWindowA("WebCap", 0, 0, 0, _width, _height, _windowHandle, 0);

            // connect this application to the capture device
            int connectAttempts = 0;
            while (!SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_CONNECT, 0, 0))
            {
                connectAttempts++;
                Thread.Sleep(1000);
            }
            int x = Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_SET_PREVIEW, 0, 0);
            _webcamDisconnected = false;

            // wait for the web cam capture form to be visible
            Form myWebcamForm = this.ParentForm;
            while (!firstTimeCapturing && myWebcamForm.Visible == false)
            {
                Thread.Sleep(1000);
            }

            // set the timer information
            _timerRunning = true;
            _timer.Start();
            _timer.Enabled = true;
        }

        public void PauseCapturing(bool pause)
        {
            lock (_syncPaused)
            {
                _webcamPaused = pause;
            }
        }

        public void StartCapturing(bool firstTimeCapturing)
        {
            // make sure that the capturing is stopped
            StopCapturing();
            if (firstTimeCapturing)
            {
                StartCaptureProcess(firstTimeCapturing);
            }
            else
            {
                this.ParentForm.Invoke(new StartPres(StartCaptureProcess), firstTimeCapturing);
            }

        }

        public void StopCapturing()
        {
            try
            {
                _sync.Reset();
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
            finally
            {
                try
                {
                    lock (_syncDisconnected)
                    {
                        if (!_webcamDisconnected)
                        {
                            // disconnect from the video capturing device
                            Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_DISCONNECT, 0, 0);
                            _webcamDisconnected = true;
                        }
                    }
                }
                catch { }
            }
            _sync.Set();
        }

        #endregion

        #region private methods


        void InitializeTimer(int interval)
        {
            if (_timer == null)
            {
                _timer = new System.Windows.Forms.Timer(_components);
                _timer.Interval = interval;
                _timer.Tag = this;
                _timer.Tick += new EventHandler(TimerTick);
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
                _sync.WaitOne();


                if (!_threadAborted && !_webcamDisconnected)
                {
                    lock (_syncDisconnected)
                    {
                        if (!_threadAborted && !_webcamDisconnected)
                        {
                            while (ParentForm.Visible == false)
                            {
                                Thread.Sleep(1000);
                            }
                            _timerRunning = false;

                            // wait for the clipboard to be unused
                            //_mutex.WaitOne();
                            //_pool.WaitOne();

                            // get the next image
                            Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_GET_FRAME, 0, 0);

                            // copy the image to the clipboard
                            Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_COPY, 0, 0);

                            // push the image into the capture event args
                            if (ImageCaptured != null)
                            {
                                // get image from the clipboard
                                System.Windows.Forms.IDataObject tempObject = Clipboard.GetDataObject();
                                Image tempImage2 = (System.Drawing.Bitmap)tempObject.GetData(DataFormats.Bitmap);
                                if (tempObject.GetDataPresent(DataFormats.Bitmap))
                                {
                                    Image tempImage = (Image)tempObject.GetData(DataFormats.Bitmap, true);
                                    _eventArgs = new VideoCaptureEventArgs();
                                    // resize the image to the required size (the API isn't doing that)
                                    _eventArgs.CapturedImage = ImageConverter.ResizeImage(tempImage, this._width, this._height);

                                    lock (_syncPaused)
                                    {
                                        if (!_webcamPaused)
                                        {
                                            // raise the capture event
                                            this.ImageCaptured(this, _eventArgs);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _threadAborted = true;
                StopCapturing();
                MessageBox.Show("capturing thread aborted");
            }
            //catch (ThreadStateException tsex)
            //{
            //    _timer.Stop();
            //    InitializeTimer(_interval);
            //    StartCapturing(false);
            //}
            catch (Exception)
            {
                //_timerRunning = true;
                //MessageBox.Show("An error ocurred while capturing the video image. The video capture will now be terminated.\r\n\n" + excep.ToString());
                //StopCapturing(); // stop the capturing process
            }
            finally
            {
                // free the clipboard
                //_mutex.ReleaseMutex();

                // restart the timer
                //Application.DoEvents();
                if (_timerRunning == false && !_threadAborted && !_webcamDisconnected)// && error == false)
                {
                    _timerRunning = true;
                    _timer.Start();
                }
                else
                {
                    if (_threadAborted || _webcamDisconnected)
                    {
                        // todo: close the parent web capture form
                        _closingEvent.Invoke(null, null);
                    }
                }
            }
        }

        #endregion

        #region proprieties

        Form _parentForm;

        public new Form ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public bool WebcamDisconnected
        {
            get { return _webcamDisconnected; }
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
