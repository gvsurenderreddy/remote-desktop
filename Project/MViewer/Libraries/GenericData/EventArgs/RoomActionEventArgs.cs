using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public class RoomActionEventArgs : EventArgs
    {
        public GenericEnums.RoomActionType ActionType
        {
            get;
            set;
        }

        public GenericEnums.SignalType SignalType
        {
            get;
            set;
        }

        public string FriendlyName
        {
            get;
            set;
        }

        public string Identity
        {
            get;
            set;
        }
    }
}
