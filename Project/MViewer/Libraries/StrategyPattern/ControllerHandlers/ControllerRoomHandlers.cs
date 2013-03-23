using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace StrategyPattern
{
    public class ControllerRoomHandlers
    {
        public IDictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Audio { get; set; }
        public IDictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Video { get; set; }
        public IDictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Remoting { get; set; }
        public IDictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Transfer { get; set; }
    }
}
