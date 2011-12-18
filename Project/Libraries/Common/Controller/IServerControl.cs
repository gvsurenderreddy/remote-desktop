using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IServerControl
    {
        #region methods

        int AddClient(string ip, string hostname);
        void RemoveClient(int id, bool checkStatus);
        void SetModel(IServerModel serverModel);
        void SetView(IServerView serverView);
        void RequestStartServer();
        void RequestStopServer();

        #endregion
    }
}
