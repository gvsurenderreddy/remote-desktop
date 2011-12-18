using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IClientModel
    {
        #region proprieties

        int Id
        {
            get;
        }

        string Ip
        {
            get;
        }

        bool Connected
        {
            get;
            set;
        }

        string Hostname
        {
            get;
        }

        bool IsServerConfigured
        {
            get;
        }


        #endregion

        #region methods

        void Connect();
        void Disconnect(bool checkStatus);
        void AddObserver(IClientView clientView);
        void RemoveObserver(IClientView clientView);
        void NotifyObservers();
        void UpdateDesktop();
        void UpdateMouseCursor();
        void StartTimer();
        void StopTimer();

        #endregion
    }
}
