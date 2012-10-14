using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GenericDataLayer
{
    public interface IController
    {
        void StartApplication();
        void StopApplication();
        void NotifyContactsObserver();
        void GetContactsStatus();
        void IdentityUpdated(object sender, IdentityEventArgs e);
        void PerformRoomAction(object sender, RoomActionEventArgs e);
        Contact PerformContactsOperation(object sender, ContactsEventArgs e);
        void StartVideoChat(WebcamCapture webcamControl, RoomActionEventArgs e);
    }
}
