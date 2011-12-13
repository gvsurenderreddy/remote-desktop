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
using System.Windows.Threading;
using Microsoft.JScript;
using System.Threading;
using System.Collections.ObjectModel;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window, IServerView
    {
        #region members

        Action EmptyDelegate = delegate() { };

        #endregion

        #region c-tor

        public ServerMainWindow()
        {
            try
            {
                InitializeComponent();
                
                lblStatus.Content = "Status: started";
                btnStartServer.Content = "Stop listening";
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
            if (serverModel != null)
            {
                if (serverModel.IsListening)
                {
                    UpdateControlContent(lblStatus, "Status: started");
                    UpdateControlContent(btnStartServer, "Stop listening");
                }
                else
                {
                    UpdateControlContent(lblStatus, "Status: stopped");
                    UpdateControlContent(btnStartServer, "Start listening");
                }
                ClearItems();
                DisplayClients(serverModel.Clients);
                UpdateControlContent(lblTotal, "Total: " + serverModel.ConnectedClients.ToString());
            }
        }

        public void DisplayClients(ObservableCollection<ConnectedClient> clients)
        {
            if (clients != null)
            {
                lvClients.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    lvClients.ItemsSource = clients;
                }));
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
                if (ServerStaticMembers.ServerModel.IsListening)
                {
                    ServerStaticMembers.ServerControl.RequestStopServer();
                }
                else
                {
                    ServerStaticMembers.ServerControl.RequestStartServer();
                }
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

        #region thread safe methods

        void SetValue(ContentControl control, string newContent)
        {
            control.Content = newContent;
        }

        void UpdateControlContent(ContentControl control, string newContent)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                Dispatcher.Invoke((Action<ContentControl, string>)SetValue, control, newContent);
            });
        }

        void ClearItems()
        {
            lvClients.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                lvClients.Items.Clear();
            }));
        }

        //void AddItem(ConnectedClient client)
        //{
        //    lvClients.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
        //    {
        //        lvClients.Items.Add(client);
        //    }));
        //}

        #endregion
    }
}
