﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIControls
{
    public static class ButtonStatuses
    {
        public enum ButtonStartStatus { Undefined = 0, Start = 1, Stop = 2 };
        public enum ButtonPauseStatus { Undefined = 0, Pause = 1, Resume = 2 };
        public enum ButtonType { Undefined = 0, Start = 1, Pause = 2, Audio = 3 };
        public enum AudioStatus { Undefined = 0, Muted = 1, Unmuted = 2 };
    }
}
