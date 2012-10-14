using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.ServiceModel;
using System.Threading;

namespace BusinessLogicLayer
{
    public class ServerController : IServerController
    {
        #region private members

        string _address;
        ServiceHost _server;

        #endregion

        #region c-tor

        public ServerController(ContactEndpoint endpoint, string identity, ControllerEventHandlers handlers)
        {
            if (endpoint.Port != 0)
            {
                _address = "https://" + endpoint.Address + ":" + (endpoint.Port + 1).ToString() + endpoint.Path;
            }
            else
            {
                _address = "https://" + endpoint.Address + endpoint.Path;
            }
            _server = ServerBuilder.BuildWCFServer(_address, handlers, identity);
        }

        #endregion

        #region private methods


        #endregion

        #region public methods

        // todo: use async operations for start/stop if necessary

        public void StartServer()
        {
            Thread t = new Thread(delegate()
            {
                _server.Open();
                string addr = _server.Description.Endpoints[0].ListenUri.AbsoluteUri;
                Console.WriteLine("Listening at: ");
                Console.WriteLine(addr); 
                
                Thread.Sleep(Timeout.Infinite);

            });
            t.Start();
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
