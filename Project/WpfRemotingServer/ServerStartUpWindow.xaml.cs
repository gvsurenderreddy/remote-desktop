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
using System.Windows.Shapes;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using Common;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for ServerStartUpWindow.xaml
    /// </summary>
    public partial class ServerStartUpWindow : Window
    {
        public ServerStartUpWindow()
        {
            InitializeComponent();
            try
            {
                ServerStaticMembers.HttpChannel = new HttpServerChannel(ServerStaticMembers.ChannelName, ServerStaticMembers.Port);
                //RemotingConfiguration.Configure(httpChannel, false);
                ChannelServices.RegisterChannel(ServerStaticMembers.HttpChannel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), ServerStaticMembers.ChannelName, WellKnownObjectMode.Singleton);
                ServerStaticMembers.ServerModel = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                   "http://" + ServerStaticMembers.Host + ":" + ServerStaticMembers.Port.ToString() + "/SingletonServer");
                lblIP.Content = "IP: " +  ServerStaticMembers.Host;
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }
    }
}
