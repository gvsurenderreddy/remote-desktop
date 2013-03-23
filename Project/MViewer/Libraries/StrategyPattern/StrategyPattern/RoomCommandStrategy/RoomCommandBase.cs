using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GenericDataLayer;
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
