using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DesktopSharing;

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
        void UpdateDesktop(Rectangle rect);
        void UpdateMouseCursor(ref int x, ref int y);
        void AddCommand(CommandInfo command);
        //void StartTimer();
        //void StopTimer();

        #endregion
    }
}
