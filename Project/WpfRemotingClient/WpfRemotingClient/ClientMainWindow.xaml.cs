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
                string serverHost = txtServer.Content.ToString();
                string localIP = ConfigurationManager.AppSettings["localIP"];
                _clientModel = new RemotingClient(timerInterval, localIP, serverHost, TimerTick);
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
            try
            {
                _clientControl.RequestConnect();
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

        #endregion

        #region methods

        void ShowError(string errorMessage)
        {
            Utils.UpdateControlContent(Dispatcher,  lblError, errorMessage, Utils.ValueType.String);
        }

        void TimerTick(object sender, ElapsedEventArgs e)
        {
            try
            {
                _clientModel.StopTimer();
                _clientControl.RequestUpdateDesktop();
                _clientControl.RequestUpdateMouseCursor();
                UpdateInterface(_clientModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                try
                {
                    if (_clientModel != null && _clientModel.Connected)
                    {
                        _clientModel.StartTimer();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                }
            }
        }

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
