using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using GenericObjects;
using Communicator;

namespace BusinessLogicLayer
{
    internal class ClientBuilder : Builder
    {
        #region private members

        private WCFClient _client;
        private ContactEndpoint _endpoint;
        bool _isSecured;

        #endregion

        #region c-tor

        public ClientBuilder(ContactEndpoint serverEndpoint, bool isSecured)
        {
            _endpoint = serverEndpoint;
            _client = new WCFClient(isSecured);
            _isSecured = isSecured;
        }

        #endregion

        #region public methods

        public override object GetResult()
        {
            return _client.Client;
        }

        public override void BuildCertificate()
        {
            _client.BuildCertificate();
        }

        public override void BuildBinding()
        {
            _client.BuildClientBinding(_endpoint);
        }

        public override void BuildContract()
        {
            _client.BuildContract();
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
