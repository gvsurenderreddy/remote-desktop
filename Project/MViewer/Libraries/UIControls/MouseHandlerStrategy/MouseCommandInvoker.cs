using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace UIControls
{
    public class MouseCommandInvoker : IMouseCommandInvoker
    {
        Dictionary<GenericEnums.MouseCommandType, IMouseCommands> commands;

        public MouseCommandInvoker(ControllerMouseHandlers mouseHandlers)
        {
            // todo: provide the mouse event handlers from the controller

            MouseCommands mouseCommands = new MouseCommands()
            {
                //StartVideoChat = mouseHandlers.Video[GenericEnums.SignalType.Start],
                //StopVideChat = mouseHandlers.Video[GenericEnums.SignalType.Stop],
                //PauseVideoChat = mouseHandlers.Video[GenericEnums.SignalType.Pause],
                //ResumeVideoChat = mouseHandlers.Video[GenericEnums.SignalType.Resume]
            };
            mouseCommands.BindCommands();
      
            commands = new Dictionary<GenericEnums.MouseCommandType, IMouseCommands>();
            commands.Add(GenericEnums.MouseCommandType.Undefined, mouseCommands);
        }

        public void PerformCommand(object sender, MouseActionEventArgs args)
        {
            //commands[args.RoomType].Execute(sender, args);
        }


    }
}
