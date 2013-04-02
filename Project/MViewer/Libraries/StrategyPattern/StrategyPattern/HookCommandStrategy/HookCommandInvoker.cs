using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public class HookCommandInvoker : IHookCommandInvoker
    {
        Dictionary<GenericEnums.RemotingCommandType, IHookCommands> commands;

        public HookCommandInvoker(ControllerRemotingHandlers remotingCommandHandlers)
        {
            // provide the mouse/keyboard event handlers from the controller

            MouseHookCommand mouseCommands = new MouseHookCommand()
            {
                LeftClickCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.LeftClick],
                RightClickCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.RightClick],
                DoubleLeftClickCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.DoubleLeftClick],
                DoubleRightClickCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.DoubleRightClick],
                MiddleClickCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.MiddleClick],
                DoubleMiddleClickCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.DoubleMiddleClick],
                LeftMouseDownCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.LeftMouseDown],
                LeftMouseUpCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.LeftMouseUp],
                RightMouseDownCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.RightMouseDown],
                RightMouseUpCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.RightMouseUp],
                MiddleMouseDownCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.MiddleMouseDown],
                MiddleMouseUpCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.MiddleMouseUp],
                MoveCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.Move],
                WheelCommand = remotingCommandHandlers.MouseCommands[GenericEnums.MouseCommandType.Wheel]
            };
            mouseCommands.BindCommands();

            commands = new Dictionary<GenericEnums.RemotingCommandType, IHookCommands>();
            commands.Add(GenericEnums.RemotingCommandType.Mouse, mouseCommands);

            KeyboardHookCommand keyboardCommands = new KeyboardHookCommand()
            {
                KeyDownCommand = remotingCommandHandlers.KeyboardCommands[GenericEnums.KeyboardCommandType.KeyDown],
                KeyPressCommand = remotingCommandHandlers.KeyboardCommands[GenericEnums.KeyboardCommandType.KeyPress],
                KeyUpCommand = remotingCommandHandlers.KeyboardCommands[GenericEnums.KeyboardCommandType.KeyUp]
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
