using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public class MouseHookCommands : MouseHookCommandsBase
    {
        public Delegates.HookCommandDelegate LeftClickCommand;
        public Delegates.HookCommandDelegate RightClickCommand;
        public Delegates.HookCommandDelegate DoubleRightClickCommand;
        public Delegates.HookCommandDelegate DoubleLeftClickCommand;
        public Delegates.HookCommandDelegate MiddleClickCommand;
        public Delegates.HookCommandDelegate DoubleMiddleClickCommand;
        public Delegates.HookCommandDelegate WheelCommand;
        public Delegates.HookCommandDelegate MoveCommand;
        public Delegates.HookCommandDelegate LeftMouseDownCommand;
        public Delegates.HookCommandDelegate LeftMouseUpCommand;
        public Delegates.HookCommandDelegate RightMouseDownCommand;
        public Delegates.HookCommandDelegate RightMouseUpCommand;
        public Delegates.HookCommandDelegate MiddleMouseDownCommand;
        public Delegates.HookCommandDelegate MiddleMouseUpCommand;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.MouseCommandType, Delegates.HookCommandDelegate>();
            _commands.Add(GenericEnums.MouseCommandType.LeftClick, LeftClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.RightClick, RightClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.DoubleRightClick, DoubleRightClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.DoubleLeftClick, DoubleLeftClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.MiddleClick, MiddleClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.DoubleMiddleClick, DoubleMiddleClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.LeftMouseDown, LeftMouseDownCommand);
            _commands.Add(GenericEnums.MouseCommandType.LeftMouseUp, LeftMouseUpCommand);
            _commands.Add(GenericEnums.MouseCommandType.RightMouseDown, RightMouseDownCommand);
            _commands.Add(GenericEnums.MouseCommandType.RightMouseUp, RightMouseUpCommand);
            _commands.Add(GenericEnums.MouseCommandType.MiddleMouseDown, MiddleMouseDownCommand);
            _commands.Add(GenericEnums.MouseCommandType.MiddleMouseUp, MiddleMouseUpCommand);
            _commands.Add(GenericEnums.MouseCommandType.Move, MoveCommand);
            _commands.Add(GenericEnums.MouseCommandType.Wheel, WheelCommand);
        }
    }
}
