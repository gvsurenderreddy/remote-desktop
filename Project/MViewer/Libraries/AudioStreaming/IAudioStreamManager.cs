using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioStreaming
{
    public interface IAudioStreamManager
    {
        void StartStreaming();
        void StopStreaming();
        void PlayAudioCapture(byte[] capture);
    }
}
