using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace GenericObjects
{
    public class Delegates
    {
        public delegate void RoomCommandDelegate(object sender, RoomActionEventArgs args);
        public delegate void HookCommandDelegate(object sender, RemotingCommandEventArgs args);

        public delegate void DesktopChangedEventHandler(System.Drawing.Image display, string partnerIdentity);
        
        public delegate void IdentityEventHandler(object o, IdentityEventArgs e);
        public delegate void ContactsEventHandler(object o, ContactsEventArgs e);
        public delegate void ActionsEventHandler(object o, RoomActionEventArgs e);

        // event delegate fired when a new image is captured by the webcam device
        public delegate void WebCamEventHandler(object source, VideoCaptureEventArgs e);

        public delegate void CloseDelegate();
       
        public delegate void StartPresenting(bool firstTimeCapturing);

    }
}
