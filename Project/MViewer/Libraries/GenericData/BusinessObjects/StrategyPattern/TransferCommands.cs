using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public class TransferCommands : CommandBase
    {
        Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> _transferCommands;

        public Delegates.CommandDelegate SendFile;
        public Delegates.CommandDelegate TransferPermission;

        public override void BindCommands()
        {
            _transferCommands = new Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate>();
            _transferCommands.Add(GenericEnums.SignalType.Start, SendFile);
        }
    }
}
