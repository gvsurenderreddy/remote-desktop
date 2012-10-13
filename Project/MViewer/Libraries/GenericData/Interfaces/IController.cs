using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GenericDataLayer
{
    public interface IController
    {
        void StartApplication();
        void StopApplication();
        void NotifyContactsObserver();
        //void InitializeWCFClient();
        //void InitializeWCFServer();
        void IdentityUpdated(object sender, IdentityEventArgs e);
        void ActionTriggered(object sender, FrontEndActionsEventArgs e);
        Contact PerformContactsOperation(object sender, ContactsEventArgs e);
    }
}
