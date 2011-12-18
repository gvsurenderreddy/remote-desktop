using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopSharing
{
    public static class CommandUtils
    {
        #region members

        public enum CommandType { undefined = 0, Mouse = 1 };

        #endregion

        #region static methods

        public static CommandInfo ParseArguments(string commandString)
        {
            CommandInfo commandInfo = null;
            string[] splitted = commandString.Split('|');
            if (splitted.Length != 2)
            {
                commandInfo = null;
            }
            else
            {
                CommandUtils.CommandType commandType = (CommandUtils.CommandType)Enum.Parse(typeof(CommandUtils.CommandType), splitted[0]);
                string arguments = splitted[1];
                commandInfo = new CommandInfo(commandType, arguments);
            }
            return commandInfo;
        }

        #endregion
    }
}
