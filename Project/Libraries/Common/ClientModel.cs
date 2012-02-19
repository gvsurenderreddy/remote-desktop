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
using DesktopSharingViewer;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;

namespace Common
{
    [Serializable]
    public class ClientModel : IClientModel
    {
        #region members

        private int _id = -1;
        string _ip;
        string _hostname;
        bool _connected;
        string _serverHost;
        ArrayList _observers = new ArrayList();
        CommandQueue _commands;
        IServerModel _singletonServer;

        IDesktopViewer _viewer;

        #endregion

        #region c-tor

        public ClientModel()
        {

        }

        public ClientModel(string localIP, string serverHost, DesktopViewer.DesktopChangedEventHandler onDesktopChanged)
        {
            _connected = false;
            _id = -1;
            _serverHost = serverHost;
            _ip = localIP;
            _hostname = Dns.GetHostName();
            _commands = new CommandQueue();
            _singletonServer = (IServerModel)Activator.GetObject(typeof(IServerModel), _serverHost);

            _viewer = new DesktopViewer( onDesktopChanged);
        }

        #endregion

        #region IClientModel Members

        //int testNo = 1;

        public void UpdateDesktop(Rectangle rect)
        {
            if (_singletonServer.CheckClientStatus(_id))
            {
                byte[] packed = _singletonServer.UpdateDesktop();

                //System.Drawing.Image partialDesktop;
                //System.Drawing.Rectangle rect2;
                //Guid id;
                if (packed != null)
                {
                    //DesktopSharingViewer.DesktopViewerUtils.Deserialize(packed, out partialDesktop, out rect2, out id);

                    //partialDesktop.Save("c:/test/test" + testNo.ToString() + "Received.bmp");
                    //testNo++;

                    _viewer.UpdateDesktop(packed);
                    NotifyObservers();
                }
            }
            else
            {
                Disconnect(true);
            }
        }

        public void UpdateMouseCursor(ref int x, ref int y)
        {
            if (_singletonServer.CheckClientStatus(_id))
            {
                byte[] packed = _singletonServer.UpdateMouseCursor();
                if (packed != null)
                {
                    _viewer.UpdateMouse(packed);
                    NotifyObservers();
                }
            }
            else
            {
                Disconnect(true);
            }
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
                try
                {
                    //if (_singletonServer == null)
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
                _singletonServer.RemoveClient(_id, checkStatus);
                _connected = false;
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

        #endregion

        #region methods

       

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
