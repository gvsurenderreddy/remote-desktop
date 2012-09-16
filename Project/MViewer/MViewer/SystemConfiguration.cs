using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MViewer
{
    public class SystemConfiguration
    {
        public readonly string MyIP = ConfigurationManager.AppSettings["MyIP"];
        public readonly int Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        public readonly string DataBasePath = ConfigurationManager.AppSettings["dataBasePath"];
        public readonly string FriendlyName = ConfigurationManager.AppSettings["FriendlyName"];
    }
}
