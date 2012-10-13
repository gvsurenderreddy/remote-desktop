using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.ServiceModel;

namespace BusinessLogicLayer
{
    public class ServerController : IServerController
    {
        #region private members

        string _address;
        ServiceHost _server;

        #endregion

        #region c-tor

        public ServerController(ContactEndpoint endpoint)
        {
            _address = "https://" + endpoint.Address + ":" + endpoint.Port.ToString() + endpoint.Path;
            _server = ServerBuilder.BuildWCFServer(_address);
        }

        #endregion

        #region private methods


        #endregion

        #region public methods

        // todo: use async operations for start/stop if necessary

        public void StartServer()
        {
            _server.Open();

        }

        public void StopServer()
        {
            _server.Close();
        }

        #endregion

        #region proprieties


        #endregion
    }
}
