using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IServerView
    {
        #region methods

        int AddClient(string ip, string hostname);
        void RemoveClient(int id);
        void Update(IServerModel serverModel);

        #endregion
    }
}
