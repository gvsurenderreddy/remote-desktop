using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace StrategyPattern
{
    public class TransferCommands : RoomCommandBase
    {
        Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> _transferCommands;

        public Delegates.RoomCommandDelegate SendFile;
        public Delegates.RoomCommandDelegate TransferPermission;

        public override void BindCommands()
        {
            _transferCommands = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            _transferCommands.Add(GenericEnums.SignalType.Start, SendFile);
        }
    }
}
