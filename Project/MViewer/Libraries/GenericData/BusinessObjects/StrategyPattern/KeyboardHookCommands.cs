using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public class KeyboardHookCommand : HookCommandBase
    {
        Delegates.HookCommandDelegate DoCommand;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.RemotingCommandType, Delegates.HookCommandDelegate>();
            _commands.Add(GenericEnums.RemotingCommandType.Keyboard, DoCommand);
        }
    }
}
