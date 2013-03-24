using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    internal class ServerBuilder : Builder
    {
        #region private members

        MViewerServer _server = new MViewerServer();
        ServiceHost _svcHost;

        string _httpsAddress;
        ControllerEventHandlers _controllerHandlers;
        string _identity;

        #endregion

        #region c-tor

        public ServerBuilder(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity)
        {
            _httpsAddress = httpsAddress;
            _controllerHandlers = controllerHandlers;
            _identity = identity;
        }

        #endregion

        #region public methods

        public override void BuildCertificate()
        {
            X509Certificate2 severCert = new X509Certificate2("Server.pfx", "", X509KeyStorageFlags.MachineKeySet);
            _svcHost.Credentials.ServiceCertificate.Certificate = severCert; //new X509Certificate2("Server.pfx");
            X509ClientCertificateAuthentication authentication = _svcHost.Credentials.ClientCertificate.Authentication;
            X509Certificate2 clientCert = new X509Certificate2("Client.pfx", "", X509KeyStorageFlags.MachineKeySet);

            // todo: decide if to keep the custome validator
            authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            authentication.CustomCertificateValidator = new GenericDataLayer.CustomCertificateValidator("CN=Mihai-PC", clientCert);
            
            _svcHost.Credentials.ClientCertificate.Certificate = clientCert;
        }

        public override void BuildBinding()
        {
            _server.BuildServerBinding();
            _svcHost.AddServiceEndpoint(typeof(IMViewerService), _server.Binding, "");

        }

        public override void BuildUri()
        {
            _server = new MViewerServer(_controllerHandlers, _identity);
            _server.BuildUri(_httpsAddress, _controllerHandlers, _identity);
            _svcHost = new ServiceHost(_server, _server.HttpURI);
        }

        public override void BuildBehavior()
        {
            _server.BuildBehavior(_svcHost);
        }

        public override object GetResult()
        {
            return _svcHost;
        }

        #endregion

    }
}
