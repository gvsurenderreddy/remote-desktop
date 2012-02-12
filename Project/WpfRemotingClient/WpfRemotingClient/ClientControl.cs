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

        public void RequestUpdateDesktop(ref Rectangle rect)
        {
            if (_clientModel != null)
            {
                _clientModel.UpdateDesktop(rect);
                //if (_clientView != null)
                //{
                //    SetView();
                //}
            }
        }

        public void RequestUpdateMouseCursor(ref int x, ref int y)
        {
            if (_clientModel != null)
            {
                _clientModel.UpdateMouseCursor(ref x, ref y);
                if (_clientView != null)
                {
                    SetView();
                }
            }
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
