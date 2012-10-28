using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    internal static class ClientBuilder
    {
        #region public static methods

        public static MViewerClient BuildWCFClient(ContactEndpoint contactEndpoint)
        {
            X509Certificate2 certificate = new X509Certificate2("c:\\Client.pfx");

            MViewerClient client;

            WSHttpBinding binding = CreateServerBinding();
            string address = "http://" + contactEndpoint.Address + ":" + contactEndpoint.Port.ToString() + "/" + contactEndpoint.Path;
            EndpointAddress endpoint = CreateServerEndpoint(address);

            client = new MViewerClient(binding, endpoint);
            client.ClientCredentials.ClientCertificate.Certificate = certificate;

            ContractDescription contract = ContractDescription.GetContract(typeof(IMViewerService), typeof(MViewerClient));
            client.Endpoint.Contract = contract;
            client.Endpoint.Binding = CreateServerBinding();
            client.Endpoint.Binding.Name = "binding1_IVideoChatRoom";
            
            return client;
        }

        #endregion

        #region private static methods

        static WSHttpBinding CreateServerBinding()
        {
            //WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message, true);
            WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpsBinding();
            //WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpBinding();

            binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);

            binding.MaxBufferPoolSize = 100000000;
            binding.ReaderQuotas.MaxArrayLength = 100000000;
            binding.ReaderQuotas.MaxStringContentLength = 100000000;
            binding.ReaderQuotas.MaxBytesPerRead = 100000000;
            binding.MaxReceivedMessageSize = 100000000;

            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.Security.Mode = SecurityMode.Message;

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = string.Empty;

            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = false;

            binding.Name = "binding1_IVideoChatRoom";

            binding.ReliableSession.Ordered = true;
            binding.ReliableSession.InactivityTimeout = TimeSpan.FromMinutes(10);
            binding.ReliableSession.Enabled = false;

            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.TransactionFlow = false;
            binding.UseDefaultWebProxy = true;
            binding.TextEncoding = Encoding.UTF8;
            binding.MessageEncoding = WSMessageEncoding.Text;

            return binding;
        }

        static EndpointAddress CreateServerEndpoint(string serverAddress)
        {
            Uri uri = new Uri(serverAddress);
            X509Certificate2 serverCert = new X509Certificate2("c:\\server2.cer");
            EndpointIdentity identity = EndpointIdentity.CreateX509CertificateIdentity(serverCert);
            EndpointAddress endpoint = new EndpointAddress(uri, identity);

            return endpoint;
        }

        #endregion
    }
}
