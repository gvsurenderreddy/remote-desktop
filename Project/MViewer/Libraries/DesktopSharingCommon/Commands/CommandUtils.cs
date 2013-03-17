using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace DesktopSharing
{
    public static class CommandUtils
    {
        #region members

        
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
                GenericEnums.RemotingCommandType commandType = (GenericEnums.RemotingCommandType)Enum.Parse(typeof(GenericEnums.RemotingCommandType), splitted[0]);
                string arguments = splitted[1];
                commandInfo = new CommandInfo(commandType, arguments);
            }
            return commandInfo;
        }

        #endregion
    }
}
