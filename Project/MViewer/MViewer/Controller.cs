using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UIControls;

namespace MViewer
{
    public class Controller
    {
        #region private members

        View _view;
        Model _model;
  
        #endregion

        #region c-tor

        public Controller()
        {
            // todo: initialize the model
            _model = new Model();
            // todo: initalize the view
            _view = new View(_model);
        }

        #endregion

        #region public methods

        public void StartApplication()
        {
            // bind the observers
            _view.BindObservers();

            // open main form
            _view.ShowMainForm();

            // todo: use manual reset event instead of thread.sleep(0)
            Thread.Sleep(2000);

            _view.NotifyIdentityObserver(new IdentityEventArgs()
                {
                    MyIdentity = _model.MyIdentity,
                    FriendlyName = _model.FriendlyName
                });
        }

        public void StopApplication()
        {
            // todo: unbind the observers

            // todo: close main form

            // todo: exit the environment
        }

        public void UpdateIdentity(string newFriendlyName)
        {
            //_model.
        }

        #endregion

        #region proprieties


        #endregion
    }
}
