using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Configuration;
using Common;
using System.Collections;
using System.Windows.Threading;
using Microsoft.JScript;
using System.Threading;
using System.Collections.ObjectModel;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window, IServerView
    {
        #region members

        Action EmptyDelegate = delegate() { };

        #endregion

        #region c-tor

        public ServerMainWindow()
        {
            try
            {
                InitializeComponent();
                lblStatus.Content = "Status: started";
                btnStartServer.Content = "Stop listening";
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region methods

        public void WireUp(IServerControl serverControl, IServerModel serverModel)
        {
            if (ServerStaticMembers.ServerModel != null)
            {
                ServerStaticMembers.ServerModel.RemoveObserver(this);
            }
            ServerStaticMembers.ServerModel = serverModel;
            ServerStaticMembers.ServerControl = serverControl;
            ServerStaticMembers.ServerControl.SetModel(ServerStaticMembers.ServerModel);
            ServerStaticMembers.ServerControl.SetView(this);
            ServerStaticMembers.ServerModel.AddObserver(this);
        }

        public void UpdateInterface(IServerModel serverModel)
        {
            if (serverModel != null)
            {
                if (serverModel.IsListening)
                {
                    Utils.UpdateControlContent(Dispatcher, lblStatus, "Status: started", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, btnStartServer, "Stop listening", Utils.ValueType.String);
                }
                else
                {
                    Utils.UpdateControlContent(Dispatcher, lblStatus, "Status: stopped", Utils.ValueType.String);
                    Utils.UpdateControlContent(Dispatcher, btnStartServer, "Start listening", Utils.ValueType.String);
                }
                DisplayClients(serverModel.Clients);
                Utils.UpdateControlContent(Dispatcher, lblTotal, "Total: " + serverModel.ConnectedClients.ToString(), Utils.ValueType.String);
            }
        }

        public void DisplayClients(ObservableCollection<ConnectedClient> clients)
        {
            if (clients != null)
            {
                lvClients.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    lvClients.ItemsSource = clients;
                }));
            }
        }

        #endregion

        #region IServerView methods

        public int AddClient(string ip, string hostname)
        {
            return ServerStaticMembers.ServerControl.AddClient(ip, hostname);
        }

        public void RemoveClient(int id, bool checkStatus)
        {
            ServerStaticMembers.ServerControl.RemoveClient(id, checkStatus);
        }

        public void Update(IServerModel serverModel)
        {
            this.UpdateInterface(serverModel);
        }

        #endregion

        #region callbacks

        System.Windows.Controls.Image ConvertDrawingImageToWPFImage(System.Drawing.Image gdiImg, ref System.Windows.Controls.Image img)
        {
            //System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            //convert System.Drawing.Image to WPF image
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = wpfBitmap;
            img.Width = 500;
            img.Height = 600;
            img.Stretch = System.Windows.Media.Stretch.Fill;
            return img;
        }

        public void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //DesktopSharing.ScreenCapture _capture = new DesktopSharing.ScreenCapture();
                //System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
                //System.Drawing.Bitmap screenCapture = _capture.CaptureScreen(ref rect);

                //byte[] serialized = DesktopSharing.RemoteServiceUtils.SerializeCapture(screenCapture, rect);

                //System.Drawing.Image partialDesktop;
                //System.Drawing.Rectangle rect2;
                //Guid id;
                //DesktopSharingViewer.DesktopViewerUtils.Deserialize(serialized, out partialDesktop, out rect2, out id);

                //ConvertDrawingImageToWPFImage(partialDesktop, ref imgDesktop);




                //if (ServerStaticMembers.ServerModel.IsListening)
                //{
                //    ServerStaticMembers.ServerControl.RequestStopServer();
                //}
                //else
                //{
                //    ServerStaticMembers.ServerControl.RequestStartServer();
                //}
            }
            catch(Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        private void btnCloseConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (ConnectedClient client in lvClients.SelectedItems)
                {
                    if (client.Connected == true)
                    {
                        client.Connected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
