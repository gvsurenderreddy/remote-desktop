using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UIControls;
using Utils;
using GenericDataLayer;
using BusinessLogicLayer;

namespace MViewer
{
    public class Controller : IController
    {
        #region private members

        IView _view;
        IModel _model;

        IClientController _clientController;
        IServerController _serverController;

        #endregion

        #region c-tor

        public Controller()
        {
            // initialize the model
            _model = new Model();
            // initalize the view
            _view = new View(_model);

            _clientController = new ClientController();
            ContactEndpoint myEndpoint = IdentityResolver.ResolveIdentity(_model.Identity.MyIdentity);
            _serverController = new ServerController(myEndpoint);
        }

        #endregion

        #region public event handlers

        public void IdentityUpdated(object sender, IdentityEventArgs e)
        {
            // todo: implement IdentityUpdated
            _model.Identity.UpdateFriendlyName(e.FriendlyName);
        }

        public void ActionTriggered(object sender, FrontEndActionsEventArgs e)
        {
            // todo: perform specific actions when action has been triggered
            
        }

        public Contact PerformContactsOperation(object sender, ContactsEventArgs e)
        {
            Contact contact = null;
            if (e.Operation == GenericEnums.ContactsOperation.Load)
            {
                NotifyContactsObserver();
            }
            else
            {
                contact = _model.PerformContactOperation(e);
            }
            return contact;
        }

        #endregion

        #region public methods

        //public void InitializeWCFClient()
        //{

        //}

        //public void InitializeWCFServer()
        //{

        //}

        public void StartApplication()
        {
            // bind the observers
            _view.BindObservers(true);

            // open main form
            _view.ShowMainForm(false);
            NotifyContactsObserver();

            // todo: ping every single contact in the list and update it's status

            // todo: use manual reset event instead of thread.sleep(0)
            Thread.Sleep(2000);

            _view.NotifyIdentityObserver();

            _serverController.StartServer();

        }

        public void StopApplication()
        {
            // unbind the observers
            _view.BindObservers(false);

            _serverController.StopServer();

            // exit the environment
            Environment.Exit(0);
        }

        public void NotifyContactsObserver()
        {
            //_model.ContactsUpdated();
            _view.NotifyContactsObserver();
        }

        public void NotifyIdentityObserver()
        {
            //_model.ContactsUpdated();
            _view.NotifyIdentityObserver();
        }

        public void NotifyActionsObserver()
        {
            //_model.ContactsUpdated();
            _view.NotifyActionsObserver();
        }

        #endregion

        #region proprieties


        #endregion
    }
}
