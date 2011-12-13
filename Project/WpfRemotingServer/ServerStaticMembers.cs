using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Remoting.Channels.Http;
using Common;
using System.Configuration;
using System.Collections.ObjectModel;

namespace WpfRemotingServer
{
    public static class ServerStaticMembers
    {
        public static ObservableCollection<ConnectedClient> ConnectedClients = new ObservableCollection<ConnectedClient>();
        public static ArrayList Observers;
        public static HttpServerChannel HttpChannel;
        public static string ChannelName = ConfigurationManager.AppSettings["channelName"];
        public static int Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        public static string Host = ConfigurationManager.AppSettings["host"];
        public static log4net.ILog Logger;

        public static IServerControl ServerControl;
        public static IServerModel ServerModel;
        public static IServerView ServerView;
    }
}
