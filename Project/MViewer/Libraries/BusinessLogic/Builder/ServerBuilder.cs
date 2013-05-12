using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using GenericObjects;
using Utils;
using Communicator;

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
        bool _isSecured;

        #endregion

        #region c-tor

        public ServerBuilder(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity, bool isSecured)
        {
            try
            {
                _httpsAddress = httpsAddress;
                _controllerHandlers = controllerHandlers;
                _identity = identity;
                _isSecured = isSecured;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public override void BuildCertificate()
        {
            try
            {
                X509Certificate2 severCert = new X509Certificate2("Server.pfx", "", X509KeyStorageFlags.MachineKeySet);
                _svcHost.Credentials.ServiceCertificate.Certificate = severCert; 
                X509ClientCertificateAuthentication authentication = _svcHost.Credentials.ClientCertificate.Authentication;
                X509Certificate2 clientCert = new X509Certificate2("Client.pfx", "", X509KeyStorageFlags.MachineKeySet);

                if (_isSecured)
                {
                    authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
                    authentication.CustomCertificateValidator = new ServerCertificateValidator(severCert, "CN=Mihai-PC", clientCert);
                }
                else
                {
                    authentication.CertificateValidationMode = X509CertificateValidationMode.None;
                }
                _svcHost.Credentials.ClientCertificate.Certificate = clientCert;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildBinding()
        {
            try
            {
                _server.BuildServerBinding(_isSecured);
                _svcHost.AddServiceEndpoint(typeof(IMViewerService), _server.Binding, _server.HttpURI);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void BuildUri()
        {
            try
            {
                _server = new MViewerServer(_controllerHandlers, _identity);
                _server.BuildUri(_httpsAddress, _controllerHandlers, _identity);
                _svcHost = new ServiceHost(_server, _server.HttpURI);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
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

        #region proprieties

        public override bool IsSecured
        {
            get { return _isSecured; }
        }

        #endregion

    }
}
