using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public interface IAudioRoom : IRoom
    {
        void PlayAudioCapture(byte[] capture);
    }
}
