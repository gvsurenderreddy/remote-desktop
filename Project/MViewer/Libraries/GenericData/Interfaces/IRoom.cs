using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Utils;

namespace GenericObjects
{
    public interface IRoom
    {
        void SetPartnerName(string friendlyName);

        void ShowRoom();
        void ChangeLanguage(string language);

        string PartnerIdentity
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
