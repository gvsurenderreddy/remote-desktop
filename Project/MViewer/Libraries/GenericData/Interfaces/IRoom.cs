using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public interface IRoom
    {
        void SetPartnerName(string friendlyName);
        void CloseRoom();
        void ShowRoom();

        GenericEnums.RoomActionType RoomType
        {
            get;
        }
    }
}
