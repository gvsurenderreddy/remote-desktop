using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;
using System.Drawing;

namespace GenericDataLayer
{
    public interface IView
    {
        void NotifyContactsObserver();
        void NotifyIdentityObserver();
        void BindObservers(bool bind);

        void ShowMainForm(bool close);
        void ShowMyWebcamForm(bool show);

        void UpdateLabels(string identity, GenericEnums.RoomType roomType);
        void ResetLabels(GenericEnums.RoomType roomType);

        void UpdateWebcapture(Image image);

        void RoomButtonAction(object sender, EventArgs e);
  
        bool IsRoomActivated(string identity, GenericEnums.RoomType roomType);
        bool ExitConfirmation();
        void WaitRoomButtonAction(bool wait);
        bool RequestTransferPermission(string identity, string fileName, long fileSize);

        bool VideoCaptureClosed
        {
            get;
        }

        IRoomManager RoomManager
        {
            get;
        }
    }
}
