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
        /* todo: If you want to allow the user to change the display size and 
         * color format of the video capture, call:
         * SendMessage (mCapHwnd, WM_CAP_DLG_VIDEOFORMAT, 0, 0);
         * You will need to requery the capture device to get the new settings
        */

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
        //Mutex _mutex = new Mutex(true, "WebCapture");

        #endregion

        #region public members

        // event delegate fired when a new image is captured by the webcam device
        public delegate void WebCamEventHandler(object source, VideoCaptureEventArgs e);
        public event WebCamEventHandler ImageCaptured;

        #endregion

        #region c-tor & d-tor

        public WebcamCapture(int interval, int windowHandle)
        {
            _components = new Container();
            // set the timer interval
            _timer = new System.Windows.Forms.Timer(_components);
            _timerRunning = false;
            _windowHandle = windowHandle;
            _timer.Interval = interval;
            _timer.Tick += new EventHandler(TimerTick);
        }

        ~WebcamCapture()
        {
            StopCapturing();
        }

        #endregion

        #region private methods

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

        #endregion

        #region public methods

        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern bool SendMessage(int hWnd, uint wMsg, int wParam, int lParam);

        public void StartCapturing()
        {
            // make sure that the capturing is stopped
            StopCapturing();

            // setup a capture window
            _captureWindowHandler = Win32APIMethods.capCreateCaptureWindowA("WebCap", 0, 0, 0, _width, _height, _windowHandle, 0);

            // connect to the capture device
            //Application.DoEvents();
            
            //Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_CONNECT, 0, 0);

            int connectAttempts = 0;
            while (!SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_CONNECT, 0, 0))
            {    
                connectAttempts++;
                //if(connectAttempts > 10)
                //{
                //    DestroyWindow(hHwnd)
                //    Me.Cursor = Cursors.Default
                //    Return False
                //}
                Thread.Sleep(1000);
            }
            Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_SET_PREVIEW, 0, 0);

            // set the frame number
            //m_FrameNumber = FrameNum;

            // set the timer information
            _timerRunning = true;
            _timer.Start();
            _timer.Enabled = true;
        }

        public void StopCapturing()
        {
            try
            {
                // stop the timer
                if (_timerRunning)
                {
                    _timer.Stop();
                    // disconnect from the video capturing device
                    // Application.DoEvents();
                    Win32APIMethods.SendMessage(_captureWindowHandler, Win32APIConstants.WM_CAP_DISCONNECT, 0, 0);
                }
                _timerRunning = false;
            }
            catch (ThreadAbortException)
            {
                _threadAborted = true;
            }
        }

        #endregion

        #region private methods

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
                if (!_threadAborted)
                {
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
                            _eventArgs.CapturedImage = ImageConverter.ResizeImage(tempImage, this._width, this._height);
                        }
                        else
                        {
                            //MessageBox.Show("The Data In Clipboard is not as image format");
                        }
                        /* todo: For some reason, the API is not resizing the video
                        * feed to the width and height provided when the video
                        * feed was started, so we must resize the image here
                        */

                        // raise the event
                        this.ImageCaptured(this, _eventArgs);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _threadAborted = true;
                StopCapturing();
            }
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
                if (_timerRunning == false && !_threadAborted)// && error == false)
                {
                    _timerRunning = true;
                    _timer.Start();
                }
            }
        }

        #endregion

        #region proprieties

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
