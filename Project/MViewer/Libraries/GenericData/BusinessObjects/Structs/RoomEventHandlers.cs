using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public struct ControllerEventHandlers
    {
        public EventHandler ClientConnectedHandler
        {
            get;
            set;
        }

        public EventHandler RoomClosingHandler
        {
            get;
            set;
        }

        public EventHandler VideoCaptureHandler
        {
            get;
            set;
        }

        public EventHandler AudioCaptureHandler
        {
            get;
            set;
        }

        public EventHandler ScreenSCaptureHandler
        {
            get;
            set;
        }

        public EventHandler MouseCaptureHandler
        {
            get;
            set;
        }

        public EventHandler ContactsHandler
        {
            get;
            set;
        }
    }
}
