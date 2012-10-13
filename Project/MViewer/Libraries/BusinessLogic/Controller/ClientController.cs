using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    public class ClientController : IClientController
    {
        #region private members

        Dictionary<string, MViewerClient> _clients;
        readonly object _syncClients = new object();

        #endregion

        #region c-tor

        public ClientController()
        {
            lock (_syncClients)
            {
                _clients = new Dictionary<string, MViewerClient>();
            }
        }

        #endregion

        #region private methods


        #endregion

        #region public methods

        public void AddClient(string identity)
        {
            lock (_syncClients)
            {
                if (_clients.ContainsKey(identity))
                {
                    _clients.Remove(identity);
                } 
                ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);
                _clients.Add(identity, ClientBuilder.BuildWCFClient(endpoint));
            }
        }

        public void RemoveClient(string identity)
        {
            lock (_syncClients)
            {
                if (_clients.ContainsKey(identity))
                {
                    _clients.Remove(identity);
                }
            }
        }

        public void StopClient(string identity)
        {
            lock (_syncClients)
            {
                if (_clients.ContainsKey(identity))
                {
                    _clients[identity].Close();
                }
            }
        }

        public void StartClient(string identity)
        {
            lock (_syncClients)
            {
                //if (!_clients.ContainsKey(identity))
                //{
                //    AddClient(identity);
                //} 
                _clients[identity].Open();
            }
        }

        #endregion
    }
}
