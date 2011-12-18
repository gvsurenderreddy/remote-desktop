using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Timers;
using System.IO;

namespace Common
{
    [Serializable]
    public  class Client : IClientModel
    {
        #region members

        private int _id = -1;
        string _ip;
        string _hostname;
        bool _connected;
        System.Timers.Timer _timer;
        string _serverHost;
        ArrayList _observers = new ArrayList();
        IServerModel _singletonServer;

        #endregion

        #region c-tor

        public Client()
        {
        }

        public Client(int timerInterval, string serverHost, ElapsedEventHandler timerTick)
        {
            _connected = false;
            _id = -1;
            _timer = new System.Timers.Timer();
            _timer.Interval = timerInterval;
            _serverHost = serverHost;
            // todo: get local ip
            _ip = "my ip";
            _hostname = Dns.GetHostName();
            _timer.Elapsed += timerTick;
        }

        #endregion

        #region IClientModel Members

        public void UpdateDesktop()
        {
            // todo: implement UpdateDesktop
            _singletonServer.UpdateDesktop();
            _singletonServer.UpdateMouseCursor();
            NotifyObservers();
        }

        public void UpdateMouseCursor()
        {
            // todo: implement UpdateMouseCursor
            NotifyObservers();
        }

        public void AddObserver(IClientView clientView)
        {
            _observers.Add(clientView);
        }

        public void RemoveObserver(IClientView clientView)
        {
            _observers.Remove(clientView);
        }

        public void NotifyObservers()
        {
            foreach (IClientView view in _observers)
            {
                view.Update(this);
            }
        }

        [STAThread]
        public void Connect()
        {
            try
            {
                if (_timer != null)
                {
                    if (_timer.Enabled == false)
                    {
                        try
                        {
                            if (_singletonServer == null)
                            {
                                _singletonServer = (IServerModel)Activator.GetObject(typeof(IServerModel), _serverHost);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Server configuration failed - " + ex.Message, ex);
                        }
                        if (_singletonServer != null)
                        {
                            _id = _singletonServer.AddClient(_ip, _hostname);
                            if (_id != -1)
                            {
                                _connected = true;
                                //_timer.Start();
                            }
                            else
                            {
                                _connected = false;
                                // todo: show connection failed message
                            }
                        }
                        else
                        {
                            // todo: show server configuration failed message
                        }
                    }
                    else
                    {
                        _timer.Stop();
                        _timer.Start();
                    }
                }
                else
                {
                    throw new Exception("Timer not initialized");
                }
            }
            catch (Exception ex)
            {
                _connected = false;
                throw new Exception(ex.Message, ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_timer != null)
                {
                    if (_timer.Enabled == true)
                    {
                        _timer.Stop();
                    }
                    _singletonServer.RemoveClient(_id);
                    _connected = false;
                }
                else
                {
                    throw new Exception("Timer not initialized");
                }
            }
            catch (Exception ex)
            {
                _connected = false;
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region proprieties

        public string Hostname
        {
            get { return _hostname; }
        }

        public string Ip
        {
            get { return _ip; }
        }

        public int Id
        {
            get { return _id; }
        }

        public bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        }

        #endregion
    }
}
