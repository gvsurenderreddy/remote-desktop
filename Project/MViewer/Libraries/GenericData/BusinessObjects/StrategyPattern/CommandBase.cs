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

        protected abstract void PerformCommand(object sender, RoomActionEventArgs args);
        //{
        //    return 0;
        //}
    }
}
