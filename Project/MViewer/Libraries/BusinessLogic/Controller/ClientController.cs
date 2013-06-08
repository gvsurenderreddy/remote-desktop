using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using System.Drawing;
using Utils;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Security;
using System.ServiceModel;

namespace BusinessLogicLayer
{
    public class ClientController : IClientController
    {
        #region private members

        IDictionary<string, IMViewerService> _clients;
        readonly object _syncClients = new object();
        bool _isSecured;

        #endregion

        #region c-tor

        public ClientController(bool isSecured)
        {
            lock (_syncClients)
            {
                _isSecured = isSecured;
                _clients = new Dictionary<string, IMViewerService>();
            }
        }

        #endregion

        #region public methods

        public void SendRemotingCommand(string receiverIdentity, EventArgs commandArgs)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(receiverIdentity);
                    }
                }
                client = (MViewerClient)_clients[receiverIdentity];
                try
                {
                    MemoryStream stream = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, commandArgs);
                    client.SendRemotingCommand(stream.GetBuffer());
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendRemotingCapture(byte[] screenCapture, byte[] mouseCapture, string receiverIdentity, string senderIdentity)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(receiverIdentity);
                    }
                }
                client = (MViewerClient)_clients[receiverIdentity];
                client.SendRemotingCapture(screenCapture, mouseCapture, senderIdentity);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public bool ConferencePermission(string partnerIdentity, string myIdentity, GenericEnums.RoomType roomType)
        {
            bool canStart = false;
            try
            {
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                client = (MViewerClient)_clients[partnerIdentity];
                canStart = client.ConferencePermission(myIdentity, roomType);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canStart;
        }

        public bool SendingPermission(string fileName, long fileSize, string partnerIdentity, string myIdentity)
        {
            bool canSend = false;
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[partnerIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(partnerIdentity);
                    }
                }
                client = (MViewerClient)_clients[partnerIdentity];
                canSend = client.SendingPermission(myIdentity, fileName, fileSize);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canSend;
        }

        public void SendFile(string myIdentity, byte[] fileBytes, string partnerIdentity, string fileName)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[partnerIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(partnerIdentity);
                    }
                }
                client = (MViewerClient)_clients[partnerIdentity];
                client.SendFile(fileBytes, fileName, myIdentity);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void WaitRoomButtonAction(string partnerIdentity, string myIdentity, GenericEnums.RoomType roomType, bool wait)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[partnerIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(partnerIdentity);
                    }
                }
                client = (MViewerClient)_clients[partnerIdentity];
                try
                {
                    client.WaitRoomButtonAction(myIdentity, roomType, wait);
                }
                catch { }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateContactStatus(string partnerIdentity, string myIdentity, GenericEnums.ContactStatus newStatus)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[partnerIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(partnerIdentity);
                    }
                }
                client = (MViewerClient)_clients[partnerIdentity];
                client.UpdateContactStatus(myIdentity, newStatus);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateFriendlyName(string partnerIdentity, string myIdentity, string newFriendlyName)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[partnerIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(partnerIdentity);
                    }
                }
                client = (MViewerClient)_clients[partnerIdentity];
                client.UpdateFriendlyName(myIdentity, newFriendlyName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendRoomCommand(string myIdentity, string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(identity))
                {
                    AddClient(identity);
                }
                else
                {
                    client = (MViewerClient)_clients[identity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(identity);
                    }
                }
                client = (MViewerClient)_clients[identity];
                client.SendRoomButtonAction(myIdentity, roomType, signalType);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public IMViewerService GetClient(string identity)
        {
            IMViewerService client = null;
            if (_clients != null && _clients.ContainsKey(identity))
            {
                client = _clients[identity];
            }
            return client;
        }

        public void AddClient(string identity)
        {
            try
            {
                lock (_syncClients)
                {
                    bool mustAdd = false;
                    if (_clients == null)
                    {
                        _clients = new Dictionary<string, IMViewerService>();
                    }
                    if (!_clients.ContainsKey(identity))
                    {
                        mustAdd = true;
                    }
                    else
                    {
                        if (
                            ((MViewerClient)_clients[identity]).State != CommunicationState.Created
                            && ((MViewerClient)_clients[identity]).State != CommunicationState.Opened)
                        {
                            _clients.Remove(identity);
                            mustAdd = true;
                        }
                    }
                    if (mustAdd)
                    {
                        ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);
                        Builder clientBuilder = new ClientBuilder(endpoint, _isSecured);
                        Director.Instance.Construct(clientBuilder);
                        IMViewerService client = (IMViewerService)clientBuilder.GetResult();
                        _clients.Add(identity, client);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void RemoveClient(string identity)
        {
            try
            {
                lock (_syncClients)
                {
                    if (_clients.ContainsKey(identity))
                    {
                        _clients.Remove(identity);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public bool IsContactOnline(string identity)
        {
            bool isOnline = false;
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(identity))
                {
                    AddClient(identity);
                }
                else
                {
                    client = (MViewerClient)_clients[identity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(identity);
                    }
                }
                client = (MViewerClient)_clients[identity];
                isOnline = client.Ping();
            }
            catch (SecurityNegotiationException ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
                isOnline = false;
            }
            catch
            {
                isOnline = false;
            }
            return isOnline;
        }

        public void SendVideoCapture(byte[] capture, DateTime timestamp, string receiverIdentity, string senderIdentity)
        {
            try
            {
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                client = (MViewerClient)_clients[receiverIdentity];
                try
                {
                    client.SendWebcamCapture(capture, timestamp, senderIdentity);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendAudioCapture(byte[] capture, DateTime timestamp, string receiverIdentity, string senderIdentity, double captureLengthInSeconds)
        {
            try
            {
                MViewerClient client = null;
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                }
                else
                {
                    client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != CommunicationState.Created && client.State != CommunicationState.Opened)
                    {
                        AddClient(receiverIdentity);
                    }
                }
                client = (MViewerClient)_clients[receiverIdentity];
                client.SendMicrophoneCapture(capture, timestamp, senderIdentity, captureLengthInSeconds);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
