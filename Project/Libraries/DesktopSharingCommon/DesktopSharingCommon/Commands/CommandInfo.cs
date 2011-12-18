using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopSharing
{
    public class CommandInfo
    {
        #region members

        CommandUtils.CommandType _type;
        string _arguments;

        #endregion

        #region c-tor

        public CommandInfo(CommandUtils.CommandType type, string arguments)
        {
            _arguments = arguments;
            _type = type;
        }

        #endregion

        #region methods

        public string GetCommand()
        {
            return _type + "|" + _arguments;
        }

        #endregion

        #region proprieties

        public CommandUtils.CommandType CommandType
        {
            get { return _type; }
            //set { _type = value; }
        }

        public string CommandString
        {
            get { return _arguments; }
            //set { _arguments = value; }
        }

        #endregion
    }
}
