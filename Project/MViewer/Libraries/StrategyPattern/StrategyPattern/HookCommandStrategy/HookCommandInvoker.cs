using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace StrategyPattern
{
    // todo: remove HookCommandInvoker if not used

    public class HookCommandInvoker : IHookCommandInvoker
    {
        Dictionary<GenericEnums.RemotingCommandType, IHookCommands> commands;

        public HookCommandInvoker(ControllerRemotingHandlers remotingCommandHandlers)
        {
            // todo: provide the mouse/keyboard event handlers from the controller

            MouseHookCommand mouseCommands = new MouseHookCommand()
            {
                //StartVideoChat = remotingCommandHandlers.Video[GenericEnums.SignalType.Start],
                //StopVideChat = remotingCommandHandlers.Video[GenericEnums.SignalType.Stop],
                //PauseVideoChat = remotingCommandHandlers.Video[GenericEnums.SignalType.Pause],
                //ResumeVideoChat = remotingCommandHandlers.Video[GenericEnums.SignalType.Resume]
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
