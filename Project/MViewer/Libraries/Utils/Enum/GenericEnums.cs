using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class GenericEnums
    {
        public enum FormMode 
        { 
            Undefined = 0, 
            Add = 1, 
            Update = 2, 
            Find = 3
        };

        public enum ContactStatus 
        { 
            Undefined = 0, 
            Offline = 1,
            Online = 2, 
            Connected = 3 
        };

        public enum SignalType 
        { 
            Undefined = 0, 
            Start = 1, 
            Stop = 2, 
            Pause = 3, 
            Resume = 4, 
            Wait = 5, 
            RemoveWait = 6
        };

        public enum RoomType 
        { 
            Undefined = 0, 
            Audio = 1, 
            Video = 2, 
            Remoting = 3, 
            Send = 4 
        };

        public enum ContactsOperation 
        { 
            Undefined = 0, 
            Add = 1, 
            Update = 2, 
            Remove = 3, 
            Get = 4, 
            Load = 5, 
            Status = 6 
        };

        public enum SessionState 
        { 
            Undefined = 0, 
            Opened = 1, 
            Closed = 2, 
            Paused = 3, 
            Pending = 4 
        };

        public enum MouseCommandType 
        { 
            Undefined = 0, 
            LeftClick = 1, 
            RightClick = 2, 
            DoubleLeftClick = 3, 
            Scroll = 4 
        }; // todo: complete MouseCommandType

        public enum KeyBoardCommandType
        {
            Undefined = 0,
            // todo: complete KeyBoardCommandType
        }

        public enum RemotingCommandType 
        { 
            Undefined = 0, 
            Mouse = 1, 
            Keyboard = 2 
        };

   
    }
}
