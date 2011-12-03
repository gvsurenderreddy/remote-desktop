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

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region members

        SingletonServer _server;
        HttpServerChannel _serverChannel;
        bool _isListening = false;

        #endregion

        #region c-tor

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                App.Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
                lblStatus.Content = "Status: stopped";
            }
            catch (Exception ex)
            {
                App.Logger.Error(ex.Message, ex);
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
                    _serverChannel = new HttpServerChannel("DesktopSharing", 8089);
                    ChannelServices.RegisterChannel(_serverChannel, false);
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), "DesktopSharing", WellKnownObjectMode.Singleton);
                    _server = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                        "http://localhost:8089/SingletonServer");
                    _isListening = true;
                    lblStatus.Content = "Status: started";
                    btnConnect.Content = "Stop listening";
                }
                else
                {
                    ChannelServices.UnregisterChannel(_serverChannel);
                    _server = null;
                    _isListening = false;
                    lblStatus.Content = "Status: stopped";
                    btnConnect.Content = "Start listening";
                }
            }
            catch(Exception ex)
            {
                App.Logger.Error(ex.Message, ex);
            }
        }

        #endregion
    }
}
