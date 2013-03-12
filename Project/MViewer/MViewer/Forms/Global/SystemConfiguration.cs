using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MViewer
{
    public class SystemConfiguration
    {
        static readonly object _syncInstance = new object();
        static SystemConfiguration _instance;

        private SystemConfiguration() { }

        public readonly string MyAddress = ConfigurationManager.AppSettings["MyAddress"];
        public readonly int Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        public readonly string ServicePath = ConfigurationManager.AppSettings["ServicePath"];
        public readonly string DataBasePath = ConfigurationManager.AppSettings["dataBasePath"];
        public readonly string FriendlyName = ConfigurationManager.AppSettings["FriendlyName"];
        public readonly int TimerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);

        public string MyIdentity;

        public static SystemConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = new SystemConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
