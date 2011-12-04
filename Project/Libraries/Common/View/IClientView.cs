using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IClientView
    {
        #region methods

        void Connect();
        void Disconnect();
        void Update(IClientModel clientModel);

        #endregion
    }
}
