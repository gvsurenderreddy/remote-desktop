using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DesktopSharing
{
    public static class CommandProcessor
    {
        #region static methods

        /// <summary>
        /// method used to execute a command
        /// </summary>
        /// <param name="command">command to execute</param>
        public static void ExecuteCommand(CommandInfo command)
        {
            if (command.CommandType == CommandUtils.CommandType.Mouse)
            {
                // if the command type is Mouse than move the mouse cursor
                MoveMouse(command.CommandString);
            }
            // no other command type available
        }

        /// <summary>
        /// method used to move the mouse cursor 
        /// </summary>
        /// <param name="commandString">command string</param>
        static void MoveMouse(string commandString)
        {
            string[] splitted = commandString.Split(',');
            if (splitted.Length == 2)
            {
                // parse the command arguments
                int x = int.Parse(splitted[0]);
                int y = int.Parse(splitted[1]);
                // move mouse cursor to specified position
                Cursor.Position = new Point(x, y);
            }
        }

        #endregion
    }
}
