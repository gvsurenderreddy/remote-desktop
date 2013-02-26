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
        void NotifyActionsObserver();
        void BindObservers(bool bind);

        void ShowMainForm(bool close);
        void ShowMyWebcamForm(bool show);

        // todo: remove this method and use the Peer status in the Controller
        void PauseWebchat(bool pause);

        void UpdateLabels(string identity, GenericEnums.RoomType roomType);

        void UpdateWebcapture(Image image);

        void PerformRoomAction(object sender, EventArgs e);
  
        bool IsRoomActivated(string identity, GenericEnums.RoomType roomType);
        bool ExitConfirmation();

        IRoomManager RoomManager
        {
            get;
        }
    }
}
