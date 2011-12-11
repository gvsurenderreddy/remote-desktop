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
        string _configurationFile;
        System.Timers.Timer _timer;
        string _serverHost;
        ArrayList _observers = new ArrayList();
        IServerModel _singletonServer;

        #endregion

        #region c-tor

        public Client()
        {
        }

        public Client(int timerInterval, string configurationFile, string serverHost, ElapsedEventHandler timerTick)
        {
            _connected = false;
            _id = -1;
            _configurationFile = configurationFile;
            _timer = new System.Timers.Timer();
            _timer.Interval = timerInterval;
            _serverHost = serverHost;
            _ip = GetLocalIP();
            _hostname = Dns.GetHostName();
            _timer.Elapsed += timerTick;
        }

        #endregion

        #region IClientModel Members

        public void UpdateDesktop()
        {
            _singletonServer.UpdateDesktop();
            _singletonServer.UpdateMouseCursor();
            NotifyObservers();
        }

        public void UpdateMouseCursor()
        {

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
            if (_timer != null)
            {
                if (_timer.Enabled == false)
                {
                    // todo: add SERVER_CONFIGURED flag
                    RemotingConfiguration.Configure(_configurationFile, false);
                    _singletonServer = (IServerModel)Activator.GetObject(typeof(IServerModel), _serverHost);
                    // todo: notify server
                    _id = _singletonServer.AddClient(_ip, _hostname);
                    if (_id != -1)
                    {
                        //_timer.Start();
                    }
                    else
                    {
                        // todo: show connection failed message
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

        public void Disconnect()
        {
            if (_timer != null)
            {
                if (_timer.Enabled == true)
                {
                    _timer.Stop();
                }
                // todo: notify server
            }
            else
            {
                throw new Exception("Timer not initialized");
            }
        }

        #endregion

        #region generic methods

        public string GetLocalIP()
        {
            //string direction;
            //WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            //WebResponse response = request.GetResponse();
            //StreamReader stream = new StreamReader(response.GetResponseStream());
            //direction = stream.ReadToEnd();
            //stream.Close();
            //response.Close();

            ////Search for the ip in the html
            //int first = direction.IndexOf("Address: ") + 9;
            //int last = direction.LastIndexOf("</body>");
            //direction = direction.Substring(first, last - first);

            WebClient webClient = new WebClient();
            return webClient.DownloadString("http://myip.ozymo.com/");

            //string host = Dns.GetHostName();
            //IPHostEntry ip = Dns.GetHostEntry(host);
            //Console.WriteLine(ip.AddressList[0].ToString());

            //return direction;

            //IPHostEntry ihe = Dns.GetHostEntry(Dns.GetHostName());
            //string ipAddr = string.Empty;
            //foreach (IPAddress ip in ihe.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.IsIPv6LinkLocal && !ip.IsIPv6Multicast 
            //        && !ip.IsIPv6SiteLocal && !ip.IsIPv6Teredo && !(ip.AddressFamily == AddressFamily.InterNetworkV6))
            //    {
            //        ipAddr = ip.ToString();
            //        break;
            //    }
            //}
            //return ipAddr;
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
