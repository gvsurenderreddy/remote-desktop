using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//
using System.Windows.Forms;
using Utils;

namespace GenericObjects
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(RemotingCommandEventArgs))]
    public class RemotingCommandEventArgs : EventArgs
    {
        [DataMember]
       public GenericEnums.RemotingCommandType RemotingCommandType { get; set; }
        [DataMember]
        public GenericEnums.MouseCommandType MouseCommandType { get; set; }
        [DataMember]
        public GenericEnums.KeyboardCommandType KeyboardCommandType { get; set; }
        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public int Delta { get; set; }

        [DataMember]
        public Keys KeyCode { get; set; }

        [DataMember]
        public char KeyChar { get; set; }

        public byte[] MouseMoves { get; set; }
    }
}
