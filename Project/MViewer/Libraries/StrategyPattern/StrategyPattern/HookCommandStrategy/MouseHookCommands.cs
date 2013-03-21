using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace StrategyPattern
{
    public class MouseHookCommand : MouseHookCommandBase
    {
        public Delegates.HookCommandDelegate LeftClickCommand;
        public Delegates.HookCommandDelegate RightClickCommand;
        public Delegates.HookCommandDelegate DoubleRightClickCommand;
        public Delegates.HookCommandDelegate DoubleLeftClickCommand;
        public Delegates.HookCommandDelegate MiddleClickCommand;
        public Delegates.HookCommandDelegate DoubleMiddleClickCommand;
        public Delegates.HookCommandDelegate WheelCommand;
        public Delegates.HookCommandDelegate MoveCommand;
        public Delegates.HookCommandDelegate MouseDownCommand;
        public Delegates.HookCommandDelegate MouseUpCommand;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.MouseCommandType, Delegates.HookCommandDelegate>();
            _commands.Add(GenericEnums.MouseCommandType.LeftClick, LeftClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.RightClick, RightClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.DoubleRightClick, DoubleRightClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.DoubleLeftClick, DoubleLeftClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.MiddleClick, MiddleClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.DoubleMiddleClick, DoubleMiddleClickCommand);
            _commands.Add(GenericEnums.MouseCommandType.MouseDown, MouseDownCommand);
            _commands.Add(GenericEnums.MouseCommandType.MouseUp, MouseUpCommand);
            _commands.Add(GenericEnums.MouseCommandType.Move, MoveCommand);
            _commands.Add(GenericEnums.MouseCommandType.Wheel, WheelCommand);

        }
    }
}
