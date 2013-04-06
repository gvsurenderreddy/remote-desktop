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

        public void Execute(object sender, RemotingCommandEventArgs args)
        {
            _command.Invoke(sender, args);
        }

        public abstract void BindCommands();

        public void PerformCommand(object sender, RemotingCommandEventArgs args)
        {
            _commands[args.MouseCommandType].Invoke(sender, args);
        }
    }
}
