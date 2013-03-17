using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public class RemotingCommandEventArgs : EventArgs
    {
        // todo: implement RemotingCommandEventArgs
        public GenericEnums.RemotingCommandType RemotingCommandType { get; set; }

        public GenericEnums.MouseCommandType MouseCommandType { get; set; }

        public GenericEnums.KeyboardCommandType KeyBoardCommandType { get; set; }
    }
}
