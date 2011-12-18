using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

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

        public void RequestUpdateDesktop()
        {
            if (_clientModel != null)
            {
                _clientModel.UpdateDesktop();
                if (_clientView != null)
                {
                    SetView();
                }
            }
        }

        public void RequestUpdateMouseCursor()
        {
            if (_clientModel != null)
            {
                _clientModel.UpdateMouseCursor();
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
                _clientModel.Disconnect();
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
