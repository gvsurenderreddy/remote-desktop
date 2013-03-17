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
        public Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Audio { get; set; }
        public Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Video { get; set; }
        public Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Remoting { get; set; }
        public Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> Transfer { get; set; }
    }
}
