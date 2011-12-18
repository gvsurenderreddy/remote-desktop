using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;

namespace WpfRemotingClient
{
    public class ClientControl : IClientControl
    {
        #region members

        IClientModel _clientModel;
        IClientView _clientView;

        #endregion

        #region c-tor

        public ClientControl(IClientModel clientModel, IClientView clientView)
        {
            this._clientModel = clientModel;
            this._clientView = clientView;
        }

        #endregion

        #region methods

        public void RequestAddCommand(DesktopSharing.CommandInfo command)
        {
            if (_clientModel != null)
            {
                _clientModel.AddCommand(command);
                if (_clientView != null)
                {
                    SetView();
                }
            }
        }

        public Bitmap RequestUpdateDesktop(ref Rectangle rect)
        {
            Bitmap desktopCapture = null;
            if (_clientModel != null)
            {
                desktopCapture = _clientModel.UpdateDesktop(rect);
                if (_clientView != null)
                {
                    SetView();
                }
            }
            return desktopCapture;
        }

        public Bitmap RequestUpdateMouseCursor(ref int x, ref int y)
        {
            Bitmap mouseCapture = null;
            if (_clientModel != null)
            {
                mouseCapture = _clientModel.UpdateMouseCursor(ref x, ref y);
                if (_clientView != null)
                {
                    SetView();
                }
            }
            return mouseCapture;
        }

        public void SetModel(IClientModel clientModel)
        {
            this._clientModel = clientModel;
        }

        public void SetView(IClientView clientView)
        {
            this._clientView = clientView;
        }

        public void RequestConnect()
        {
            if(_clientModel != null)
            {
                _clientModel.Connect();
                if(_clientView != null) 
                {
                    SetView();
                }
            }
        }

        public void RequestDisconnect()
        {
            if (_clientModel != null)
            {
                _clientModel.Disconnect(false);
                if (_clientView != null)
                {
                    SetView();
                }
            }
        }

        public void SetView()
        {
            if (_clientView != null)
            {
                _clientView.Update(_clientModel);
            }
        }

        #endregion
    }
}
