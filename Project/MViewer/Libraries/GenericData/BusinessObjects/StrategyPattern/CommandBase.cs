using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public abstract class CommandBase : ICommand 
    {
        protected Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> _commands;

        Delegates.CommandDelegate _command;

        public CommandBase()
        {
            _command = PerformCommand;
        }

        public void Execute(object sender, RoomActionEventArgs args)
        {
            _command.Invoke(sender, args);
        }

        public abstract void BindCommands();

        public void PerformCommand(object sender, RoomActionEventArgs args)
        {
            _commands[args.SignalType].Invoke(sender, args);
        }
    }
}
