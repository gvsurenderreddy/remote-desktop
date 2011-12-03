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

namespace WpfRemotingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region members

        Server _singletonServer;
        Client _client;
        ILog _logger;
        int _timerInterval;

        #endregion

        #region c-tor

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
                _timerInterval = int.Parse(ConfigurationManager.AppSettings["timerInterval"]);
                _client = new Client(_timerInterval);
                _singletonServer = (Server)Activator.GetObject(typeof(Server), ConfigurationManager.AppSettings["server"]);
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
                if (_client.Connected)
                {

                    btnConnect.Content = "Connect";
                    lblStatus.Content = "Status: Disconnected";
                }
                else
                {
                    _singletonServer.ShareDesktop(ref _client);
                    lblStatus.Content = "Status: Connected";
                    btnConnect.Content = "Disconnect";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion
    }
}
