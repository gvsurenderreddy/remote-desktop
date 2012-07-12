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
using DesktopSharing;
using Webcam_Test;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using WebcaptureServer;

namespace WpfRemotingServer
{
    public class SingletonServer : MarshalByRefObject, IServerModel
    {
        #region members

        bool _isListening = false;
        BackgroundWorker _worker;
        //Dispatcher _dispatcher;
        //static ServerMainWindow _smw;
        delegate void NotifyObserversDelegate();
        readonly object _syncConnectedClients = new object();
        //IRemoteService _remoteService;

        #endregion

        #region c-tor

        public SingletonServer()
        {
            try
            {
                //if (ServerStaticMembers.Observers == null)
                //{
                //    ServerStaticMembers.Observers = new ArrayList();
                //}
                //if (ServerStaticMembers.ConnectedClients == null)
                //{
                //    ServerStaticMembers.ConnectedClients = new ObservableCollection<ConnectedClient>();
                //}
                //_remoteService = new RemoteService();
                Thread t = new Thread(ThreadProc);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                Thread.Sleep(5000);
                //_dispatcher = _smw.Dispatcher;
                _worker = new BackgroundWorker();
                _worker.WorkerSupportsCancellation = true;
                ServerStaticMembers.ServerModel = this;
                //ServerStaticMembers.ServerView = _smw;
                ServerStaticMembers.ServerControl = new ServerControl(ServerStaticMembers.ServerModel, ServerStaticMembers.ServerView);
                ServerStaticMembers.ServerView.WireUp(ServerStaticMembers.ServerControl, ServerStaticMembers.ServerModel);
                //ServerStaticMembers.Logger.Info("Remoting Server Initialized");
                _isListening = true;


                // todo: use some encryption algorithm to encrypt the server address and decrypt in the client logic
            }
            catch //(Exception ex)
            {
                _isListening = false;
                //ServerStaticMembers.Logger.Error("Remoting Server Initialization failed - " + ex.Message, ex);
            }
        }  

        #endregion

        #region methods

        public void AddObserver(IServerView serverView)
        {
            //if (ServerStaticMembers.Observers != null)
            //{
            //    if (ServerStaticMembers.Observers.Contains(serverView) == false)
            //    {
            //        ServerStaticMembers.Observers.Add(serverView);
            //    }
            //}
        }

        public void RemoveObserver(IServerView serverView)
        {
            //if (ServerStaticMembers.Observers != null)
            //{
            //    if (ServerStaticMembers.Observers.Contains(serverView))
            //    {
            //        ServerStaticMembers.Observers.Remove(serverView);
            //    }
            //}
        }

        public void RemoveAllClients()
        {
            lock (_syncConnectedClients)
            {
                //ServerStaticMembers.ConnectedClients.Clear();
            }
        }

        public void NotifyObservers()
        {
            //if (ServerStaticMembers.Observers != null)
            //{
            //    foreach (IServerView view in ServerStaticMembers.Observers)
            //    {
            //        view.Update(this);
            //    }
            //}
        }

