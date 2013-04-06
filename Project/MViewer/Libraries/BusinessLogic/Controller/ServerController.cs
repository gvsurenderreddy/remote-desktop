using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using Utils;

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
            try
            {
                if (endpoint.Port != 0)
                {
                    _address = "https://" + endpoint.Address + ":" + (endpoint.Port + 1).ToString() + endpoint.Path;
                }
                else
                {
                    _address = "https://" + endpoint.Address + endpoint.Path;
                }
                Builder serverBuilder = new ServerBuilder(_address, handlers, identity);
                Director.Instance.Construct(serverBuilder);
                _server = (ServiceHost)serverBuilder.GetResult();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void StartServer()
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    _server.Open();
                    string addr = _server.Description.Endpoints[0].ListenUri.AbsoluteUri;
                    Console.WriteLine("Listening at: ");
                    Console.WriteLine(addr);

                    Thread.Sleep(Timeout.Infinite);
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void StopServer()
        {
            try
            {
                _server.Close();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
