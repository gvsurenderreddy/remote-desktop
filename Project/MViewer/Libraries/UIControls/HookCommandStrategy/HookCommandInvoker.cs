using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace UIControls
{
    public class HookCommandInvoker : IHookCommandInvoker
    {
        Dictionary<GenericEnums.RemotingCommandType, IHookCommands> commands;

        public HookCommandInvoker(ControllerHookCommandHandlers mouseHandlers)
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
      
            commands = new Dictionary<GenericEnums.RemotingCommandType, IHookCommands>();
            commands.Add(GenericEnums.RemotingCommandType.Mouse, mouseCommands);

            KeyboardHookCommands keyboardCommands = new KeyboardHookCommands()
            {

            };
            keyboardCommands.BindCommands();

            commands.Add(GenericEnums.RemotingCommandType.Keyboard, keyboardCommands);

        }

        public void PerformCommand(object sender, EventArgs args)
        {
            //commands[args.RoomType].Execute(sender, args);
        }


    }
}
