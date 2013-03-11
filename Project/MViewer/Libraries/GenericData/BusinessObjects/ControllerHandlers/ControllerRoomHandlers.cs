using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public class ControllerRoomHandlers
    {
        public Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> Audio { get; set; }
        public Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> Video { get; set; }
        public Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> Remoting { get; set; }
        public Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> Transfer { get; set; }
    }
}
