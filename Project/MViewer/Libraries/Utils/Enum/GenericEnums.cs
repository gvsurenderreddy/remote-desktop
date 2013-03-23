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

        public enum ServiceEndpointPart
        {
            Undefined = 0,
            Certificate = 1, // client parts
            Binding = 2,
            Contract = 3,
            Uri = 4, // server specific parts
            Behavior = 5
        }

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
            MiddleClick = 3,
            DoubleLeftClick = 4,
            DoubleRightClick = 5,
            DoubleMiddleClick = 6,
            Wheel = 7,
            Move = 8,
            LeftMouseUp = 9,
            LeftMouseDown = 10,
            RightMouseUp = 11, // todo: use the right mouse up/down
            RightMouseDown = 12, 
            MiddleMouseUp = 13,
            MiddleMouseDown = 14
        }; // todo: complete MouseCommandType

        public enum KeyboardCommandType
        {
            Undefined = 0,
            KeyUp = 1,
            KeyDown = 2,
            KeyPress = 3
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
