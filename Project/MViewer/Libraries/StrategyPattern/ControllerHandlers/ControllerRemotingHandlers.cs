using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using System.Threading.Tasks;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public class ControllerRemotingHandlers
    {
        public IDictionary<GenericEnums.MouseCommandType, Delegates.HookCommandDelegate> MouseCommands;
        public IDictionary<GenericEnums.KeyboardCommandType, Delegates.HookCommandDelegate> KeyboardCommands;
    }
}
