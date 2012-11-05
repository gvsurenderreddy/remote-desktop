using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MViewer
{
    public static class SystemConfiguration
    {
        public static readonly string MyAddress = ConfigurationManager.AppSettings["MyAddress"];
        public static readonly int Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        public static readonly string ServicePath = ConfigurationManager.AppSettings["ServicePath"];
        public static readonly string DataBasePath = ConfigurationManager.AppSettings["dataBasePath"];
        public static readonly string FriendlyName = ConfigurationManager.AppSettings["FriendlyName"];
        public static string MyIdentity;
    }
}
