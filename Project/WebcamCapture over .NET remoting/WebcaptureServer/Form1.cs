using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using Webcam_Test;
using WpfRemotingServer;
using Common;
using System.IO;

namespace WebcaptureServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // todo: start the remoting server
            HttpServerChannel HttpChannel = new HttpServerChannel(ServerStaticMembers.ChannelName, ServerStaticMembers.Port);
            //RemotingConfiguration.Configure(httpChannel, false);
            ChannelServices.RegisterChannel(HttpChannel, false);
            
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SingletonServer), ServerStaticMembers.ChannelName, WellKnownObjectMode.Singleton);
            IServerModel ServerModel = (SingletonServer)Activator.GetObject(typeof(SingletonServer),
               "http://" + ServerStaticMembers.Host + ":" + ServerStaticMembers.Port.ToString() + "/SingletonServer");
        }

        public void WebCamCapture_ImageCaptured(byte[] bytes)
        {
            if (File.Exists("c:\\receive.bmp"))
            {
                File.Delete("c:\\receive.bmp");
            }
            Image returnImage = ImageConverter.byteArrayToImage(bytes);
            returnImage.Save("c:\\receive.bmp");
            // set the picturebox picture
            this.pictureBox1.Image = returnImage;

        }
    }
}
