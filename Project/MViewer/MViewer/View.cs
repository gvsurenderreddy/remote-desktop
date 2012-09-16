using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GenericData;
using System.Drawing;
using UIControls;

namespace MViewer
{
    public class View
    {
        #region private members

        FormMain _formMain; 
        FormActions _formActions;

        Dictionary<Type, object> _observers;

        Model _model;

        #endregion

        #region event handlers

 
        #endregion

        #region c-tor

        public View(Model model)
        {
            _model = model;
            _formMain = new FormMain(_model.IdentityUpdated);
            _formMain.InitializeRepository(model);
        }

        #endregion

        #region public methods

        public void BindObservers()
        {
            _observers = new Dictionary<Type, object>();
            _observers.Add(_formMain.IdentityObserver.GetType(), _formMain.IdentityObserver);
        }

        public void NotifyIdentityObserver(IdentityEventArgs args)
        {
            EventHandlers.IdentityEventHandler identityObserver = (EventHandlers.IdentityEventHandler)_observers[typeof(EventHandlers.IdentityEventHandler)];
            identityObserver.Invoke(this, new IdentityEventArgs()
            {
                FriendlyName = args.FriendlyName,
                MyIdentity = args.MyIdentity
            });
        }

        public void ShowMainForm()
        {
            ThreadPool.QueueUserWorkItem(OpenActionsForm);

            ThreadPool.QueueUserWorkItem(OpenMainForm);
//            Application.Run(_formMain);

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
            // todo: open the Actions form
            _formActions = new FormActions();
            _formActions.StartPosition = FormStartPosition.Manual;
            // todo: position the Actions form at the bottom of the screen
            _formActions.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - _formActions.Width, Screen.PrimaryScreen.WorkingArea.Height - _formActions.Height);
            _formActions.ShowDialog();
        }

        #endregion

        #region proprieties


        #endregion
    }
}
