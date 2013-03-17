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
            // todo: provide the mouse/keyboard event handlers from the controller

            MouseHookCommand mouseCommands = new MouseHookCommand()
            {
                //StartVideoChat = mouseHandlers.Video[GenericEnums.SignalType.Start],
                //StopVideChat = mouseHandlers.Video[GenericEnums.SignalType.Stop],
                //PauseVideoChat = mouseHandlers.Video[GenericEnums.SignalType.Pause],
                //ResumeVideoChat = mouseHandlers.Video[GenericEnums.SignalType.Resume]
            };
            mouseCommands.BindCommands();
      
            commands = new Dictionary<GenericEnums.RemotingCommandType, IHookCommands>();
            commands.Add(GenericEnums.RemotingCommandType.Mouse, mouseCommands);

            KeyboardHookCommand keyboardCommands = new KeyboardHookCommand()
            {

            };
            keyboardCommands.BindCommands();

            commands.Add(GenericEnums.RemotingCommandType.Keyboard, keyboardCommands);

        }

        public void PerformCommand(object sender, RemotingCommandEventArgs args)
        {

            commands[args.RemotingCommandType].Execute(sender, args);
        }


    }
}