        void ThreadProc()
        {
            try
            {
                //_smw = new ServerMainWindow();
                //_smw.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //[STAThread]
        //public int AddClient(string ip, string hostname)
        //{
        //    lock (_syncConnectedClients)
        //    {
        //        //int newID = ServerStaticMembers.ConnectedClients.Count + 1;
        //        //try
        //        //{
        //        //    _dispatcher.Invoke((Action)delegate { ServerStaticMembers.ConnectedClients.Add(new ConnectedClient(ip, hostname, newID)); });
        //        //    _worker.DoWork += delegate(object s, DoWorkEventArgs args)
        //        //    {
        //        //        try
        //        //        {
        //        //            if (_worker.CancellationPending)
        //        //            {
        //        //                args.Cancel = true;
        //        //                return;
        //        //            }
        //        //            System.Threading.Thread.Sleep(10);
        //        //            NotifyObserversDelegate update3 = new NotifyObserversDelegate(NotifyObservers);
        //        //            _dispatcher.BeginInvoke(update3);
        //        //        }
        //        //        catch (Exception ex)
        //        //        {
        //        //            throw new Exception(ex.Message, ex);
        //        //        }
        //        //    };
        //        //    _worker.RunWorkerAsync();
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    newID = -1;
        //        //    throw new Exception(ex.Message, ex);
        //        //}
        //        //return newID;
        //    }
        //}

        public void RemoveClient(int id, bool checkStatus)
        {
            //lock (_syncConnectedClients)
            //{
            //    if (ServerStaticMembers.ConnectedClients != null)
            //    {
            //        if (checkStatus)
            //        {
            //            if (ServerStaticMembers.ConnectedClients.Where(x => x.Id == id && x.Connected == false).Count() > 0)
            //            {
            //                _dispatcher.Invoke((Action)delegate
            //                {
            //                    ServerStaticMembers.ConnectedClients.RemoveAt(id - 1);
            //                });
            //                this.NotifyObservers();
            //            }
            //        }
            //        else
            //        {
            //            if (ServerStaticMembers.ConnectedClients.Where(x => x.Id == id).Count() > 0)
            //            {
            //                _dispatcher.Invoke((Action)delegate
            //                {
            //                    ServerStaticMembers.ConnectedClients.RemoveAt(id - 1);
            //                });
            //                this.NotifyObservers();
            //            }
            //        }
            //    }
            //}
        }

        public void StartServer()
        {
            try
            {
                //ServerStaticMembers.HttpChannel = new HttpServerChannel(ServerStaticMembers.ChannelName, ServerStaticMembers.Port);
                //ChannelServices.RegisterChannel(ServerStaticMembers.HttpChannel, false);
                //RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), ServerStaticMembers.ChannelName, WellKnownObjectMode.Singleton);
                //ServerStaticMembers.ServerModel = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
                //  "http://" + ServerStaticMembers.Host + ":" + ServerStaticMembers.Port.ToString() + "/SingletonServer");
                //_isListening = true;
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

        //public byte[] UpdateDesktop()
        //{
        //    return _remoteService.CaptureDekstopImage();
        //}

        //public byte[] UpdateMouseCursor()
        //{
        //    return _remoteService.CaptureMouseImage();
        //}

        //public bool CheckClientStatus(int id)
        //{
        //    lock (_syncConnectedClients)
        //    {
        //        bool retVal = false;
        //        int count = ServerStaticMembers.ConnectedClients.Where(x => x.Id == id).Count();
        //        if (count > 0)
        //        {
        //            retVal = ServerStaticMembers.ConnectedClients[id - 1].Connected;
        //        }
        //        return retVal;
        //    }
        //}

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

        //public int ConnectedClients
        //{
        //    get { return ServerStaticMembers.ConnectedClients.Count; }
        //}

        //public ObservableCollection<ConnectedClient> Clients
        //{
        //    get
        //    {
        //        return ServerStaticMembers.ConnectedClients;
        //    }
        //}

        #endregion

        #region IServerModel Members

        private WebCam_Capture.WebCamCapture WebCamCapture;

        public void StartCapturing()
        {
            this.WebCamCapture = new WebCam_Capture.WebCamCapture();
            this.WebCamCapture.CaptureHeight = 240;
            this.WebCamCapture.CaptureWidth = 320;

            //// set the image capture size
            //this.WebCamCapture.CaptureHeight = this.pictureBox1.Height;
            //this.WebCamCapture.CaptureWidth = this.pictureBox1.Width;

            // TODO: Code generation for 'this.WebCamCapture.FrameNumber' failed because of Exception 'Invalid Primitive Type: System.UInt64. Only CLS compliant primitive types can be used. Consider using CodeObjectCreateExpression.'.
            this.WebCamCapture.Location = new System.Drawing.Point(17, 17);
            this.WebCamCapture.Name = "WebCamCapture";
            this.WebCamCapture.Size = new System.Drawing.Size(342, 252);
            this.WebCamCapture.TabIndex = 0;
            this.WebCamCapture.TimeToCapture_milliseconds = 100;
            this.WebCamCapture.ImageCaptured += new WebCam_Capture.WebCamCapture.WebCamEventHandler(this.WebCamCapture_ImageCaptured);

            // change the capture time frame
            this.WebCamCapture.TimeToCapture_milliseconds = 20;

            // start the video capture. let the control handle the
            // frame numbers.
            this.WebCamCapture.Start(0);
        }

        /// <summary>
        /// An image was capture
        /// </summary>
        /// <param name="source">control raising the event</param>
        /// <param name="e">WebCamEventArgs</param>
        private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
        {
            // set the picturebox picture
            //this.pictureBox1.Image = e.WebCamImage;

        }

        public void StopCapturing()
        {
            this.WebCamCapture.Stop();
        }

        public byte[] SendWebcamCapture()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServerModel Members


        public void SendWebcamCapture(byte[] capture)
        {
            Program.FRM.WebCamCapture_ImageCaptured(capture); 
        }

        #endregion

        #region IServerModel Members


        public int ConnectedClients
        {
            get { throw new NotImplementedException(); }
        }

        public ObservableCollection<ConnectedClient> Clients
        {
            get { throw new NotImplementedException(); }
        }

        public int AddClient(string ip, string hostname)
        {
            throw new NotImplementedException();
        }

        public byte[] UpdateDesktop()
        {
            throw new NotImplementedException();
        }

        public byte[] UpdateMouseCursor()
        {
            throw new NotImplementedException();
        }

        public bool CheckClientStatus(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
