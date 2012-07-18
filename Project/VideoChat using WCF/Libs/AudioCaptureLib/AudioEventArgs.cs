using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioCaptureLib
{
    public class AudioEventArgs
    {
        #region private members

        byte[] _capturedSound;

        #endregion

        #region c-tor

        public AudioEventArgs(byte[] capturedSound)
        {
            _capturedSound = capturedSound;
        }

        #endregion

        #region proprieties

        public byte[] CapturedSound
        {
            get 
            { 
                return _capturedSound; 
            }
        }

        #endregion
    }
}
