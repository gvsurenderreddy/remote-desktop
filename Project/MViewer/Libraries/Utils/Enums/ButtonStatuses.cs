using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class ButtonStatuses
    {
        public enum ButtonStartStatus { Undefined = 0, Start = 1, Stop = 2 };
        public enum ButtonPauseStatus { Undefined = 0, Pause = 1, Resume = 2 };
        public enum AudioStatus { Undefined = 0, Muted = 1, Unmuted = 2 };
    }
}
