using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace WpfRemotingServer
{
    public class SingletonServer : MarshalByRefObject, Server
    {
        #region members

        Dictionary<int, Client> _connectedClients;

        #endregion

        #region c-tor

        public SingletonServer()
        {
            try
            {
                _connectedClients = new Dictionary<int, Client>();
                App.Logger.Info("Remoting Server Initialized");
            }
            catch (Exception ex)
            {
                App.Logger.Error("Server C-tor exception - " + ex.Message, ex);
            }
        }

        #endregion

        #region methods

        public void ShareDesktop(ref Client client)
        {
            _connectedClients.Add(_connectedClients.Count + 1, client);
            client.Id = _connectedClients.Count;
            DisplayClient(client.Id);
        }

        #endregion

        #region interface methods

        void DisplayClient(int id)
        {

        }

        #endregion
    }
}
