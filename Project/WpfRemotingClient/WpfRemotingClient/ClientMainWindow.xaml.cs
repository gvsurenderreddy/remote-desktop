using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using System.Reflection;
using log4net;
using System.Configuration;
using System.Runtime.Remoting;
using System.Timers;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using DesktopSharing;

namespace WpfRemotingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //[Serializable]
    public partial class ClientMainWindow : Window, IClientView
    {
        #region members

        IClientModel _clientModel;
        IClientControl _clientControl;
        ILog _logger;

        //private Thread _threadScreen;
        //private Thread _threadCursor;
        //private bool _stopping;
        //private int _numByteFullScreen;

        #endregion

        #region c-tor

        public ClientMainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
                //_stopping = false;
                //_numByteFullScreen = 1;
                int timerInterval = int.Parse(ConfigurationManager.AppSettings["timerInterval"]);
                string serverHost = txtServer.Content.ToString();
                string localIP = ConfigurationManager.AppSettings["localIP"];
                _clientModel = new RemotingClient(timerInterval, localIP, serverHost);
                _clientControl = new ClientControl(_clientModel, this);
                WireUp(_clientControl, _clientModel);
                Update(_clientModel);

                //_threadScreen = new Thread(new ThreadStart(DesktopThread));
                //_threadCursor = new Thread(new ThreadStart(MouseThread));
          
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region callbacks

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_clientModel.Connected)
                {
                    Connect();      
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region IClientView Members

        public void Connect()
        {
            try
            {
                _clientControl.RequestConnect();
                if (_clientModel.Connected)
                {
                    //_stopping = false;
                    //_threadCursor.Start();
                    //_threadScreen.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                //_stopping = true;
                //_threadCursor.Join();
                //_threadScreen.Join();
                _clientControl.RequestDisconnect();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        public void Update(IClientModel clientModel)
        {
            this.UpdateInterface(clientModel);
        }

        private void imgDesktop_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // todo: check if imgDesktop has background image
                //Visual v = new
                IInputElement i = (IInputElement)sender;
                System.Windows.Point p = e.GetPosition(i);
                double x = p.X;
                double y = p.Y;
                string data = x + "," + y;
                CommandInfo command = new CommandInfo(CommandUtils.CommandType.Mouse, data);
                _clientControl.RequestAddCommand(command);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region methods

        public System.Windows.Point CorrectGetPosition(Visual relativeTo)
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return relativeTo.PointFromScreen(new System.Windows.Point(w32Mouse.X, w32Mouse.Y));
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);


        System.Windows.Controls.Image ConvertDrawingImageToWPFImage(System.Drawing.Image gdiImg)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            //convert System.Drawing.Image to WPF image
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = WpfBitmap;
            img.Width = 500;
            img.Height = 600;
            img.Stretch = System.Windows.Media.Stretch.Fill;
            return img;
        }

        void SetBackgroundValue(System.Drawing.Image desktop)
        {
            imgDesktop = ConvertDrawingImageToWPFImage(desktop);
        }

        void OnDesktopChanged(System.Drawing.Image desktop)
        {
            if (desktop != null)
            {
                lock (desktop)
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(state =>
                    {
                        Dispatcher.Invoke((Action<System.Drawing.Image>)SetBackgroundValue, desktop);
                    });
                }
            }
        }

        private delegate void UpdateDisplayDelegate(System.Drawing.Image display);
        private void UpdateDisplay(System.Drawing.Image display)
        {

        }

        //private void DesktopThread()
        //{
        //    try
        //    {
        //        System.Drawing.Rectangle rect = System.Drawing.Rectangle.Empty;
        //        while (!_stopping)
        //        {
        //            Bitmap desktopImage = _clientControl.RequestUpdateDesktop(ref rect);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //    }
        //}

        //private void MouseThread()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //    }
        //}

        void ShowError(string errorMessage)
        {
            Utils.UpdateControlContent(Dispatcher,  lblError, errorMessage, Utils.ValueType.String);
        }

        //void TimerTick(object sender, ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        _clientModel.StopTimer();
        //        _clientControl.RequestUpdateDesktop();
        //        _clientControl.RequestUpdateMouseCursor();
        //        UpdateInterface(_clientModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            if (_clientModel != null && _clientModel.Connected)
        //            {
        //                _clientModel.StartTimer();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.Error(ex.Message, ex);
        //        }
        //    }
        //}

        private void UpdateInterface(IClientModel clientModel)
        {
            try
            {
                // todo: update desktop sharing and mouse cursor
                Utils.UpdateControlContent(Dispatcher, lblHostname, "Hostname: " + _clientModel.Hostname, Utils.ValueType.String);
                Utils.UpdateControlContent(Dispatcher, lblId, "ID: " + _clientModel.Id.ToString(), Utils.ValueType.String);
                Utils.UpdateControlContent(Dispatcher, lblIP, "IP: " + _clientModel.Ip, Utils.ValueType.String);
                ShowError(_clientModel.IsServerConfigured ? string.Empty : "Error: Server configuration failed");
                if (!_clientModel.Connected)
                {
                    Utils.UpdateControlContent(Dispatcher, btnConnect, "Connect", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, lblStatus, "Status: Disconnected", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, txtServer, true, Utils.ValueType.Boolean);                   
                }
                else
                {
                    Utils.UpdateControlContent(Dispatcher, lblStatus, "Status: Connected", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, btnConnect, "Disconnect", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, txtServer, false, Utils.ValueType.Boolean);  
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        private void WireUp(IClientControl clientControl, IClientModel clientModel)
        {
            try
            {
                if (_clientModel != null)
                {
                    _clientModel.RemoveObserver(this);
                }
                _clientModel = clientModel;
                _clientControl = clientControl;
                _clientControl.SetModel(_clientModel);
                _clientControl.SetView(this);
                _clientModel.AddObserver(this);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion
    }
}
