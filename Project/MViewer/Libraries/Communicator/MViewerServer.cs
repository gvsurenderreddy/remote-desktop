using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using Utils;
using System.Drawing;
using System.Drawing.Imaging;
using GenericObjects;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using Abstraction;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communicator
{
    public class MViewerServer : Product, IMViewerService
    {
        #region private members

        WSHttpBinding _binding;
        Uri _httpURI;
        string _httpsAddress;
        ControllerEventHandlers _controllerHandlers;
        string _identity;
        ManualResetEvent _syncVideoCaptures = new ManualResetEvent(true);
        ManualResetEvent _syncRemotingCaptures = new ManualResetEvent(true);
        ManualResetEvent _syncAudioCaptures = new ManualResetEvent(true);
        
        #endregion

        #region c-tor

        public MViewerServer() 
        { 

        }

        public MViewerServer(ControllerEventHandlers controllerHandlers, string identity)
        {
            _identity = identity;
            _controllerHandlers = controllerHandlers;
        }

        #endregion

        #region public methods

        public void SendRemotingCapture(byte[] screenCapture, byte[] mouseCapture, string senderIdentity)
        {
            try
            {
                _syncRemotingCaptures.WaitOne();
                _controllerHandlers.RemotingCaptureObserver.Invoke(this,
                    new RemotingCaptureEventArgs()
                    {
                        Identity = senderIdentity,
                        ScreenCapture = screenCapture,
                        MouseCapture = mouseCapture
                    });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public bool SendingPermission(string senderIdentity, string fileName, long fileSize)
        {
            bool canSend = false;
            try
            {
                TransferInfo transferInfo = new TransferInfo()
                    {
                        FileName = fileName,
                        FileSize = fileSize
                    };

                // request permission from the user
                _controllerHandlers.FilePermissionObserver.Invoke(this,
                    new RoomActionEventArgs()
                    {
                        Identity = senderIdentity,
                        RoomType = GenericEnums.RoomType.Send,
                        TransferInfo = transferInfo
                    });
                canSend = transferInfo.HasPermission;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canSend;
        }

        public void SendFile(byte[] fileStream, string fileName, string senderIdentity)
        {
            try
            {
                // SendFile
                _controllerHandlers.FileTransferObserver.Invoke(fileStream, new RoomActionEventArgs()
                {
                    RoomType = GenericEnums.RoomType.Send,
                    TransferInfo = new TransferInfo() { FileName = fileName },
                    Identity = senderIdentity
                });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateContactStatus(string senderIdentity, GenericEnums.ContactStatus newStatus)
        {
            try
            {
                // propagate the update to the UI, through the controller
                _controllerHandlers.ContactsObserver.Invoke(this, new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Status,
                    UpdatedContact = new Contact(-1, senderIdentity, newStatus)
                }
                //,null, null
                );
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendRemotingCommand(byte[] commandArgs)
        {
            try
            {
                MemoryStream stream = new MemoryStream(commandArgs);

                BinaryFormatter formatter = new BinaryFormatter();
                RemotingCommandEventArgs record2 = (RemotingCommandEventArgs)formatter.Deserialize(stream);
                // send command to controller handler
                _controllerHandlers.RemotingCommandHandler.Invoke(this, record2);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateFriendlyName(string senderIdentity, string newFriendlyName)
        {
            try
            {
                // propagate the update to the UI, through the controller
                _controllerHandlers.ContactsObserver.Invoke(this, new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Update,
                    UpdatedContact = new Contact(-1, newFriendlyName, senderIdentity)
                });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendWebcamCapture(byte[] capture, DateTime captureTimestamp, string senderIdentity)
        {
            try
            {
                // wait for the room command to finish (might be a stop signal)
                _syncVideoCaptures.WaitOne();
               
                MemoryStream ms = new MemoryStream(capture);
                //read the Bitmap back
                Image bmp = (Bitmap)Bitmap.FromStream(ms);

                _controllerHandlers.VideoCaptureObserver.Invoke(this,
                    new VideoCaptureEventArgs()
                    {
                        Identity = senderIdentity,
                        CapturedImage = bmp,
                        CaptureTimestamp = captureTimestamp
                    });                
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendMicrophoneCapture(byte[] capture, DateTime captureTimestamp, string senderIdentity, double captureLengthInSeconds)
        {
            try
            {
                _syncAudioCaptures.WaitOne();
                _controllerHandlers.AudioCaptureObserver.Invoke(this,
                    new AudioCaptureEventArgs()
                    {
                        Identity = senderIdentity,
                        Capture = capture,
                        CaptureTimestamp = captureTimestamp,
                        CaptureLengthInSeconds = captureLengthInSeconds
                    }
                    //, null, null
                    );
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void WaitRoomButtonAction(string senderIdentity, GenericEnums.RoomType roomType, bool wait)
        {
            try
            {
                _controllerHandlers.WaitRoomActionObserver.Invoke(null,
                    new RoomActionEventArgs()
                    {
                        Identity = senderIdentity,
                        RoomType = roomType,
                        SignalType = wait == true ? GenericEnums.SignalType.Wait : GenericEnums.SignalType.RemoveWait
                    });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SendRoomButtonAction(string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType)
        {
            try
            {
                _syncVideoCaptures.Reset();
                _syncRemotingCaptures.Reset();
                _syncAudioCaptures.Reset();

                _controllerHandlers.RoomButtonObserver.Invoke(this,
                    new RoomActionEventArgs()
                    {
                        RoomType = roomType,
                        Identity = identity,
                        SignalType = signalType
                    });

                _syncRemotingCaptures.Set();
                _syncVideoCaptures.Set();
                _syncAudioCaptures.Set();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void AddContact(string identity, string friendlyName)
        {
            try
            {
                Tools.Instance.Logger.LogInfo("Started added contact: " + identity);

                ContactsEventArgs args = new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Add,
                    UpdatedContact = new Contact(-1, friendlyName, identity)
                };
                _controllerHandlers.ContactsObserver.Invoke(this, args);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void RemoveContact(string identity)
        {
            try
            {
                ContactsEventArgs args = new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Remove,
                    UpdatedContact = new Contact(-1, "", identity)
                };
                _controllerHandlers.ContactsObserver.Invoke(this, args);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public bool Ping()
        {
            return true;
        }

        public bool ConferencePermission(string senderIdentity, GenericEnums.RoomType roomType)
        {
            bool canStart = false;
            try
            {
                RoomActionEventArgs args =
                new RoomActionEventArgs()
                    {
                        Identity = senderIdentity,
                        RoomType = roomType
                    };
                // request permission from the user
                _controllerHandlers.ConferencePermissionObserver.Invoke(this, args);
                canStart = args.HasPermission;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canStart;
        }

        #endregion

        public override void BuildServerBinding(bool isSecured)
        {
            try
            {
                // Add MEX endpoint

                //_binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpsBinding();
                _binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpBinding();

                _binding.MaxBufferPoolSize = 100000000;
                _binding.ReaderQuotas.MaxArrayLength = 100000000;
                _binding.ReaderQuotas.MaxStringContentLength = 100000000;
                _binding.ReaderQuotas.MaxBytesPerRead = 100000000;
                _binding.MaxReceivedMessageSize = 100000000;

                _binding.HostNameComparisonMode = HostNameComparisonMode.WeakWildcard;
                _binding.Security.Mode = SecurityMode.Message;

                if (isSecured == false)
                {
                    _binding.Security.Mode = SecurityMode.None;
                }
                _binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                _binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                _binding.Security.Transport.Realm = string.Empty;

                _binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
                _binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
                _binding.Security.Message.EstablishSecurityContext = false;
                _binding.Security.Message.NegotiateServiceCredential = false;

                _binding.Name = "binding1";

                _binding.OpenTimeout = new TimeSpan(0, 1, 00);
                _binding.CloseTimeout = new TimeSpan(0, 1, 00);
                _binding.ReceiveTimeout = new TimeSpan(0, 1, 30);
                _binding.SendTimeout = new TimeSpan(0, 1, 30);


                // todo : optional - programmatically add global error handler to the WCF
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildUri(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity)
        {
            try
            {
                _httpsAddress = httpsAddress;
                _httpURI = new Uri(httpsAddress.Replace("https", "http"), UriKind.Absolute);
                int httpPort = _httpURI.Port - 1;
                string httpAddress = _httpURI.ToString().Replace(_httpURI.Port.ToString(), httpPort.ToString());
                _httpURI = new Uri(httpAddress, UriKind.Absolute);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildBehavior(ServiceHost svcHost)
        {
            try
            {
                var behavior = svcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.IncludeExceptionDetailInFaults = true;
                behavior.AutomaticSessionShutdown = true;
                behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
                behavior.Name = "MetadataExchangeHttpsBinding_IVideoRoom";

                behavior.AddressFilterMode = AddressFilterMode.Any;

                // Check to see if the service host already has a ServiceMetadataBehavior
                ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                // If not, add one
                if (smb == null)
                    smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = _httpURI;

                smb.HttpsGetEnabled = true;
                Uri httpsURI = new Uri(_httpsAddress);
                smb.HttpsGetUrl = httpsURI;

                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                svcHost.Description.Behaviors.Add(smb);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public WSHttpBinding Binding
        {
            get { return _binding; }
        }
        public Uri HttpURI
        {
            get { return _httpURI; }
        }
    }
}
