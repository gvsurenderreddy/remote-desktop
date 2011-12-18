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

        #endregion

        #region c-tor

        public ClientMainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
                int timerInterval = int.Parse(ConfigurationManager.AppSettings["timerInterval"]);
                string serverHost = txtServer.Text;
                _clientModel = new RemotingClient(timerInterval, serverHost, TimerTick);
                _clientControl = new ClientControl(_clientModel, this);
                WireUp(_clientControl, _clientModel);
                Update(_clientModel);
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
            _clientControl.RequestConnect();
        }

        public void Disconnect()
        {
            _clientControl.RequestDisconnect();
        }

        public void Update(IClientModel clientModel)
        {
            this.UpdateInterface(clientModel);
        }

        #endregion

        #region methods

        void TimerTick(object sender, ElapsedEventArgs e)
        {
            _clientControl.RequestUpdateDesktop();
            _clientControl.RequestUpdateMouseCursor();
            UpdateInterface(_clientModel);
        }

        private void UpdateInterface(IClientModel clientModel)
        {
            // todo: update desktop sharing and mouse cursor
            lblHostname.Content = "Hostname: " + _clientModel.Hostname;
            lblId.Content = "ID: " + _clientModel.Id.ToString();
            lblIP.Content = "IP: " + _clientModel.Ip;
            
            if (!_clientModel.Connected)
            {
                btnConnect.Content = "Connect";
                lblStatus.Content = "Status: Disconnected";
            }
            else
            {
                lblStatus.Content = "Status: Connected";
                btnConnect.Content = "Disconnect";
            }
        }

        private void WireUp(IClientControl clientControl, IClientModel clientModel)
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

        #endregion
    }
}
