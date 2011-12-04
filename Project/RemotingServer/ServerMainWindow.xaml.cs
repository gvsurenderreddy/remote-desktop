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
using System.Reflection;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Configuration;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window
    {
        #region members

        SingletonServer _server;
        bool _isListening = false;
        string _channelName;
        int _port;
        string _host;
        log4net.ILog Logger;
        #endregion

        #region c-tor

        public ServerMainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
                lblStatus.Content = "Status: stopped";
                _channelName = ConfigurationManager.AppSettings["channelName"];
                _port = int.Parse(ConfigurationManager.AppSettings["port"]);
                _host = ConfigurationManager.AppSettings["host"];
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region callbacks

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isListening == false)
                {
                    RemotingConfiguration.Configure("RemotingServer.exe.config", false);
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), _channelName, WellKnownObjectMode.Singleton);
                    _server = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                        _host + ":" + _port.ToString() + "/SingletonServer");

                    _isListening = true;
                    lblStatus.Content = "Status: started";
                    btnConnect.Content = "Stop listening";
                }
                else
                {
                    //ChannelServices.UnregisterChannel(_serverChannel);
                    _server = null;
                    _isListening = false;
                    lblStatus.Content = "Status: stopped";
                    btnConnect.Content = "Start listening";
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        #endregion
    }
}
