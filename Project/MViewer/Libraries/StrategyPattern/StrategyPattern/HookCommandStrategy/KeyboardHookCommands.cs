using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public class KeyboardHookCommand : KeyboardHookCommandBase
    {
        public Delegates.HookCommandDelegate KeyDownCommand;
        public Delegates.HookCommandDelegate KeyPressCommand;
        public Delegates.HookCommandDelegate KeyUpCommand;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.KeyboardCommandType, Delegates.HookCommandDelegate>();
            _commands.Add(GenericEnums.KeyboardCommandType.KeyDown, KeyDownCommand);
            _commands.Add(GenericEnums.KeyboardCommandType.KeyPress, KeyPressCommand);
            _commands.Add(GenericEnums.KeyboardCommandType.KeyUp, KeyUpCommand);
        }
    }
}
