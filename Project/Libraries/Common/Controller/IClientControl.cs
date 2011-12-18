using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DesktopSharing;

namespace Common
{
    public interface IClientControl
    {
        #region methods

        void RequestConnect();
        void RequestDisconnect();
        Bitmap RequestUpdateDesktop(ref Rectangle rect);
        Bitmap RequestUpdateMouseCursor(ref int x, ref int y);
        void SetModel(IClientModel clientModel);
        void SetView(IClientView clientView);
        void RequestAddCommand(CommandInfo command);

        #endregion
    }
}
