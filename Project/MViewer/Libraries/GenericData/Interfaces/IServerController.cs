using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public interface IServerController
    {
        void StartServer();
        void StopServer();
    }
}
