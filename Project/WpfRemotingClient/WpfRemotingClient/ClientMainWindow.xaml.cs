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
using DesktopSharingViewer;

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

        Dictionary<string, System.Drawing.Image> _views = new Dictionary<string, System.Drawing.Image>();

        Thread _threadMouse;
        Thread _threadDesktop;
        bool _stopping;

        #endregion

        #region c-tor

        public ClientMainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());

                string serverHost = txtServer.Text.ToString();
                string localIP = ConfigurationManager.AppSettings["localIP"];

                _threadMouse = new Thread(delegate() { MouseThread(); });
                _threadDesktop = new Thread(delegate() { DesktopThread(); });

                _clientModel = new RemotingClient(localIP, serverHost, OnDesktopChanged);
                _clientControl = new ClientControl(_clientModel, this);
                WireUp(_clientControl, _clientModel);
                Update(_clientModel);
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
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
                  MessageBox.Show(ex.ToString());
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
                    _stopping = false;
                    // start receiving screenshots from the server
                    _threadDesktop.Start();
                    _threadMouse.Start();
                }
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        public void Disconnect()
        {
            try
            {
                _stopping = true;
                _threadMouse.Abort();
                _threadDesktop.Abort();
                _clientControl.RequestDisconnect();
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
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
                //if (imgDesktop.Source != null)
                //{
                //    IInputElement i = (IInputElement)sender;
                //    System.Windows.Point p = e.GetPosition(i);
                //    double x = p.X;
                //    double y = p.Y;
                //    string data = x + "," + y;
                //    CommandInfo command = new CommandInfo(CommandUtils.CommandType.Mouse, data);
                //    _clientControl.RequestAddCommand(command);
                //}
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region methods

        private void UpdateTabs(System.Drawing.Image display, string remoteIpAddress)
        {
            //System.Threading.ThreadPool.QueueUserWorkItem(state =>
            //{
                if (!_views.ContainsKey(remoteIpAddress))
                {
                    // add a new tab
                    TabItem tabPage = null;
                    Thread t = new Thread(delegate()
                    {
                        tabPage = new TabItem();
                            // todo: keep the remoteIPaddr in the ViewerContext
                        //page.Name = remoteIpAddress;
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();
                        
                    // todo: use the Dispatcher to add the tab to the UI
                    //if (tabPage != null)
                    //{
                    //    AddTabPage(tabPage);
                    //} 
                }

                // add the new desktop to the dictionary
                _views[remoteIpAddress] = display;

                // update the viewer interface
                Dispatcher.Invoke((Action<System.Drawing.Image>)SetBackgroundValue, display);
            //});
        }

        void OnDesktopChanged(System.Drawing.Image desktop, string remoteIp)
        {
            if (desktop != null)
            {
                lock (desktop)
                {
                    UpdateTabs(desktop, remoteIp);
                }
            }
        }

        void SetBackgroundValue(System.Drawing.Image desktop)
        {
            Utils.ConvertDrawingImageToWPFImage(desktop, ref imgDesktop);
        }

        void AddTabPage(TabItem tabPage)
        {
            //tcViews.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            //new Action(
            //  delegate()
            //  {
            //      tcViews.Items.Add(tabPage);
            //  }));
        }

        private void DesktopThread()
        {
            try
            {
                Thread.Sleep(2000);
                while (!_stopping)
                {
                    System.Drawing.Rectangle rect = System.Drawing.Rectangle.Empty;
                    _clientControl.RequestUpdateDesktop(ref rect);
                }
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        private void MouseThread()
        {
            try
            {
                Thread.Sleep(2000);
                while (!_stopping)
                {
                    int x = 0, y = 0;
                    _clientControl.RequestUpdateMouseCursor(ref x, ref y);
                }
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        void ShowError(string errorMessage)
        {
            Utils.UpdateControlContent(Dispatcher,  lblError, errorMessage, Utils.ValueType.String);
        }

        private void UpdateInterface(IClientModel clientModel)
        {
            try
            {
                Utils.UpdateControlContent(Dispatcher, lblHostname, "Hostname: " + _clientModel.Hostname, Utils.ValueType.String);
                Utils.UpdateControlContent(Dispatcher, lblId, "ID: " + _clientModel.Id.ToString(), Utils.ValueType.String);
                Utils.UpdateControlContent(Dispatcher, lblIP, "IP: " + _clientModel.Ip, Utils.ValueType.String);
                ShowError(_clientModel.IsServerConfigured ? string.Empty : "Error: Server configuration failed");
                if (!_clientModel.Connected)
                {
                    Utils.UpdateControlContent(Dispatcher, btnConnect, "Connect", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, lblStatus, "Status: Disconnected", Utils.ValueType.String);
                    //Utils.UpdateControlContent(Dispatcher, txtServer, true, Utils.ValueType.Boolean);                   
                }
                else
                {
                    Utils.UpdateControlContent(Dispatcher, lblStatus, "Status: Connected", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, btnConnect, "Disconnect", Utils.ValueType.String);
                    //Utils.UpdateControlContent(Dispatcher, txtServer, false, Utils.ValueType.Boolean);  
                }

                // todo: use the _views member to update the interface

            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
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
                  MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
