using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.Drawing;
using Utils;

namespace BusinessLogicLayer
{
    public class ClientController : IClientController
    {
        #region private members

        IDictionary<string, MViewerClient> _clients;
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

        public void SendRoomCommand(string identity, GenericEnums.RoomActionType roomType, GenericEnums.SignalType signalType)
        {
            if (_clients.ContainsKey(identity))
            {
                MViewerClient client = _clients[identity];
                client.SendRoomAction(identity, roomType, signalType);
            }
        }
        
        public MViewerClient GetClient(string identity)
        {
            MViewerClient client = null;
            if (_clients != null && _clients.ContainsKey(identity))
            {
                client = _clients[identity];
            }
            return client;
        }

        public void AddClient(string identity)
        {
            lock (_syncClients)
            {
                if (_clients == null)
                {
                    _clients = new Dictionary<string, MViewerClient>();
                }
                if (_clients.ContainsKey(identity))
                {
                    _clients.Remove(identity);
                } 
                ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);
                MViewerClient client = ClientBuilder.BuildWCFClient(endpoint);
                _clients.Add(identity, client);
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
                if (!_clients.ContainsKey(identity))
                {
                    AddClient(identity);
                }
                MViewerClient mviewerClient = _clients[identity];
                if (mviewerClient.State != System.ServiceModel.CommunicationState.Closed && mviewerClient.State != System.ServiceModel.CommunicationState.Opening && mviewerClient.State != System.ServiceModel.CommunicationState.Opened)
                {
                    mviewerClient.Open();
                }
            }
        }

        public bool IsContactOnline(string identity)
        {
            ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);
            MViewerClient client = ClientBuilder.BuildWCFClient(endpoint);
            return client.Ping();
        }

        public void SendCapture(byte[]capture, string receiverIdentity, string senderIdentity)
        {
            if(_clients.ContainsKey(receiverIdentity))
            {
                MViewerClient client = _clients[receiverIdentity];
                if (client.State == System.ServiceModel.CommunicationState.Opened)
                {
                    client.SendWebcamCapture(capture, senderIdentity);
                }
            }
        }

        #endregion
    }
}
