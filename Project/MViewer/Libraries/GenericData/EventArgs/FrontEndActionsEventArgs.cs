using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public class FrontEndActionsEventArgs : EventArgs
    {
        public GenericEnums.FrontEndActionType ActionType
        {
            get;
            set;
        }

        public GenericEnums.SignalType SignalType
        {
            get;
            set;
        }
    }
}
