using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using System.Drawing;
using Utils;
using System.IO;
using System.Runtime.Serialization;

namespace BusinessLogicLayer
{
    public class ClientController : IClientController
    {
        #region private members

        IDictionary<string, IMViewerService> _clients;
        readonly object _syncClients = new object();

        #endregion

        #region c-tor

        public ClientController()
        {
            lock (_syncClients)
            {
                _clients = new Dictionary<string, IMViewerService>();
            }
        }

        #endregion

        #region public methods

        public void SendRemotingCommand(string receiverIdentity, EventArgs commandArgs)
        {
            try
            {
                if (_clients.ContainsKey(receiverIdentity))
                {
                    MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != System.ServiceModel.CommunicationState.Opened)
                    {
                        client.Open();
                    }
                    {
                        try
                        {
                            MemoryStream stream1 = new MemoryStream();
                            //Serialize the Record object to a memory stream using DataContractSerializer.
                            DataContractSerializer serializer = new DataContractSerializer(typeof(RemotingCommandEventArgs));
                            serializer.WriteObject(stream1, commandArgs);
                            client.SendRemotingCommand(stream1.GetBuffer());
                        }
                        catch (Exception)
                        {

                        }
                    }
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
                if (_clients.ContainsKey(receiverIdentity))
                {
                    MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != System.ServiceModel.CommunicationState.Opened)
                    {
                        client.Open();
                    }
                    try
                    {
                        client.SendRemotingCapture(screenCapture, mouseCapture, senderIdentity);
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public bool SendingPermission(string fileName, long fileSize, string partnerIdentity, string myIdentity)
        {
            bool canSend = false;
            try
            {
                if (_clients != null)
                {
                    if (_clients.ContainsKey(partnerIdentity))
                    {
                        MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                        if (client != null)
                        {
                            canSend = client.SendingPermission(myIdentity, fileName, fileSize);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canSend;
        }

        public void SendFile(byte[] fileBytes, string partnerIdentity, string fileName)
        {
            try
            {
                if (_clients != null)
                {
                    if (_clients.ContainsKey(partnerIdentity))
                    {
                        MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                        if (client != null)
                        {
                            client.SendFile(fileBytes, fileName);
                        }
                    }
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
                if (_clients != null)
                {
                    if (_clients.ContainsKey(partnerIdentity))
                    {
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
                if (_clients != null)
                {
                    if (_clients.ContainsKey(partnerIdentity))
                    {
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
                if (_clients != null)
                {
                    if (_clients.ContainsKey(partnerIdentity))
                    {
                        MViewerClient client = (MViewerClient)_clients[partnerIdentity];
                        if (client != null)
                        {
                            client.UpdateFriendlyName(myIdentity, newFriendlyName);
                        }
                    }
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
                if (_clients.ContainsKey(identity))
                {
                    MViewerClient client = (MViewerClient)_clients[identity];
                    try
                    {
                        client.SendRoomButtonAction(myIdentity, roomType, signalType);
                    }
                    catch { }
                }
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

                    Builder clientBuilder = new ClientBuilder(endpoint);
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

        public void SendVideoCapture(byte[] capture, string receiverIdentity, string senderIdentity)
        {
            try
            {
                if (_clients.ContainsKey(receiverIdentity))
                {
                    MViewerClient client = (MViewerClient)_clients[receiverIdentity];
                    if (client.State != System.ServiceModel.CommunicationState.Opened)
                    {
                        client.Open();
                    }
                    try
                    {
                        client.SendWebcamCapture(capture, senderIdentity);
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendAudioCapture(byte[] capture, string receiverIdentity, string senderIdentity)
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
                        client.Open();
                    }
                    try
                    {
                        client.SendMicrophoneCapture(capture, senderIdentity);
                    }
                    catch (Exception)
                    {

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
