using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;
using System.Threading;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using Utils;
using System.Drawing;
using System.Drawing.Imaging;
using GenericDataLayer;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace GenericDataLayer
{
    public class MViewerServer : Product, IMViewerService
    {
        #region private members

        WSHttpBinding _binding;

        public WSHttpBinding Binding
        {
            get { return _binding; }
        }

        Uri _httpURI;
        string _httpsAddress;

        public Uri HttpURI
        {
            get { return _httpURI; }
        }

        //Computer computer = new Computer();
        ControllerEventHandlers _controllerHandlers;
        string _identity;
        ManualResetEvent _syncVideoCaptures = new ManualResetEvent(true);
        ManualResetEvent _syncRemotingCaptures = new ManualResetEvent(true);
        readonly object _syncAudioCaptures = new object();
        
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
            _syncRemotingCaptures.WaitOne();

            _controllerHandlers.RemotingCaptureObserver.Invoke(this,
                new RemotingCaptureEventArgs()
                {
                    Identity = senderIdentity,
                    ScreenCapture = screenCapture,
                    MouseCapture = mouseCapture
                });
        }

        public bool SendingPermission(string senderIdentity, string fileName, long fileSize)
        {
            bool canSend = false;

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
            return canSend;
        }

        public void SendFile(byte[] fileStream, string fileName)
        {
            // SendFile
            _controllerHandlers.FileTransferObserver.Invoke(fileStream, new RoomActionEventArgs()
            {
                RoomType = GenericEnums.RoomType.Send,
                TransferInfo = new TransferInfo() { FileName = fileName }
            });
        }

        public void UpdateContactStatus(string senderIdentity, GenericEnums.ContactStatus newStatus)
        {
            // propagate the update to the UI, through the controller
            _controllerHandlers.ContactsObserver.Invoke(this, new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Status,
                UpdatedContact = new Contact(-1, senderIdentity, newStatus)
            });
        }

        public void SendRemotingCommand(RemotingCommandEventArgs commandArgs)
        {
            // send command to controller handler
            _controllerHandlers.RemotingCommandHandler.Invoke(this, commandArgs);
        }

        public void UpdateFriendlyName(string senderIdentity, string newFriendlyName)
        {
            // propagate the update to the UI, through the controller
            _controllerHandlers.ContactsObserver.Invoke(this, new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Update,
                UpdatedContact = new Contact(-1, newFriendlyName, senderIdentity)
            });
        }

        public void SendWebcamCapture(byte[] capture, string senderIdentity)
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
                        CapturedImage = bmp
                    });

                GC.Collect();
                
            }
            catch (Exception)
            {

            }
        }

        public void SendMicrophoneCapture(byte[] capture)
        {
            lock (_syncAudioCaptures)
            {
                Computer computer = new Computer();
                computer.Audio.Play(capture, AudioPlayMode.Background);
            }
        }

        public void WaitRoomButtonAction(string senderIdentity, GenericEnums.RoomType roomType, bool wait)
        {
            _controllerHandlers.WaitRoomActionObserver.Invoke(null,
                new RoomActionEventArgs()
                {
                    Identity = senderIdentity,
                    RoomType = roomType,
                    SignalType = wait == true ? GenericEnums.SignalType.Wait: GenericEnums.SignalType.RemoveWait
                });
        }

        public void SendRoomButtonAction(string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType)
        {
            _syncVideoCaptures.Reset();
            _syncRemotingCaptures.Reset();

            _controllerHandlers.RoomButtonObserver.Invoke(this,
                new RoomActionEventArgs()
                {
                    RoomType = roomType,
                    Identity = identity,
                    SignalType = signalType
                });

            _syncRemotingCaptures.Set();
            _syncVideoCaptures.Set();
        }

        public void AddContact(string identity, string friendlyName)
        {
            ContactsEventArgs args = new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Add,
                UpdatedContact = new Contact(-1, friendlyName, identity)
            };
            _controllerHandlers.ContactsObserver.Invoke(this, args);
        }

        public void RemoveContact(string identity)
        {
            ContactsEventArgs args = new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Remove,
                UpdatedContact = new Contact(-1, "", identity)
            };
            _controllerHandlers.ContactsObserver.Invoke(this, args);
        }

        public bool Ping()
        {
            return true;
        }

        #endregion

        public override void BuildServerBinding()
        {
            // Add MEX endpoint

            _binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpsBinding();
            //WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpBinding();

            _binding.MaxBufferPoolSize = 100000000;
            _binding.ReaderQuotas.MaxArrayLength = 100000000;
            _binding.ReaderQuotas.MaxStringContentLength = 100000000;
            _binding.ReaderQuotas.MaxBytesPerRead = 100000000;
            _binding.MaxReceivedMessageSize = 100000000;

            _binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            _binding.Security.Mode = SecurityMode.Message;

            _binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            _binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            _binding.Security.Transport.Realm = string.Empty;

            _binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            _binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
            _binding.Security.Message.EstablishSecurityContext = false;
            _binding.Security.Message.NegotiateServiceCredential = false;

            _binding.Name = "binding1";

            // todo : programmatically add global error handler to the WCF 


            // Add application endpoint

        }

        public override void BuildUri(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity)
        {
            _httpsAddress = httpsAddress;
            _httpURI = new Uri(httpsAddress.Replace("https", "http"), UriKind.Absolute);
            int httpPort = _httpURI.Port - 1;
            string httpAddress = _httpURI.ToString().Replace(_httpURI.Port.ToString(), httpPort.ToString());
            _httpURI = new Uri(httpAddress, UriKind.Absolute);

        }

        public override void BuildBehavior(ServiceHost svcHost)
        {
            var behavior = svcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behavior.InstanceContextMode = InstanceContextMode.Single;
            behavior.IncludeExceptionDetailInFaults = true;
            behavior.AutomaticSessionShutdown = true;
            behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
            behavior.Name = "MetadataExchangeHttpsBinding_IVideoChatRoom";

            behavior.AddressFilterMode = AddressFilterMode.Any;

            // Check to see if the service host already has a ServiceMetadataBehavior
            ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            // If not, add one
            if (smb == null)
                smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;

            smb.HttpsGetEnabled = true;
            Uri httpsURI = new Uri(_httpsAddress);
            smb.HttpsGetUrl = httpsURI;

            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            svcHost.Description.Behaviors.Add(smb);
        }

        // not needed
        public override void BuildCertificate(){}
        public override void BuildContract() { }
        public override void BuildClientBinding(ContactEndpoint contractEndpoint) { }
    }
}
