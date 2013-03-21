using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace StrategyPattern
{
    public abstract class HookCommandBase : IHookCommands
    {
        protected Dictionary<GenericEnums.RemotingCommandType, Delegates.HookCommandDelegate> _commands;

        Delegates.HookCommandDelegate _command;

        public HookCommandBase()
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
            _commands[args.RemotingCommandType].Invoke(sender, args);
        }
    }
}
