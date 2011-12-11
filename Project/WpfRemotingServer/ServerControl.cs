using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Runtime.Remoting;

namespace WpfRemotingServer
{
    public class ServerControl : IServerControl
    {
         #region members

        IServerModel _serverModel;
        IServerView _serverView;

        #endregion

        #region c-tor

        public ServerControl(IServerModel serverModel, IServerView serverView)
        {
            this._serverModel = serverModel;
            this._serverView = serverView;
        }

        #endregion

        #region methods

        public void SetModel(IServerModel serverModel)
        {
            this._serverModel = serverModel;
        }

        public void SetView(IServerView serverView)
        {
            this._serverView = serverView;
        }

        public int AddClient(string ip, string hostname)
        {
            int newID = -1;
            if(_serverModel != null)
            {
                newID = _serverModel.AddClient(ip, hostname);
                if(_serverView != null) 
                {
                    SetView();
                }
            }
            return newID;
        }

        public void RemoveClient(int id)
        {
            if (_serverModel != null)
            {
                _serverModel.RemoveClient(id);
                if (_serverView != null)
                {
                    SetView();
                }
            }
        }

        public void RequestStartServer()
        {
            if (_serverModel != null)
            {
                _serverModel.StartServer();
                if (_serverView != null)
                {
                    SetView();
                }
            }
        }

        public void RequestStopServer()
        {
            if (_serverModel != null)
            {
                _serverModel.StopServer();
                _serverModel.RemoveAllClients();
                if (_serverView != null)
                {
                    SetView();
                }
            }
        }

        public void SetView()
        {
            if (_serverView != null)
            {
                _serverView.Update(_serverModel);
            }
        }
        
        public void UpdateDesktop()
        {
            if (_serverModel != null)
            {
                _serverModel.UpdateDesktop();
                if (_serverView != null)
                {
                    SetView();
                }
            }
        }

        public void UpdateMouseCursor()
        {
            if (_serverModel != null)
            {
                _serverModel.UpdateMouseCursor();
                if (_serverView != null)
                {
                    SetView();
                }
            }
        }

        #endregion
    }
}
