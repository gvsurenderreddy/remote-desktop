using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace GenericObjects
{
    public class RemotingCommandEventArgs : EventArgs
    {
        // todo: implement RemotingCommandEventArgs
        public GenericEnums.RemotingCommandType RemotingCommandType { get; set; }

        public GenericEnums.MouseCommandType MouseCommandType { get; set; }

        public GenericEnums.KeyboardCommandType KeyboardCommandType { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public int Delta { get; set; }

        public Keys KeyCode { get; set; }
        public char KeyChar { get; set; }
    }
}
