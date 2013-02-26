using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class GenericEnums
    {
        public enum FormMode { Undefined = 0, Add = 1, Update = 2, Find = 3};
        public enum ContactStatus { Undefined = 0, Offline = 1, Online = 2, Connected = 3 };
        public enum SignalType { Undefined = 0, Start = 1, Stop = 2, Pause = 3, Resume };
        public enum RoomActionType { Undefined = 0, Audio = 1, Video = 2, Remoting = 3, Send = 4 };
        public enum ContactsOperation { Undefined = 0, Add = 1, Update = 2, Remove = 3, Get = 4, Load = 5, Status = 6 };
        // todo: remove the SessionType if not needed - we'll use only Client Sessions
        public enum SessionType { Undefined = 0, ClientSession = 1, ServerSession };
        public enum SessionState { Undefined = 0, Opened = 1, Closed = 2, Paused = 3 };
   
    }
}
