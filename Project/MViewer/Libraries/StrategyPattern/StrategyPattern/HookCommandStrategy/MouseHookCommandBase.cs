using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public abstract class MouseHookCommandBase : IHookCommands
    {
        protected Dictionary<GenericEnums.MouseCommandType, Delegates.HookCommandDelegate> _commands;

        Delegates.HookCommandDelegate _command;

        public MouseHookCommandBase()
        {
            _command = PerformCommand;
        }

        public void Execute(object sender, EventArgs args)
        {
            _command.Invoke(sender, args);
        }

        public abstract void BindCommands();

        public void PerformCommand(object sender, EventArgs args)
        {
            RemotingCommandEventArgs e = (RemotingCommandEventArgs)args;
            _commands[e.MouseCommandType].Invoke(sender, args);
        }
    }
}
