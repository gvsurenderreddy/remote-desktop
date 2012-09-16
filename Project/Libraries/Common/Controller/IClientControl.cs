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

        void SendWebcamCapture(byte[] capture);
        void RequestConnect();
        void RequestDisconnect();
        void RequestUpdateDesktop(ref Rectangle rect);
        void RequestUpdateMouseCursor(ref int x, ref int y);
        void SetModel(IClientModel clientModel);
        void SetView(IClientView clientView);
        void RequestAddCommand(CommandInfo command);

        #endregion
    }
}
