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

        void IdentityObserver(object sender, IdentityEventArgs e);
        void RoomButtonAction(object sender, RoomActionEventArgs e);
        Contact PerformContactsOperation(object sender, ContactsEventArgs ee);
        void StartVideoChat(WebcamCapture webcamControl);
        void RoomClosingObserver(object sender, EventArgs e);
        void ActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType);
        void FileTransferObserver(object sender, EventArgs e);
        void FileTransferPermission(object sender, EventArgs e);
    }
}
