using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopSharing
{
    public class CommandQueue
    {
        #region members

        readonly Queue<CommandInfo> _commands;

        #endregion

        #region c-tor

        public CommandQueue()
        {
            _commands = new Queue<CommandInfo>();
        }

        #endregion

        #region methods

        /// <summary>
        /// method used to retrieve next command from the queue
        /// </summary>
        /// <returns>next command</returns>
        public CommandInfo Next()
        {
            lock (_commands)
            {
                CommandInfo command = null;
                try
                {
                    command = _commands.Dequeue();
                }
                catch{}
                return command;
            }
        }

        /// <summary>
        /// method used to add a new command to the queue
        /// </summary>
        /// <param name="type">command type</param>
        /// <param name="commandString">command arguments</param>
        public void AddCommand(CommandUtils.CommandType type, string commandString)
        {
            CommandInfo command = new CommandInfo(type, commandString);
            AddCommand(command);
        }

        /// <summary>
        /// method used to add a new command to the queue
        /// </summary>
        /// <param name="command">command to add</param>
        public void AddCommand(CommandInfo command)
        {
            lock (_commands)
            {
                _commands.Enqueue(command);
            }
        }

        /// <summary>
        /// method used to serialize the commands queue
        /// </summary>
        /// <returns>serialized commands as string</returns>
        public string Serialize()
        {
            lock (_commands)
            {
                StringBuilder builder = new StringBuilder();
                foreach (CommandInfo command in _commands)
                {
                    builder.Append(command.GetCommand());
                    builder.Append(";");
                }
                _commands.Clear();
                return builder.ToString();
            }
        }

        /// <summary>
        /// method used to deserialize commands from string
        /// </summary>
        /// <param name="serializedCommands">serialized commands as string</param>
        public void Deserialize(string serializedCommands)
        {
            lock (_commands)
            {
                string[] splitted = serializedCommands.Split(';');
                foreach (string serializedCommand in splitted)
                {
                    string part = serializedCommand.Trim();
                    if (!string.IsNullOrEmpty(part))
                    {
                        CommandInfo commandInfo = CommandUtils.ParseArguments(part);
                        if (commandInfo != null)
                        {
                            AddCommand(commandInfo);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
