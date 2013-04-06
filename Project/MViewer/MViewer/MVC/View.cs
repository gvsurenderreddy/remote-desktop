using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GenericObjects;
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
        Thread _threadWebcaptureForm;

        IDictionary<Type, object> _observers;
        bool _observersActive;

        IRoomManager _roomManager;


        IModel _model;

        #endregion

        #region event handlers



        #endregion

        #region c-tor

        public View(IModel model)
        {
            _model = model;
            _formMain = new FormMain();
            _roomManager = new RoomManager(_formMain);
            _formActions = new FormActions(new EventHandler(this.RoomButtonAction));
        }

        #endregion

        #region public methods

        public void ChangeLanguage(string language)
        {
            _formActions.ChangeLanguage(language);
            _formMain.ChangeLanguage(language);
            _roomManager.ChangeLanguage(language);
        }

        public void SetResultText(string text)
        {
            _formMain.SetResultText(text);
        }

        public void SetFormMainBackground(string filePath)
        {
            _formMain.SetFormMainBackground(filePath);
        }

        public bool RequestTransferPermission(string identity, string fileName, long fileSize)
        {
            bool canSend = false;

            string friendlyName = _model.GetContact(identity).FriendlyName;
            string fileSize2 = Tools.Instance.ImageConverter.GetSize(fileSize);

            DialogResult dialogResult = MessageBox.Show(
                string.Format("{0} is sending you the file {1} of {2} long. Permit transfer?",
                friendlyName, fileName, fileSize2),
                "Transfer confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                canSend = true;
            }

            return canSend;
        }

        public void ResetLabels(GenericEnums.RoomType roomType)
        {
            // call the labels update for each room type
            _formActions.UpdateLabels(true, true, roomType);
            if (roomType == GenericEnums.RoomType.Video)
            {
                // reset the audio labels also
                _formActions.UpdateLabels(true, true, GenericEnums.RoomType.Audio);
            }
        }

        public void UpdateLabels(string identity, GenericEnums.RoomType roomType)
        {
            if (string.IsNullOrEmpty(identity))
            {
                // call the labels update for each room type
                _formActions.UpdateLabels(true, true, GenericEnums.RoomType.Undefined);
            }
            else
            {
                PeerStates peers = _model.SessionManager.GetPeerStatus(identity);
                GenericEnums.SessionState sessionState = _model.SessionManager.GetSessionState(identity, roomType);
                bool start = sessionState == GenericEnums.SessionState.Opened || sessionState == GenericEnums.SessionState.Paused || sessionState == GenericEnums.SessionState.Pending ? false : true;
                bool pause = sessionState == GenericEnums.SessionState.Paused ? false : true;

                // call the labels update for each room type
                _formActions.UpdateLabels(start, pause, roomType);
            }
        }

        public bool IsRoomActivated(string identity, GenericEnums.RoomType roomType)
        {
            return _roomManager.IsRoomActivated(identity, roomType);
        }

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

        //public void WebCaptureClosing(object sender, EventArgs args)
        //{
        //    //_threadWebcaptureForm = null;
        //    //_formWebCapture = null;
        //    this.WebcaptureClosed = false;
        //}

        public void ShowMyWebcamForm(bool show)
        {
            if (show)
            {
                if (_formWebCapture != null && this.VideoCaptureClosed == true)
                {
                    _formWebCapture.StartCapturing();
                }
                else
                {
                    if (this._threadWebcaptureForm == null) // first time when the video  is starting
                    {
                        // open my webcam form if no video  was previously started
                        _threadWebcaptureForm = new Thread(delegate()
                        {
                            _formWebCapture = new FormMyWebcam();
                            _formWebCapture.ShowDialog();
                        });
                        _threadWebcaptureForm.IsBackground = true;
                        _threadWebcaptureForm.SetApartmentState(ApartmentState.STA);
                        _threadWebcaptureForm.Start();
                    }
                }
            }
            else
            {
                if (_formWebCapture != null)
                {
                    _formWebCapture.StopCapturing();
                }
            }
        }

        public void FocusActionsForm()
        {
            _formActions.Invoke(new MethodInvoker
                       (
                      delegate
                      {
                          _formActions.Activate();
                      }
                       )
                       );
        }

        public void NotifyContactsObserver()
        {
            if (_observers != null)
            {
                Delegates.ContactsEventHandler contactsEventHandler =
                    (Delegates.ContactsEventHandler)_observers[typeof(Delegates.ContactsEventHandler)];
                contactsEventHandler.Invoke(this, new ContactsEventArgs()
                {
                    ContactsDV = _model.Contacts,
                    Operation = GenericEnums.ContactsOperation.Load
                });
            }
        }

        public void NotifyIdentityObserver()
        {
            Delegates.IdentityEventHandler identityObserver = (Delegates.IdentityEventHandler)_observers[typeof(Delegates.IdentityEventHandler)];
            identityObserver.Invoke(this, new IdentityEventArgs()
            {
                FriendlyName = _model.FriendlyName,
                MyIdentity = _model.Identity.MyIdentity
            });
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

        public void RoomButtonAction(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            if (args.RoomType == GenericEnums.RoomType.Video && _formWebCapture != null)
            {
                // put the video transfers on hold
                _formWebCapture.WaitRoomButtonAction(true);
            }

            if (args.RoomType == GenericEnums.RoomType.Remoting)
            {
                // todo: put the remoting transfers on hold

            }

            if (args.RoomType == GenericEnums.RoomType.Audio)
            {
                // todo: put the audio transfers on hold

            }

            string activeRoom = _roomManager.ActiveRoom;
            if (string.IsNullOrEmpty(activeRoom))
            {
                // this is the first opened room
                // retrieve the selected contact from the Contacts list
                KeyValuePair<string, string> contact = _formMain.GetSelectedContact();
                if (!string.IsNullOrEmpty(contact.Key) && !string.IsNullOrEmpty(contact.Value))
                {
                    args.Identity = contact.Key;
                }
                else
                {
                    args = null;
                }
            }
            else
            {
                // perform specified action against the active  room
                args.Identity = activeRoom;
            }
            if (args != null) // check if there is a selected contact or active  room
            {
                Program.Controller.OnRoomButtonActionTriggered(sender, args);
            }

            if (args.RoomType == GenericEnums.RoomType.Video && _formWebCapture != null)
            {
                // release video lock
                _formWebCapture.WaitRoomButtonAction(false);
            }

            // todo: release audio lock


            // todo: release remoting lock

        }

        public void UpdateWebcapture(Image image)
        {
            if (_formWebCapture != null)
            {
                _formWebCapture.SetPicture(image);
            }
        }

        public bool ExitConfirmation()
        {
            bool canExit = true;

            bool videoRoomsActive = RoomManager.RoomsLeft(GenericEnums.RoomType.Video);
            bool remotingRoomsActive = RoomManager.RoomsLeft(GenericEnums.RoomType.Remoting);
            bool audioRoomsActive = RoomManager.RoomsLeft(GenericEnums.RoomType.Audio);

            if (videoRoomsActive || remotingRoomsActive || audioRoomsActive)
            {
                _formMain.Invoke(new MethodInvoker
                        (
                       delegate
                       {
                           DialogResult result = MessageBox.Show(_formMain,
                               "Close all active rooms?",
                               "Exit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                           if (result != DialogResult.Yes)
                           {
                               canExit = false;
                           }
                       }
                        )
                        );
            }

            return canExit;
        }

        public void WaitRoomButtonAction(bool wait)
        {
            if (_formWebCapture != null)
            {
                _formWebCapture.WaitRoomButtonAction(wait);
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
            Thread t = new Thread(delegate()
                        {
                            _formActions.StartPosition = FormStartPosition.Manual;
                            // position the Actions form at the right bottom of the screen
                            _formActions.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - _formActions.Width, Screen.PrimaryScreen.WorkingArea.Height - _formActions.Height);
                            _formActions.ShowDialog();
                        });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        #endregion

        #region proprieties

        public IRoomManager RoomManager
        {
            get { return _roomManager; }
        }

        public bool VideoCaptureClosed
        {
            get
            {
                bool isClosed = false;
                if (this._formWebCapture != null)
                {
                    isClosed = this._formWebCapture.WebcaptureClosed;
                }
                return isClosed;
            }
        }

        #endregion
    }
}
