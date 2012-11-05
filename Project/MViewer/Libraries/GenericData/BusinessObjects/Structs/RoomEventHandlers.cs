﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public struct ControllerEventHandlers
    {
        public EventHandler ClientConnectedObserver
        {
            get;
            set;
        }

        public EventHandler RoomClosingObserver
        {
            get;
            set;
        }

        public EventHandler VideoCaptureObserver
        {
            get;
            set;
        }

        public EventHandler AudioCaptureObserver
        {
            get;
            set;
        }

        public EventHandler ScreenSCaptureObserver
        {
            get;
            set;
        }

        public EventHandler MouseCaptureObserver
        {
            get;
            set;
        }

        public EventHandler ContactsObserver
        {
            get;
            set;
        }
    }
}
