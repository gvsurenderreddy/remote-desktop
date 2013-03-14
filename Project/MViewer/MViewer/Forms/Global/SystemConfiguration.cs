using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using GenericDataLayer;
using Utils;

namespace MViewer
{
    public class SystemConfiguration
    {
        static readonly object _syncInstance = new object();
        static SystemConfiguration _instance;

        private SystemConfiguration() 
        {
            int timerInterval = 100;
            int height = 354, width = 360;

            // todo: move the presenter settings to the system configuration class
            _presenterSettings = new PresenterSettings()
            {
                Identity = Program.Controller.MyIdentity(),
                VideoTimerInterval = timerInterval,
                VideoScreenSize =
                    new Structures.ScreenSize()
                    {
                        Height = height,
                        Width = width
                    },
                VideoImageCaptured = new EventHandler(Program.Controller.VideoImageCaptured),
                RemotingTimerInterval = 50,
                RemotingImageCaptured = new EventHandler(Program.Controller.RemotingImageCaptured)
            };
        }

        public readonly string MyAddress = ConfigurationManager.AppSettings["MyAddress"];
        public readonly int Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        public readonly string ServicePath = ConfigurationManager.AppSettings["ServicePath"];
        public readonly string DataBasePath = ConfigurationManager.AppSettings["dataBasePath"];
        public readonly string FriendlyName = ConfigurationManager.AppSettings["FriendlyName"];
        public readonly int TimerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
        
        private PresenterSettings _presenterSettings;

        public PresenterSettings PresenterSettings
        {
            get { return _presenterSettings; }
        }

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
