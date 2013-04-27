using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public class TransferCommands : RoomCommandBase
    {
        public Delegates.RoomCommandDelegate SendFile;
        public Delegates.RoomCommandDelegate TransferPermission;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            _commands.Add(GenericEnums.SignalType.Start, SendFile);
        }
    }
}
