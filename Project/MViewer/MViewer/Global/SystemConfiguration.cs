using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using GenericDataLayer;
using Utils;
using StrategyPattern;

namespace MViewer
{
    public class SystemConfiguration
    {
        #region private members

        static readonly object _syncInstance = new object();
        static SystemConfiguration _instance;

        private PresenterSettings _presenterSettings;

        ControllerRoomHandlers _chatRoomHandlers;
        ControllerRemotingHandlers _remotingCommandHandlers;

        Delegates.HookCommandDelegate _remotingCommandInvoker;

        #endregion

        #region c-tor

        private SystemConfiguration() 
        {
            int timerInterval = 100;
            int height = 354, width = 360;

            _presenterSettings = new PresenterSettings()
            {
                Identity = FriendlyName,
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

            // handlers initialization 
            Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> videoDelegates = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            videoDelegates.Add(GenericEnums.SignalType.Start, Program.Controller.StartVideoChat);
            videoDelegates.Add(GenericEnums.SignalType.Stop, Program.Controller.StopVideChat);
            videoDelegates.Add(GenericEnums.SignalType.Pause, Program.Controller.PauseVideoChat);
            videoDelegates.Add(GenericEnums.SignalType.Resume, Program.Controller.ResumeVideoChat);

            Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> transferDelegates = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            transferDelegates.Add(GenericEnums.SignalType.Start, Program.Controller.SendFileHandler);

            Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> remotingDelegates = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            remotingDelegates.Add(GenericEnums.SignalType.Start, Program.Controller.StartRemotingChat);
            remotingDelegates.Add(GenericEnums.SignalType.Stop, Program.Controller.StopRemotingChat);
            remotingDelegates.Add(GenericEnums.SignalType.Pause, Program.Controller.PauseRemotingChat);
            remotingDelegates.Add(GenericEnums.SignalType.Resume, Program.Controller.ResumeRemotingChat);

            _chatRoomHandlers = new ControllerRoomHandlers()
            {
                // todo: add audio & remoting handlers handlers by signal type
                Video = videoDelegates,
                Transfer = transferDelegates,
                Remoting = remotingDelegates
            };

            // add mouse & keyboard delegate from the controller
            RemotingCommand = Program.Controller.SendRemotingCommand;

            _remotingCommandHandlers = new ControllerRemotingHandlers()
            {
                //KeyboardCommands = new Dictionary<
            };
        }

        #endregion

        #region properties

        public readonly string MyAddress = ConfigurationManager.AppSettings["MyAddress"];
        public readonly int Port = int.Parse(ConfigurationManager.AppSettings["port"]);
        public readonly string ServicePath = ConfigurationManager.AppSettings["ServicePath"];
        public readonly string DataBasePath = ConfigurationManager.AppSettings["dataBasePath"];
        public readonly string FriendlyName = ConfigurationManager.AppSettings["FriendlyName"];
        public readonly int TimerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);

        public Delegates.HookCommandDelegate RemotingCommand
        {
            get { return _remotingCommandInvoker; }
            set { _remotingCommandInvoker = value; }
        }

        public ControllerRoomHandlers ChatRoomHandlers
        {
            get { return _chatRoomHandlers; }
        }

        public PresenterSettings PresenterSettings
        {
            get { return _presenterSettings; }
        }

        public ControllerRemotingHandlers RemotingCommandHandlers
        {
            get { return _remotingCommandHandlers; }
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

        #endregion
    }
}
