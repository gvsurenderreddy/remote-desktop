using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace DesktopSharing
{
    public class CommandInfo
    {
        #region members

        GenericEnums.RemotingCommandType _type;
        string _arguments;

        #endregion

        #region c-tor

        public CommandInfo(GenericEnums.RemotingCommandType type, string arguments)
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

        public GenericEnums.RemotingCommandType CommandType
        {
            get { return _type; }
        }

        public string CommandString
        {
            get { return _arguments; }
        }

        #endregion
    }
}
