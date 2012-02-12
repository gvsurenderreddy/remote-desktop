using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Timers;
using DesktopSharingViewer;

namespace WpfRemotingClient
{
    [Serializable]
    public class RemotingClient : ClientModel
    {
        public RemotingClient(string localIP, string serverHost, DesktopViewer.DesktopChangedEventHandler onDesktopChanged) 
            : base(localIP, serverHost, onDesktopChanged) { }
    }
}
