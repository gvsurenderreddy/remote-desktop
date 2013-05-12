using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using Utils;
using GenericObjects;
//

namespace Communicator
{
    public class WCFClient : Product
    {
        MViewerClient _client;
        WSHttpBinding _binding;
        EndpointAddress _endpoint;
        bool _isSecured;

        public WCFClient(bool isSecured)
        {
            _isSecured = isSecured;
        }

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

                _client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;

                _client.ClientCredentials.ServiceCertificate.Authentication.CustomCertificateValidator =
                    new ClientCertificateValidator("CN=Mihai-PC");
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildClientBinding(ContactEndpointBase contractEndpoint)
        {
            try
            {
                ContactEndpoint endpoint = (ContactEndpoint)contractEndpoint;
                string address = "http://" + endpoint.Address + ":" + endpoint.Port.ToString() + "/" + endpoint.Path;
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
                CreateServerBinding(_isSecured);
                _client = new MViewerClient(_binding, _endpoint);
                _client.Endpoint.Binding = _binding;
                _client.Endpoint.Binding.Name = "binding1_IVideoRoom";
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #region private methods

        void CreateServerBinding(bool isSecured)
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
                _binding.Security.Message.EstablishSecurityContext = true;
                _binding.Security.Message.NegotiateServiceCredential = true;

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
