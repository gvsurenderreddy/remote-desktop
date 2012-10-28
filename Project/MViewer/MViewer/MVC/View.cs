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
using MViewer;
using BusinessLogicLayer;

namespace MViewer
{
    public class View : IView
    {
        #region private members

        FormMain _formMain;
        FormActions _formActions; 
        FormMyWebcam _formWebCapture;

        IDictionary<Type, object> _observers;
        bool _observersActive;

        IRoomManager _roomManager = new RoomManager();

        IModel _model;

        #endregion

        #region event handlers


 
        #endregion

        #region c-tor

        public View(IModel model)
        {
            _model = model;
            _formMain = new FormMain();
            _formActions = new FormActions(new EventHandler(this.PerformRoomAction));
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

        public void ShowMyWebcamForm(RoomActionEventArgs e)
        {
            if (_formWebCapture == null)
            {    
                // open my webcam form if no video chat was previously started
                Thread t = new Thread(delegate()
                {
                    _formWebCapture = new FormMyWebcam(e);
                    _formWebCapture.ShowDialog();
                    //Thread.Sleep(Timeout.Infinite);
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
        }

        //public IntPtr ShowRoomForm(object sender, EventArgs e) // GenericEnums.FrontEndActionType roomType, string friendlyName, string identity)
        //{
        //    // todo: implement ShowRoomForm
        //    RoomActionEventArgs args = (RoomActionEventArgs)e;
        //    IRoom room = null;
        //    IntPtr handle = IntPtr.Zero;
        //    switch(args.ActionType)
        //    {
        //        case GenericEnums.RoomActionType.Audio:

        //            break;
        //        case GenericEnums.RoomActionType.Remoting:

        //            break;
        //        case GenericEnums.RoomActionType.Video:
        //            room = new FormVideoRoom(ref handle);
        //            room.SetPartnerName(args.FriendlyName);
        //            OpenRoomForm(room);
        //            break;
        //    }
        //    _roomManager.AddRoom(args.Identity, room);
        //    return handle;
        //}
        
        public void NotifyContactsObserver()
        {
            EventHandlers.ContactsEventHandler contactsEventHandler = 
                (EventHandlers.ContactsEventHandler)_observers[typeof(EventHandlers.ContactsEventHandler)];
            contactsEventHandler.Invoke(this, new ContactsEventArgs() 
            { 
                ContactsDV = _model.Contacts,
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

        public void PerformRoomAction(object sender, EventArgs e)
        {
            string activeRoom = _roomManager.ActiveRoom;
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            if (string.IsNullOrEmpty(activeRoom))
            {
                // this is the first opened room
                // retrieve the selected contact from the Contacts list
                KeyValuePair<string, string> contact = _formMain.GetSelectedContact();
                args.Identity = contact.Key;
                args.FriendlyName = contact.Value;
               
            }
            Program.Controller.PerformRoomAction(sender, args);
        }

        public void UpdateWebcapture(Image image)
        {
            _formWebCapture.SetPicture(image);
        }

        #endregion

        #region private methods

        //void OpenRoomForm(Object threadContext)
        //{
        //    ((Form)threadContext).Show();
        //}

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

        public WebcamCapture GetWebcaptureControl
        {
            get
            {
                return _formWebCapture.CaptureControl;
            }
        }

        public IRoomManager RoomManager
        {
            get { return _roomManager; }
        }

        #endregion
    }
}
