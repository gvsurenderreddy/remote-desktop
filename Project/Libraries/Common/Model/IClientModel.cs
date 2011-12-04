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

        #endregion

        #region methods

        void Connect();
        void Disconnect();
        void AddObserver(IClientView clientView);
        void RemoveObserver(IClientView clientView);
        void NotifyObservers();
        void UpdateDesktop();
        void UpdateMouseCursor();

        #endregion
    }
}
