using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
     public abstract class MouseCommandBase: IMouseCommands
    {
         protected Dictionary<GenericEnums.MouseCommandType, Delegates.MouseCommandDelegate> _commands;

         Delegates.MouseCommandDelegate _command;

        public MouseCommandBase()
        {
            _command = PerformCommand;
        }

        public void Execute(object sender, MouseActionEventArgs args)
        {
            _command.Invoke(sender, args);
        }

        public abstract void BindCommands();

        public void PerformCommand(object sender, MouseActionEventArgs args)
        {
            //_commands[args.SignalType].Invoke(sender, args);
        }
    }
}
