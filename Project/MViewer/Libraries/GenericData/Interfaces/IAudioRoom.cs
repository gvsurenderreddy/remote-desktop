using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public interface IAudioRoom : IRoom
    {
        void PlayAudioCapture(byte[] capture);
        void ToggleAudioStatus();
    }
}
