using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.Drawing;
using Utils;
using System.IO;

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

        public void SendRemotingCommand(string receiverIdentity, RemotingCommandEventArgs commandArgs)
        {
            if (_clients.ContainsKey(receiverIdentity))
            {
                MViewerClient client = _clients[receiverIdentity];
                if (client.State == System.ServiceModel.CommunicationState.Opened)
                {
                    try
                    {
                        client.SendRemotingCommand(commandArgs);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public void SendRemotingCapture(byte[] screenCapture, byte[] mouseCapture, string receiverIdentity, string senderIdentity)
        {
            if (_clients.ContainsKey(receiverIdentity))
            {
                MViewerClient client = _clients[receiverIdentity];
                if (client.State == System.ServiceModel.CommunicationState.Opened)
                {
                    try
                    {
                        client.SendRemotingCapture(screenCapture, mouseCapture, senderIdentity);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public bool SendingPermission(string fileName, long fileSize, string partnerIdentity, string myIdentity)
        {
            bool canSend = false;
            if (_clients != null)
            {
                if (_clients.ContainsKey(partnerIdentity))
                {
                    MViewerClient client = _clients[partnerIdentity];
                    if (client != null)
                    {
                        canSend = client.SendingPermission(myIdentity, fileName, fileSize);
                    }
                }
            }
            return canSend;
        }

        public void SendFile(byte[] fileBytes, string partnerIdentity, string fileName)
        {
            if (_clients != null)
            {
                if (_clients.ContainsKey(partnerIdentity))
                {
                    MViewerClient client = _clients[partnerIdentity];
                    if (client != null)
                    {
                        client.SendFile(fileBytes, fileName);
                    }
                }
            }
        }

        public void WaitRoomButtonAction(string partnerIdentity, string myIdentity, GenericEnums.RoomType roomType, bool wait)
        {
            if (_clients != null)
            {
                if (_clients.ContainsKey(partnerIdentity))
                {
                    MViewerClient client = _clients[partnerIdentity];
                    if (client != null)
                    {
                        client.WaitRoomButtonAction(myIdentity, roomType, wait);
                    }
                }
            }
        }

        public void UpdateContactStatus(string partnerIdentity, string myIdentity, GenericEnums.ContactStatus newStatus)
        {
            if (_clients != null)
            {
                if (_clients.ContainsKey(partnerIdentity))
                {
                    MViewerClient client = _clients[partnerIdentity];
                    if (client != null)
                    {
                        try
                        {
                            client.UpdateContactStatus(myIdentity, newStatus);
                        }
                        catch 
                        {
                        
                        }
                    }
                }
            }
        }

        public void UpdateFriendlyName(string partnerIdentity, string myIdentity, string newFriendlyName)
        {
            if (_clients != null)
            {
                if (_clients.ContainsKey(partnerIdentity))
                {
                    MViewerClient client = _clients[partnerIdentity];
                    if (client != null)
                    {
                        client.UpdateFriendlyName(myIdentity, newFriendlyName);
                    }
                }
            }
        }

        public void SendRoomCommand(string myIdentity, string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType)
        {
            if (_clients.ContainsKey(identity))
            {
                MViewerClient client = _clients[identity];
                client.SendRoomButtonAction(myIdentity, roomType, signalType);
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

                Builder clientBuilder = new ClientBuilder(endpoint);
                Director.Instance.Construct(clientBuilder);
                MViewerClient client = (MViewerClient)clientBuilder.GetResult();
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
            bool isOnline = false;
            try
            {
                ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);
                Builder clientBuilder = new ClientBuilder(endpoint);
                Director.Instance.Construct(clientBuilder);
                MViewerClient client = (MViewerClient)clientBuilder.GetResult();
                isOnline = client.Ping();
            }
            catch
            {
                isOnline = false;
            }
            return isOnline;
        }

        public void SendVideoCapture(byte[]capture, string receiverIdentity, string senderIdentity)
        {
            if(_clients.ContainsKey(receiverIdentity))
            {
                MViewerClient client = _clients[receiverIdentity];
                if (client.State == System.ServiceModel.CommunicationState.Opened)
                {
                    try
                    {
                        client.SendWebcamCapture(capture, senderIdentity);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        #endregion
    }
}
