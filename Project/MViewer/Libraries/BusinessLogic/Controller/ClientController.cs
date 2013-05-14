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
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                    StartClient(receiverIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                try
                {
                    MemoryStream stream = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, commandArgs);
                    client.SendRemotingCommand(stream.GetBuffer());
                }
                catch (Exception)
                {

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
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                    StartClient(receiverIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                //if (client.State != System.ServiceModel.CommunicationState.Opened)
                //{
                //    client.Open();
                //}
                try
                {
                    client.SendRemotingCapture(screenCapture, mouseCapture, senderIdentity);
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

        public bool ConferencePermission(string partnerIdentity, string myIdentity, GenericEnums.RoomType roomType)
        {
            bool canStart = false;
            try
            {
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                    StartClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                if (client != null)
                {
                    canStart = client.ConferencePermission(myIdentity, roomType);
                }
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
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                    StartClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                if (client != null)
                {
                    canSend = client.SendingPermission(myIdentity, fileName, fileSize);
                }
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
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                    StartClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                if (client != null)
                {
                    client.SendFile(fileBytes, fileName, myIdentity);
                }
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
                if (_clients != null && !string.IsNullOrEmpty(partnerIdentity) && !_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                    StartClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                if (client != null)
                {
                    try
                    {
                        client.WaitRoomButtonAction(myIdentity, roomType, wait);
                    }
                    catch { }
                }
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
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                    StartClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateFriendlyName(string partnerIdentity, string myIdentity, string newFriendlyName)
        {
            try
            {
                if (!_clients.ContainsKey(partnerIdentity))
                {
                    AddClient(partnerIdentity);
                    StartClient(partnerIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                if (client != null)
                {
                    client.UpdateFriendlyName(myIdentity, newFriendlyName);
                }
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
                if (!_clients.ContainsKey(identity))
                {
                    AddClient(identity);
                    StartClient(identity);
                }
                MViewerClient client = (MViewerClient)_clients[identity];
                try
                {
                    client.SendRoomButtonAction(myIdentity, roomType, signalType);
                }
                catch { }
                
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
                    if (_clients == null)
                    {
                        _clients = new Dictionary<string, IMViewerService>();
                    }
                    if (_clients.ContainsKey(identity))
                    {
                        _clients.Remove(identity);
                    }
                    ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);

                    Builder clientBuilder = new ClientBuilder(endpoint, _isSecured);
                    Director.Instance.Construct(clientBuilder);
                    IMViewerService client = (IMViewerService)clientBuilder.GetResult();
                    _clients.Add(identity, client);
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

        public void StopClient(string identity)
        {
            try
            {
                lock (_syncClients)
                {
                    if (_clients.ContainsKey(identity))
                    {
                        ((MViewerClient)_clients[identity]).Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StartClient(string identity)
        {
            try
            {
                lock (_syncClients)
                {
                    if (!_clients.ContainsKey(identity))
                    {
                        AddClient(identity);
                    }
                    MViewerClient mviewerClient = (MViewerClient)_clients[identity];
                    if (mviewerClient.State != System.ServiceModel.CommunicationState.Closed && mviewerClient.State != System.ServiceModel.CommunicationState.Opening && mviewerClient.State != System.ServiceModel.CommunicationState.Opened)
                    {
                        mviewerClient.Open();
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
                ContactEndpoint endpoint = IdentityResolver.ResolveIdentity(identity);
                Builder clientBuilder = new ClientBuilder(endpoint, _isSecured);
                Director.Instance.Construct(clientBuilder);
                MViewerClient client = (MViewerClient)clientBuilder.GetResult();
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
                    StartClient(receiverIdentity);
                }
                MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                try
                {
                    client.SendWebcamCapture(capture, timestamp, senderIdentity);
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendAudioCapture(byte[] capture, DateTime timestamp, string receiverIdentity, string senderIdentity)
        {
            try
            {
                if (!_clients.ContainsKey(receiverIdentity))
                {
                    AddClient(receiverIdentity);
                    StartClient(receiverIdentity);
                }
                if (_clients.ContainsKey(receiverIdentity))
                {
                    MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != System.ServiceModel.CommunicationState.Opened)
                    {
                        RemoveClient(receiverIdentity);
                        AddClient(receiverIdentity);
                        StartClient(receiverIdentity);
                        client = (MViewerClient)_clients[receiverIdentity];
                    }
                    try
                    {
                        client.SendMicrophoneCapture(capture, timestamp, senderIdentity);
                    }
                    catch (Exception ex)
                    {
                        Tools.Instance.Logger.LogError(ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
