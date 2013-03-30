using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public interface IAudioStreamManager
    {
        void StartStreaming();
        void StopStreaming();
        void PlayAudioCapture(byte[] capture);
    }
}
