using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GenericDataLayer;
using System.Drawing;
using UIControls;
using Utils;
using System.Data;

namespace MViewer
{
    public class View : IView
    {
        #region private members

        FormMain _formMain; 
        FormActions _formActions;

        Dictionary<Type, object> _observers;
        bool _observersActive;

        IModel _model;

        #endregion

        #region event handlers


 
        #endregion

        #region c-tor

        public View(IModel model)
        {
            _model = model;
            _formMain = new FormMain();
            _formActions = new FormActions();
        }

        #endregion

        #region public methods

        public void BindObservers(bool bind)
        {
            if (bind)
            {
                _observers = new Dictionary<Type, object>();
                _observers.Add(_formMain.IdentityObserver.GetType(), _formMain.IdentityObserver);
                _observers.Add(_formMain.ContactsObserver.GetType(), _formMain.ContactsObserver);
                _observers.Add(_formActions.ActionsObserver.GetType(), _formActions.ActionsObserver);
            }
            else
            {
                _observers = null;
            }
            _observersActive = bind;
        }

        public void NotifyContactsObserver()
        {
            EventHandlers.ContactsEventHandler contactsEventHandler = 
                (EventHandlers.ContactsEventHandler)_observers[typeof(EventHandlers.ContactsEventHandler)];
            contactsEventHandler.Invoke(this, new ContactsEventArgs() 
            { 
                ContactsDV = _model.Contacts ,
                Operation = GenericEnums.ContactsOperation.Load
            });
        }

        public void NotifyIdentityObserver()
        {
            EventHandlers.IdentityEventHandler identityObserver = (EventHandlers.IdentityEventHandler)_observers[typeof(EventHandlers.IdentityEventHandler)];
            identityObserver.Invoke(this, new IdentityEventArgs() 
            {
                FriendlyName = _model.FriendlyName,
                MyIdentity = _model.Identity.MyIdentity
            });
        }

        public void NotifyActionsObserver()
        {
            // todo: use NotifyActionsObserver
            EventHandlers.ActionsEventHandler actionsObserver = (EventHandlers.ActionsEventHandler)_observers[typeof(EventHandlers.ActionsEventHandler)];
            actionsObserver.Invoke(this, null);
        }

        public void ShowMainForm(bool close)
        {
            if (!close)
            {
                ThreadPool.QueueUserWorkItem(OpenActionsForm);

                ThreadPool.QueueUserWorkItem(OpenMainForm);
                //            Application.Run(_formMain);
            }
            else
            {
                _formMain.Close();
                _formMain.Dispose();
            }
        }

        #endregion

        #region private methods

        void OpenMainForm(object identity)
        {
            Application.Run(_formMain);
            _formMain.BringToFront();
        }

        void OpenActionsForm(Object threadContext)
        {
            _formActions.StartPosition = FormStartPosition.Manual;
            // position the Actions form at the right bottom of the screen
            _formActions.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - _formActions.Width, Screen.PrimaryScreen.WorkingArea.Height - _formActions.Height);
            _formActions.ShowDialog();
        }

        #endregion

        #region proprieties


        #endregion
    }
}
