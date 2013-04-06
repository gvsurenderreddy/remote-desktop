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
    [System.Drawing.ToolboxBitmap(typeof(WebcamCapture), "CAMERA.ICO")] // toolbox bitmap
    [Designer("Sytem.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(System.ComponentModel.Design.IDesigner))] // make composite
    public class WebcamCapture : System.Windows.Forms.UserControl
    {
        #region private members

        IContainer _components;
        System.Windows.Forms.Timer _timer;

        delegate void StartPresenting(bool firstTimeCapturing);

        bool _timerRunning;
        int _captureTimespan;
        int _width;
        int _height;
        IntPtr _captureWindowHandler;
        IntPtr _windowHandle;
        VideoCaptureEventArgs _eventArgs;
        bool _threadAborted;
        bool _webcamClosed;

        ManualResetEvent _syncCaptures = new ManualResetEvent(false);

        //Mutex _mutex = new Mutex(true, "WebCapture");

        #endregion

        #region public members

        public event Delegates.WebCamEventHandler ImageCaptured;

        //EventHandler _closingEvent;
        int _interval;

        #endregion

        #region c-tor & d-tor

        public WebcamCapture(int interval, IntPtr windowHandle)
        {
            //_closingEvent = closingEvent;
            _components = new Container();
            // set the timer interval
            _interval = interval;
            InitializeTimer(interval);

            _timerRunning = false;
            _threadAborted = false;
            _webcamClosed = false;

            _windowHandle = windowHandle;

        }

        #endregion

        #region public methods

        void StartCaptureProcess(bool firstTimeCapturing)
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
                this.ParentForm.Invoke(new StartPresenting(StartCaptureProcess), firstTimeCapturing);
            }

        }

        public void WaitRoomButtonAction(bool wait)
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
                        
                        //const int WM_CAP_START = 0x0400;
                        //const int WM_CAP_FILE_SAVEAS = WM_CAP_START + 23;
                        //string filepath = "C:\\RecordedVideo.avi";
                        //IntPtr result = Win32APIMethods.SendMessage(_captureWindowHandler, WM_CAP_FILE_SAVEAS, IntPtr.Zero, filepath);
                        //int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();

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

                                // todo: add timestamp to be used for sinchron with audio

                                // todo: resize to lower resolution for bandwidth saving
                                _eventArgs.CapturedImage = Tools.Instance.ImageConverter.ResizeImage(tempImage, this._width, this._height);

                                // raise the capture event
                                this.ImageCaptured(this, _eventArgs);
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
                if (_timerRunning == false && !_threadAborted && !_webcamClosed)// && error == false)
                {
                    _timerRunning = true;
                    _timer.Start();
                }
                //else
                //{
                //    if (_threadAborted || _webcamClosed)
                //    {
                //        // todo: close/hide the parent web capture form
                //        _closingEvent.Invoke(null, null);
                //    }
                //}
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
