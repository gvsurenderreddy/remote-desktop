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

        ControllerRoomHandlers _RoomHandlers;
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
            videoDelegates.Add(GenericEnums.SignalType.Start, Program.Controller.StartVideo);
            videoDelegates.Add(GenericEnums.SignalType.Stop, Program.Controller.StopVideo);
            videoDelegates.Add(GenericEnums.SignalType.Pause, Program.Controller.PauseVideo);
            videoDelegates.Add(GenericEnums.SignalType.Resume, Program.Controller.ResumeVideo);

            Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> transferDelegates = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            transferDelegates.Add(GenericEnums.SignalType.Start, Program.Controller.SendFileHandler);

            Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> remotingDelegates = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            remotingDelegates.Add(GenericEnums.SignalType.Start, Program.Controller.StartRemoting);
            remotingDelegates.Add(GenericEnums.SignalType.Stop, Program.Controller.StopRemoting);
            remotingDelegates.Add(GenericEnums.SignalType.Pause, Program.Controller.PauseRemoting);
            remotingDelegates.Add(GenericEnums.SignalType.Resume, Program.Controller.ResumeRemoting);

            _RoomHandlers = new ControllerRoomHandlers()
            {
                // todo: add audio & remoting handlers handlers by signal type
                Video = videoDelegates,
                Transfer = transferDelegates,
                Remoting = remotingDelegates
            };

            // add mouse & keyboard delegate from the controller
            RemotingCommand = Program.Controller.SendRemotingCommand;

            Dictionary<GenericEnums.KeyboardCommandType, Delegates.HookCommandDelegate> keyboardDelegates = new Dictionary<GenericEnums.KeyboardCommandType, Delegates.HookCommandDelegate>();
            keyboardDelegates.Add(GenericEnums.KeyboardCommandType.KeyDown, Program.Controller.KeyDown);
            keyboardDelegates.Add(GenericEnums.KeyboardCommandType.KeyPress, Program.Controller.KeyPress);
            keyboardDelegates.Add(GenericEnums.KeyboardCommandType.KeyUp, Program.Controller.KeyUp);

            IDictionary<GenericEnums.MouseCommandType, Delegates.HookCommandDelegate> mouseDelegates = new Dictionary<GenericEnums.MouseCommandType, Delegates.HookCommandDelegate>();
            mouseDelegates.Add(GenericEnums.MouseCommandType.LeftClick, Program.Controller.LeftClickCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.RightClick, Program.Controller.RightClickCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.DoubleRightClick, Program.Controller.DoubleRightClickCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.DoubleLeftClick, Program.Controller.DoubleLeftClickCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.MiddleClick, Program.Controller.MiddleClickCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.DoubleMiddleClick, Program.Controller.DoubleMiddleClickCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.LeftMouseDown, Program.Controller.LeftMouseDownCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.LeftMouseUp, Program.Controller.LeftMouseUpCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.RightMouseDown, Program.Controller.RightMouseDownCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.RightMouseUp, Program.Controller.RightMouseUpCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.MiddleMouseDown, Program.Controller.MiddleMouseDownCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.MiddleMouseUp, Program.Controller.MiddleMouseUpCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.Move, Program.Controller.MouseMoveCommand);
            mouseDelegates.Add(GenericEnums.MouseCommandType.Wheel, Program.Controller.MouseWheelCommand);

            _remotingCommandHandlers = new ControllerRemotingHandlers()
            {
                KeyboardCommands = keyboardDelegates,
                MouseCommands = mouseDelegates
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

        public ControllerRoomHandlers RoomHandlers
        {
            get { return _RoomHandlers; }
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
