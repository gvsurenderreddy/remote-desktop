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

        void ThreadProc()
        {
            _smw = new ServerMainWindow();
            _smw.ShowDialog();
        }

        [STAThread]
        public int AddClient(string ip, string hostname)
        {
            int newID = ServerStaticMembers.ConnectedClients.Count + 1;
            ServerStaticMembers.ConnectedClients.Add(new ConnectedClient(ip, hostname, newID));
            _worker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                if (_worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                System.Threading.Thread.Sleep(10);

                //UpdatePBvalueDelegate update = new UpdatePBvalueDelegate(UpdateProgressText);
                //_dispatcher.BeginInvoke(update, 5);

                //UpdateClientsDelegate update2 = new UpdateClientsDelegate(SetText);
                //_dispatcher.BeginInvoke(update2, ServerStaticMembers.ConnectedClients.Count.ToString());

                NotifyObserversDelegate update3 = new NotifyObserversDelegate(NotifyObservers);
                _dispatcher.BeginInvoke(update3);
           
            };
            _worker.RunWorkerAsync();
            //App.smw.ShowDialog();
            return newID;
        }

        public delegate void NotifyObserversDelegate();

        public delegate void UpdatePBvalueDelegate(int newVal);

        public delegate void UpdateClientsDelegate(string newText);

        public void RemoveClient(int id)
        {
            // todo: test remove client
            if (ServerStaticMembers.ConnectedClients.Where(x => x.Id == id) != null)
            {
                ServerStaticMembers.ConnectedClients = (ObservableCollection<ConnectedClient>)ServerStaticMembers.ConnectedClients.Where(x => x.Id != id);
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
            //ServerStaticMembers.ServerModel = null;
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

        public ObservableCollection<ConnectedClient> Clients
        {
            get
            {
                //var connectedClients = from client in ServerStaticMembers.ConnectedClients
                //           where client.Value.Connected == true
                //           select client.Value;
                return ServerStaticMembers.ConnectedClients;
            }
        }

        #endregion
    }
}
