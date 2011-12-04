using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IClientControl
    {
        #region methods

        void RequestConnect();
        void RequestDisconnect();
        void RequestUpdateDesktop();
        void RequestUpdateMouseCursor();
        void SetModel(IClientModel clientModel);
        void SetView(IClientView clientView);

        #endregion
    }
}
