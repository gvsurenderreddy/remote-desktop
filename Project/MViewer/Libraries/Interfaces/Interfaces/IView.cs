using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;
using System.Drawing;

namespace GenericObjects
{
    public interface IView
    {
        void ChangeLanguage(string language);
        void SetMessageText(string text);
        void NotifyContactsObserver();
        void NotifyIdentityObserver();
        void BindObservers(bool bind);
        void SetFormMainBackgroundImage(string filePath);
        void ShowMainForm(bool close);
        void ShowMyWebcamForm(bool show);

        void UpdateLabels(string identity, GenericEnums.RoomType roomType);
        void ResetLabels(GenericEnums.RoomType roomType);

        void UpdateWebcapture(Image image);

        void RoomButtonAction(object sender, EventArgs e);
  
        bool IsRoomActivated(string identity, GenericEnums.RoomType roomType);
        bool ExitConfirmation();
        void WaitRoomButtonAction(bool wait, GenericEnums.RoomType roomType);
        bool RequestTransferPermission(string identity, string fileName, long fileSize);
        void FocusActionsForm();

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
