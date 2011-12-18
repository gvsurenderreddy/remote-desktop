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
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Collections.ObjectModel;

namespace WpfRemotingServer
{
    public class SingletonServer : MarshalByRefObject, IServerModel
    {
        #region members

        bool _isListening = false;
        BackgroundWorker _worker;
        Dispatcher _dispatcher;
        static ServerMainWindow _smw;
        public delegate void NotifyObserversDelegate();

        #endregion

        #region c-tor

        public SingletonServer()
        {
            try
            {
                if (ServerStaticMembers.Observers == null)
                {
                    ServerStaticMembers.Observers = new ArrayList();
                }
                if (ServerStaticMembers.ConnectedClients == null)
                {
                    ServerStaticMembers.ConnectedClients = new ObservableCollection<ConnectedClient>();
                }
                Thread t = new Thread(ThreadProc);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                Thread.Sleep(5000);
                _dispatcher = _smw.Dispatcher;
                _worker = new BackgroundWorker();
                _worker.WorkerSupportsCancellation = true;
                ServerStaticMembers.ServerModel = this;
                ServerStaticMembers.ServerView = _smw;
                ServerStaticMembers.ServerControl = new ServerControl(ServerStaticMembers.ServerModel, ServerStaticMembers.ServerView);
                ServerStaticMembers.ServerView.WireUp(ServerStaticMembers.ServerControl, ServerStaticMembers.ServerModel);
                ServerStaticMembers.Logger.Info("Remoting Server Initialized");
                _isListening = true;
          
            }
            catch (Exception ex)
            {
                _isListening = false;
                ServerStaticMembers.Logger.Error("Remoting Server Initialization failed - " + ex.Message, ex);
            }
        }  

        #endregion

        #region methods

        public void AddObserver(IServerView serverView)
        {
            if (ServerStaticMembers.Observers != null)
            {
                if (ServerStaticMembers.Observers.Contains(serverView) == false)
                {
                    ServerStaticMembers.Observers.Add(serverView);
                }
            }
        }

        public void RemoveObserver(IServerView serverView)
        {
            if (ServerStaticMembers.Observers != null)
            {
                if (ServerStaticMembers.Observers.Contains(serverView))
                {
                    ServerStaticMembers.Observers.Remove(serverView);
                }
            }
        }

        public void RemoveAllClients()
        {
            ServerStaticMembers.ConnectedClients.Clear();
        }

        public void NotifyObservers()
        {
            if (ServerStaticMembers.Observers != null)
            {
                foreach (IServerView view in ServerStaticMembers.Observers)
                {
                    view.Update(this);
                }
            }
        }

        void ThreadProc()
        {
            try
            {
                _smw = new ServerMainWindow();
                _smw.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [STAThread]
        public int AddClient(string ip, string hostname)
        {
            int newID = ServerStaticMembers.ConnectedClients.Count + 1;
            try
            {
                _dispatcher.Invoke((Action)delegate { ServerStaticMembers.ConnectedClients.Add(new ConnectedClient(ip, hostname, newID)); });
                _worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {
                    try
                    {
                        if (_worker.CancellationPending)
                        {
                            args.Cancel = true;
                            return;
                        }
                        System.Threading.Thread.Sleep(10);
                        NotifyObserversDelegate update3 = new NotifyObserversDelegate(NotifyObservers);
                        _dispatcher.BeginInvoke(update3);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                };
                _worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                newID = -1;
                throw new Exception(ex.Message, ex);
            }
            return newID;
        }

        public void RemoveClient(int id)
        {
            if (ServerStaticMembers.ConnectedClients != null)
            {
                if (ServerStaticMembers.ConnectedClients.Where(x => x.Id == id) != null)
                {
                    _dispatcher.Invoke((Action)delegate
                    {
                        ServerStaticMembers.ConnectedClients.RemoveAt(id - 1);
                    });
                    this.NotifyObservers();
                }
            }
        }

        public void StartServer()
        {
            try
            {
                ServerStaticMembers.HttpChannel = new HttpServerChannel(ServerStaticMembers.ChannelName, ServerStaticMembers.Port);
                ChannelServices.RegisterChannel(ServerStaticMembers.HttpChannel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), ServerStaticMembers.ChannelName, WellKnownObjectMode.Singleton);
                ServerStaticMembers.ServerModel = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                  "http://" + ServerStaticMembers.Host + ":" + ServerStaticMembers.Port.ToString() + "/SingletonServer");
                _isListening = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void StopServer()
        {
            try
            {
                RemoveAllClients();
                ChannelServices.UnregisterChannel(ServerStaticMembers.HttpChannel);
                _isListening = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
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

        public ObservableCollection<ConnectedClient> Clients
        {
            get
            {
                return ServerStaticMembers.ConnectedClients;
            }
        }

        #endregion
    }
}
