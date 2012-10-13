using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public interface IClientController
    {
        void StartClient(string identity);
        void RemoveClient(string identity);
        void AddClient(string identity);
        MViewerClient GetClient(string identity);
        bool IsContactOnline(string identity);
    }
}
