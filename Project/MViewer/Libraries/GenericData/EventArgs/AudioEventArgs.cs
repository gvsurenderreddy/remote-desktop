using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public class AudioEventArgs : EventArgs
    {
        public byte[] Capture { get; set; }
        public string Identity { get; set; }
    }
}
