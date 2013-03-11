using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Utils;

namespace GenericDataLayer
{
    public interface IRoom
    {
        void SetPartnerName(string friendlyName);

        // todo: remove close room
        //void CloseRoom();
        
        void ShowRoom();

        string ContactIdentity
        {
            get;
            set;
        }

        GenericEnums.RoomType RoomType
        {
            get;
        }

        ManualResetEvent SyncClosing
        {
            get;
        }
    }
}
