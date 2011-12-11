using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Configuration;
using System.Windows;

namespace WpfRemotingServer
{
    public class SingletonServer : MarshalByRefObject, IServerModel
    {
        #region members

        //Dictionary<int, ConnectedClient> _connectedClients;
        //ArrayList _observers;
        //HttpServerChannel _httpChannel;
        bool _isListening = false;
        //string _channelName;
        //int _port;
        //string _host;
        //string _configurationFile;
        //SingletonServer _server;
        //log4net.ILog Logger;

        #endregion

        #region c-tor

        public SingletonServer()
        {
            if (ServerStaticMembers.Observers == null)
            {
                ServerStaticMembers.Observers = new ArrayList();
            }
            if (ServerStaticMembers.ConnectedClients == null)
            {
                ServerStaticMembers.ConnectedClients = new Dictionary<int, ConnectedClient>();
            }
            ServerStaticMembers.ServerModel = this;
            ServerStaticMembers.ServerControl = new ServerControl(ServerStaticMembers.ServerModel, ServerStaticMembers.ServerView);
            ServerStaticMembers.ServerView.WireUp(ServerStaticMembers.ServerControl, ServerStaticMembers.ServerModel);
            _isListening = true;
            ServerStaticMembers.Logger.Info("Remoting Server Initialized");
        }

        #endregion

        #region methods

        public void AddObserver(IServerView serverView)
        {
            if (ServerStaticMembers.Observers.Contains(serverView) == false)
            {
                ServerStaticMembers.Observers.Add(serverView);
            }
        }

        public void RemoveObserver(IServerView serverView)
        {
            if (ServerStaticMembers.Observers.Contains(serverView))
            {
                ServerStaticMembers.Observers.Remove(serverView);
            }
        }

        public void RemoveAllClients()
        {
            ServerStaticMembers.ConnectedClients.Clear();
        }

        public void NotifyObservers()
        {
            foreach (IServerView view in ServerStaticMembers.Observers)
            {
                view.Update(this);
            }
        }

        public int AddClient(string ip, string hostname)
        {
            int newID = ServerStaticMembers.ConnectedClients.Count + 1;
            ServerStaticMembers.ConnectedClients.Add(newID, new ConnectedClient(ip, hostname, newID));
            this.NotifyObservers();
            return newID;
        }

        public void RemoveClient(int id)
        {
            if (ServerStaticMembers.ConnectedClients.ContainsKey(id))
            {
                ServerStaticMembers.ConnectedClients.Remove(id);
                this.NotifyObservers();
            }
        }

        public void StartServer()
        {
            ServerStaticMembers.HttpChannel = new HttpServerChannel(ServerStaticMembers.ChannelName, ServerStaticMembers.Port);
            //RemotingConfiguration.Configure(httpChannel, false);
            ChannelServices.RegisterChannel(ServerStaticMembers.HttpChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), ServerStaticMembers.ChannelName, WellKnownObjectMode.Singleton);
            ServerStaticMembers.ServerModel = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                ServerStaticMembers.Host + ":" + ServerStaticMembers.Port.ToString() + "/SingletonServer");
            _isListening = true;
        }

        public void StopServer()
        {
            RemoveAllClients();
            ChannelServices.UnregisterChannel(ServerStaticMembers.HttpChannel);
            ServerStaticMembers.ServerModel = null;
            _isListening = false;
        }

        public void UpdateDesktop()
        {
            // todo: implement update desktop
            
        }

        public void UpdateMouseCursor()
        {
            // todo: implement update mouse cursor
            
        }

        #endregion

        #region proprieties

        public bool IsListening
        {
            get { return _isListening; }
            set { _isListening = value; }
        }

        public string ChannelName
        {
            get { return ServerStaticMembers.ChannelName; }
        }

        public string Host
        {
            get { return ServerStaticMembers.Host; }
        }

        public int Port
        {
            get { return ServerStaticMembers.Port; }
        }

        public int ConnectedClients
        {
            get { return ServerStaticMembers.ConnectedClients.Count; }
        }

        public IList<ConnectedClient> Clients
        {
            get
            {
                var connectedClients = from client in ServerStaticMembers.ConnectedClients
                           where client.Value.Connected == true
                           select client.Value;
            return connectedClients.ToList<ConnectedClient>();
            }
        }

        #endregion
    }
}
