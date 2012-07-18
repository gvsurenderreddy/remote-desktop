using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using System.Xml;
using System.Configuration;
using System.ServiceModel.Security;
using VideoChatWCF;

namespace SelfHostedWCF
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri httpURI = new Uri(ConfigurationManager.AppSettings["httpURI"]);
            ServiceHost svcHost = new ServiceHost(typeof(VideoChatRoomService), httpURI);

            var behavior = svcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behavior.InstanceContextMode = InstanceContextMode.Single;
            behavior.IncludeExceptionDetailInFaults = true;
            behavior.AutomaticSessionShutdown = true;
            behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
            behavior.Name = "MetadataExchangeHttpsBinding_IVideoChatRoom";

            try
            {                
                // Check to see if the service host already has a ServiceMetadataBehavior
                ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                // If not, add one
                if (smb == null)
                    smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;

                smb.HttpsGetEnabled = true;
                Uri httpsURI = new Uri(ConfigurationManager.AppSettings["httpsURI"]);
                smb.HttpsGetUrl = httpsURI;

                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                svcHost.Description.Behaviors.Add(smb);
                // Add MEX endpoint

                WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpsBinding();
                //WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpBinding();
            
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
                
                binding.Name = "binding1";

                //ServiceEndpoint endpoint = new ServiceEndpoint(contract, binding, new EndpointAddress(myuri));
                //endpoint.Binding = binding;
                //endpoint.Name = "WCFEndpoint";

                svcHost.Credentials.ServiceCertificate.Certificate = new X509Certificate2("c:\\Server.pfx");
                X509ClientCertificateAuthentication authentication = svcHost.Credentials.ClientCertificate.Authentication;
                authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
                authentication.CustomCertificateValidator = new CustomCertificateValidator("CN=Mihai-PC", new X509Certificate2("c:\\Client.pfx"));
                svcHost.Credentials.ClientCertificate.Certificate = new X509Certificate2("c:\\Client.pfx");

                // Add application endpoint
                svcHost.AddServiceEndpoint(typeof(IVideoChatRoom), binding, "");

                // Open the service host to accept incoming calls
                svcHost.Open();

                string addr = svcHost.Description.Endpoints[0].ListenUri.AbsoluteUri;
                Console.WriteLine("Listening at: ");
                Console.WriteLine(addr);

                // The service can now be accessed.
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();

                // Close the ServiceHostBase to shutdown the service.
                svcHost.Close();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                Console.Read();
            }
        }
    }
}
