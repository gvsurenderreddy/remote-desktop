using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public abstract class RoomCommandBase : IRoomCommands 
    {
        protected Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate> _commands;

        Delegates.RoomCommandDelegate _command;

        public RoomCommandBase()
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
            RoomActionEventArgs e = (RoomActionEventArgs)args;
            _commands[e.SignalType].Invoke(sender, args);
        }
    }
}
