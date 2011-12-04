using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Collections;
using System.Runtime.Remoting;

namespace Common
{
    public  class SingletonServer : MarshalByRefObject, IServerModel
    {
        #region members

        Dictionary<int, ConnectedClient> _connectedClients = new Dictionary<int,ConnectedClient>();
        ArrayList _observers = new ArrayList();
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
            RemotingConfiguration.Configure(_configurationFile, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), _channelName, WellKnownObjectMode.Singleton);
            _server = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                _host + ":" + _port.ToString() + "/IServer");
            _isListening = true;
        }

        public void StopServer()
        {
            RemoveAllClients();
            _server = null;
            _isListening = false;
        }

        public void UpdateDesktop()
        {
            // todo: implement update desktop
            throw new NotImplementedException();
        }

        public void UpdateMouseCursor()
        {
            // todo: implement update mouse cursor
            throw new NotImplementedException();
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
