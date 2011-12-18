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
using System.Drawing;
using DesktopSharing;

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
        //System.Timers.Timer _timer;
        string _serverHost;
        ArrayList _observers = new ArrayList();
        CommandQueue _commands;
        IServerModel _singletonServer;

        #endregion

        #region c-tor

        public Client()
        {
        }

        public Client(int timerInterval, string localIP, string serverHost)
        {
            _connected = false;
            _id = -1;
            //_timer = new System.Timers.Timer();
            //_timer.Interval = timerInterval;
            _serverHost = serverHost;
            _ip = localIP;
            _hostname = Dns.GetHostName();
            //_timer.Elapsed += timerTick;
            _commands = new CommandQueue();
            _singletonServer = (IServerModel)Activator.GetObject(typeof(IServerModel), _serverHost);
        }

        #endregion

        #region IClientModel Members

        public Bitmap UpdateDesktop(Rectangle rect)
        {
            if (_singletonServer.CheckClientStatus(_id))
            {
                // todo: implement UpdateDesktop
                _singletonServer.UpdateDesktop();
                NotifyObservers();
            }
            else
            {
                Disconnect(true);
            }
            return null;
        }

        public Bitmap UpdateMouseCursor(ref int x, ref int y)
        {
            // todo: implement UpdateMouseCursor
            _singletonServer.UpdateMouseCursor();
            NotifyObservers();
            return null;
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
                //if (_timer != null)
                {
                    //if (_timer.Enabled == false)
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
                            }
                        }
                        else
                        {
                            throw new Exception("Server configuration failed");
                        }
                    }
                    //else
                    //{
                    //    _timer.Stop();
                    //    _timer.Start();
                    //}
                }
                //else
                //{
                //    throw new Exception("Connect failed - Timer not initialized");
                //}
            }
            catch (Exception ex)
            {
                _connected = false;
                throw new Exception(ex.Message, ex);
            }
        }

        public void Disconnect(bool checkStatus)
        {
            try
            {
                //if (_timer != null)
                {
                    //if (_timer.Enabled == true)
                    {
                        //_timer.Stop();
                    }
                    _singletonServer.RemoveClient(_id, checkStatus);
                    _connected = false;
                }
                //else
                {
                //    throw new Exception("Disconnect failed - Timer not initialized");
                }
            }
            catch (Exception ex)
            {
                _connected = false;
                throw new Exception(ex.Message, ex);
            }
        }


        public void AddCommand(DesktopSharing.CommandInfo command)
        {
            _commands.AddCommand(command);
        }


        //public void StartTimer()
        //{
        //    if(_timer != null)
        //    {
        //        _timer.Start();
        //    }
        //    else
        //    {
        //        throw new Exception("StartTimer failed - Timer not initialized");
        //    }
        //}

        //public void StopTimer()
        //{
        //    if (_timer != null)
        //    {
        //        _timer.Stop();
        //    }
        //    else
        //    {
        //        throw new Exception("StopTimer failed - Timer not initialized");
        //    }
        //}

        #endregion

        #region proprieties

        public bool IsServerConfigured
        {
            get { return _singletonServer != null; }
        }

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
