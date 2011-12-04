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

namespace Common
{
    public class SingletonServer : MarshalByRefObject, IServerModel
    {
        #region members

        Dictionary<int, ConnectedClient> _connectedClients;
        ArrayList _observers;
        HttpServerChannel _httpChannel;
        bool _isListening = false;
        string _channelName;
        int _port;
        string _host;
        string _configurationFile;
        SingletonServer _server;
        //log4net.ILog Logger;

        #endregion

        #region c-tor

        public SingletonServer()
        {
            if (_observers == null)
            {
                _observers = new ArrayList();
            }
            if (_connectedClients == null)
            {
                _connectedClients = new Dictionary<int, ConnectedClient>();
            }
            _channelName = ConfigurationManager.AppSettings["channelName"];
            _port = int.Parse(ConfigurationManager.AppSettings["port"]);
            _host = ConfigurationManager.AppSettings["host"];
            _configurationFile = "WpfRemotingServer.exe.config";
        }

        public SingletonServer(string channelName, string host, int port, string configurationFile)
        {
            try
            {
                _channelName = channelName;
                _port = port;
                _host = host;
                _configurationFile = configurationFile;
                _connectedClients = new Dictionary<int, ConnectedClient>();
                _observers = new ArrayList();
                //Logger.Info("Remoting Server Initialized");
            }
            catch (Exception ex)
            {
                throw new Exception("Server C-tor exception - " + ex.Message, ex);
            }
        }

        #endregion

        #region methods

        public void AddObserver(IServerView clientView)
        {
            _observers.Add(clientView);
        }

        public void RemoveObserver(IServerView clientView)
        {
            _observers.Remove(clientView);
        }

        public void RemoveAllClients()
        {
            _connectedClients.Clear();
        }

        public void NotifyObservers()
        {
            foreach (IServerView view in _observers)
            {
                view.Update(this);
            }
        }

        public int AddClient(string ip, string hostname)
        {
            int newID = _connectedClients.Count + 1;
            _connectedClients.Add(newID, new ConnectedClient(ip, hostname, newID));
            this.NotifyObservers();
            return newID;
        }

        public void RemoveClient(int id)
        {
            if (_connectedClients.ContainsKey(id))
            {
                _connectedClients.Remove(id);
                this.NotifyObservers();
            }
        }

        public void StartServer()
        {
            _httpChannel = new HttpServerChannel(_channelName, _port);
            //RemotingConfiguration.Configure(httpChannel, false);
            ChannelServices.RegisterChannel(_httpChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), _channelName, WellKnownObjectMode.Singleton);
            _server = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                _host + ":" + _port.ToString() + "/SingletonServer");
            _isListening = true;
        }

        public void StopServer()
        {
            RemoveAllClients();
            ChannelServices.UnregisterChannel(_httpChannel);
            _server = null;
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
            get { return _channelName; }
        }

        public string Host
        {
            get { return _host; }
        }

        public int Port
        {
            get { return _port; }
        }

        public int ConnectedClients
        {
            get { return _connectedClients.Count; }
        }

        public IList<ConnectedClient> Clients
        {
            get { var connectedClients = from client in _connectedClients
                           where client.Value.Connected == true
                           select client.Value;
            return connectedClients.ToList<ConnectedClient>();
            }
        }

        #endregion
    }
}
