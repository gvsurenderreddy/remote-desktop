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
using Common;
using System.Collections;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window, IServerView
    {
        #region members

        #endregion

        #region c-tor

        public ServerMainWindow()
        {
            try
            {
                InitializeComponent();
                log4net.Config.BasicConfigurator.Configure();
                ServerStaticMembers.Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
                lblStatus.Content = "Status: stopped";
            }
            catch (Exception ex)
            {
                ServerStaticMembers.Logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region methods

        public void WireUp(IServerControl serverControl, IServerModel serverModel)
        {
            if (ServerStaticMembers.ServerModel != null)
            {
                ServerStaticMembers.ServerModel.RemoveObserver(this);
            }
            ServerStaticMembers.ServerModel = serverModel;
            ServerStaticMembers.ServerControl = serverControl;
            ServerStaticMembers.ServerControl.SetModel(ServerStaticMembers.ServerModel);
            ServerStaticMembers.ServerControl.SetView(this);
            ServerStaticMembers.ServerModel.AddObserver(this);
        }

        public void UpdateInterface(IServerModel serverModel)
        {
            if (serverModel.IsListening)
            {
                lblStatus.Content = "Status: started";
                btnConnect.Content = "Stop listening";
            }
            else
            {
                lblStatus.Content = "Status: stopped";
                btnConnect.Content = "Start listening";
            }
            lvClients.Items.Clear();
            if (serverModel != null)
            {
                DisplayClients(serverModel.Clients);
                lblTotal.Content = "Total: " + serverModel.ConnectedClients.ToString();
            }
        }

        public void DisplayClients(IList<ConnectedClient> clients)
        {
            if (clients != null)
            {
                foreach (ConnectedClient client in clients)
                {
                    lvClients.Items.Add(client);
                }
            }
        }

        #endregion

        #region IServerView methods

        public int AddClient(string ip, string hostname)
        {
            return ServerStaticMembers.ServerControl.AddClient(ip, hostname);
        }

        public void RemoveClient(int id)
        {
            ServerStaticMembers.ServerControl.RemoveClient(id);
        }

        public void Update(IServerModel serverModel)
        {
            this.UpdateInterface(serverModel);
        }

        #endregion

        #region callbacks

        public void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServerStaticMembers.HttpChannel = new HttpServerChannel(ServerStaticMembers.ChannelName, ServerStaticMembers.Port);
                //RemotingConfiguration.Configure(httpChannel, false);
                ChannelServices.RegisterChannel(ServerStaticMembers.HttpChannel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), ServerStaticMembers.ChannelName, WellKnownObjectMode.Singleton);
                ServerStaticMembers.ServerModel = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                    ServerStaticMembers.Host + ":" + ServerStaticMembers.Port.ToString() + "/SingletonServer");
                lblStatus.Content = "Status: started";
                btnConnect.Content = "Stop listening";
                //if (ServerStaticMembers.ServerModel.IsListening == false)
                //{
                //    ServerStaticMembers.ServerControl.RequestStartServer();
                //}
                //else
                //{
                //    ServerStaticMembers.ServerControl.RequestStopServer();
                //}
                //UpdateInterface(ServerStaticMembers.ServerModel);

            }
            catch(Exception ex)
            {
                ServerStaticMembers.Logger.Error(ex.Message, ex);
            }
        }

        private void btnCloseConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList<int> toRemove = new List<int>();
                foreach (ListViewItem client in lvClients.SelectedItems)
                {
                    if (((ConnectedClient)client.Content).Connected == true)
                    {
                        toRemove.Add(((ConnectedClient)client.Content).Id);
                        ((ConnectedClient)client.Content).Connected = false;
                    }
                }
                foreach (int id in toRemove)
                {
                    ServerStaticMembers.ServerControl.RemoveClient(id);
                }
            }
            catch (Exception ex)
            {
                ServerStaticMembers.Logger.Error(ex.Message, ex);
            }
        }

        #endregion
    }
}
