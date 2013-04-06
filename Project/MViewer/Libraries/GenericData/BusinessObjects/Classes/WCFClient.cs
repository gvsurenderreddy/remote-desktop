using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using Utils;
////using System.Threading.Tasks;

namespace GenericObjects
{
    public class WCFClient : Product
    {
        MViewerClient _client;
        WSHttpBinding _binding;
        EndpointAddress _endpoint;

        public MViewerClient Client
        {
            get { return _client; }
        }

        public override void BuildCertificate()
        {
            try
            {
                X509Certificate2 certificate = new X509Certificate2("Client.pfx", "", X509KeyStorageFlags.MachineKeySet);
                _client.ClientCredentials.ClientCertificate.Certificate = certificate;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildClientBinding(ContactEndpoint contractEndpoint)
        {
            try
            {
                string address = "http://" + contractEndpoint.Address + ":" + contractEndpoint.Port.ToString() + "/" + contractEndpoint.Path;
                _endpoint = CreateServerEndpoint(address);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildContract()
        {
            try
            {
                ContractDescription contract = ContractDescription.GetContract(typeof(IMViewerService), typeof(MViewerClient));
                CreateServerBinding();
                _client = new MViewerClient(_binding, _endpoint);
                _client.Endpoint.Binding = _binding;
                _client.Endpoint.Binding.Name = "binding1_IVideoRoom";
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        //not needed
        public override void BuildUri(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity)
        { }
        public override void BuildBehavior(System.ServiceModel.ServiceHost svcHost)
        { }
        public override void BuildServerBinding() { }

        #region private methods

        void CreateServerBinding()
        {
            try
            {
                _binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpsBinding();
                _binding.CloseTimeout = new TimeSpan(0, 1, 0);
                _binding.OpenTimeout = new TimeSpan(0, 0, 30);
                _binding.ReceiveTimeout = new TimeSpan(0, 0, 30);
                _binding.SendTimeout = new TimeSpan(0, 0, 30);

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

                _binding.Name = "binding1_IVideoRoom";

                _binding.ReliableSession.Ordered = true;
                _binding.ReliableSession.InactivityTimeout = TimeSpan.FromMinutes(10);
                _binding.ReliableSession.Enabled = false;

                _binding.AllowCookies = false;
                _binding.BypassProxyOnLocal = false;
                _binding.TransactionFlow = false;
                _binding.UseDefaultWebProxy = true;
                _binding.TextEncoding = Encoding.UTF8;
                _binding.MessageEncoding = WSMessageEncoding.Text;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        EndpointAddress CreateServerEndpoint(string serverAddress)
        {
            Uri uri = new Uri(serverAddress);
            X509Certificate2 serverCert = new X509Certificate2("server2.cer");
            EndpointIdentity identity = EndpointIdentity.CreateX509CertificateIdentity(serverCert);
            EndpointAddress endpoint = new EndpointAddress(uri, identity);

            return endpoint;
        }

        #endregion
    }
}
