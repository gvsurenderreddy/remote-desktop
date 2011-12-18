using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Common
{
    public interface IServerModel
    {
        #region proprieties

        bool IsListening { get; }
        string ChannelName { get; }
        int Port { get; }
        string Host { get; }
        int ConnectedClients { get; }
        ObservableCollection<ConnectedClient> Clients { get; }

        #endregion

        #region methods

        int AddClient(string ip, string hostname);
        void RemoveClient(int id, bool checkStatus);
        void RemoveAllClients();
        void StartServer();
        void StopServer();
        void UpdateDesktop();
        void UpdateMouseCursor();
        void AddObserver(IServerView serverView);
        void RemoveObserver(IServerView serverView);
        void NotifyObservers();
        bool CheckClientStatus(int id);

        #endregion
    }
}
