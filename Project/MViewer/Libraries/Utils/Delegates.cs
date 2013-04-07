using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//

namespace GenericObjects
{
    public class Delegates
    {
        public delegate void RoomCommandDelegate(object sender, EventArgs args);
        public delegate void HookCommandDelegate(object sender, EventArgs args);

        public delegate void DesktopChangedEventHandler(Image display, string partnerIdentity);
        
        public delegate void IdentityEventHandler(object o, EventArgs e);
        public delegate void ContactsEventHandler(object o, EventArgs e);
        public delegate void ActionsEventHandler(object o, EventArgs e);

        // event delegate fired when a new image is captured by the webcam device
        public delegate void WebCamEventHandler(object source, EventArgs e);

        public delegate void CloseDelegate();
       
        public delegate void StartPresenting(bool firstTimeCapturing);

    }
}
