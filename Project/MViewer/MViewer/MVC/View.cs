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

        #region c-tor

        public View(IModel model)
        {
            try
            {
                _model = model;
                _formMain = new FormMain();
                _roomManager = new RoomManager(_formMain);
                _formActions = new FormActions(new EventHandler(this.RoomButtonAction));
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// method used to change the language for the UI controls and forms
        /// </summary>
        /// <param name="language"></param>
        public void ChangeLanguage(string language)
        {
            try
            {
                _formActions.ChangeLanguage(language);
                _formMain.ChangeLanguage(language);
                _roomManager.ChangeLanguage(language);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to provide feedback for the user
        /// </summary>
        /// <param name="text"></param>
        public void SetMessageText(string text)
        {
            try
            {
                _formMain.SetMessageText(text);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to update the main form background image
        /// </summary>
        /// <param name="filePath"></param>
        public void SetFormMainBackgroundImage(string filePath)
        {
            _formMain.SetFormMainBackground(filePath);
        }

        /// <summary>
        /// method used to provide file transfer permission response to partner enquiry
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public bool RequestTransferPermission(string identity, string fileName, long fileSize)
        {
            bool canSend = false;
            try
            {
                string friendlyName = ((Contact)_model.GetContact(identity)).FriendlyName;
                string fileSize2 = Tools.Instance.GenericMethods.GetSize(fileSize);

                DialogResult dialogResult = MessageBox.Show(
                    string.Format("{0} is sending you the file {1} of {2} long. Permit transfer?",
                    friendlyName, fileName, fileSize2),
                    "Transfer confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    canSend = true;
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canSend;
        }

        /// <summary>
        /// method used to reset the actions controls labels
        /// </summary>
        /// <param name="roomType"></param>
        public void ResetLabels(GenericEnums.RoomType roomType)
        {
            try
            {
                // call the labels update for each room type
                _formActions.UpdateLabels(true, true, roomType);
                if (roomType == GenericEnums.RoomType.Video)
                {
                    // reset the audio labels also
                    _formActions.UpdateLabels(true, true, GenericEnums.RoomType.Audio);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to update the actions controls labels based on partner conference status
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="roomType"></param>
        public void UpdateLabels(string identity, GenericEnums.RoomType roomType)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to find out if specific confrence room is active
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="roomType"></param>
        /// <returns></returns>
        public bool IsRoomActivated(string identity, GenericEnums.RoomType roomType)
        {
            return _roomManager.IsRoomActivated(identity, roomType);
        }

        /// <summary>
        /// method used to bind UI observers
        /// </summary>
        /// <param name="bind"></param>
        public void BindObservers(bool bind)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to show/hide the webcam form
        /// </summary>
        /// <param name="show"></param>
        public void ShowMyWebcamForm(bool show)
        {
            try
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
                                _formWebCapture = new FormMyWebcam(SystemConfiguration.Instance.PresenterSettings.VideoTimerInterval);
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: remove the focus actions method if not necessary
        /// <summary>
        /// method used to set focus to the actions form
        /// </summary>
        public void FocusActionsForm()
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to trigger contacts re-load
        /// </summary>
        public void NotifyContactsObserver()
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to trigger identity re-load
        /// </summary>
        public void NotifyIdentityObserver()
        {
            try
            {
                Delegates.IdentityEventHandler identityObserver = (Delegates.IdentityEventHandler)_observers[typeof(Delegates.IdentityEventHandler)];
                identityObserver.Invoke(this, new IdentityEventArgs()
                {
                    FriendlyName = _model.FriendlyName,
                    MyIdentity = ((Identity)_model.Identity).MyIdentity
                });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to show/hide the main app form
        /// </summary>
        /// <param name="close"></param>
        public void ShowMainForm(bool close)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to handle conference room command that came from the actions control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RoomButtonAction(object sender, EventArgs e)
        {
            try
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;
                if (args.RoomType == GenericEnums.RoomType.Video && _formWebCapture != null)
                {
                    // put the video transfers on hold
                    _formWebCapture.WaitRoomButtonAction(true);
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
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to display the last captured image by my webcam
        /// </summary>
        /// <param name="image"></param>
        public void UpdateWebcapture(Image image)
        {
            try
            {
                if (_formWebCapture != null)
                {
                    _formWebCapture.SetPicture(image);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to ask for app exit confirmation from the app user
        /// </summary>
        /// <returns></returns>
        public bool ExitConfirmation()
        {
            bool canExit = true;
            try
            {
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
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return canExit;
        }

        /// <summary>
        /// method used to tell the video streaming to wait for conference room button action to finish
        /// </summary>
        /// <param name="wait"></param>
        public void WaitRoomButtonAction(bool wait)
        {
            try
            {
                if (_formWebCapture != null)
                {
                    _formWebCapture.WaitRoomButtonAction(wait);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        void OpenMainForm(object identity)
        {
            try
            {
                Application.Run(_formMain);
                _formMain.BringToFront();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void OpenActionsForm(Object threadContext)
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    _formActions.StartPosition = FormStartPosition.Manual;
                    // position the Actions form at the right bottom of the screen
                    _formActions.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - _formActions.Width, 
                        Screen.PrimaryScreen.WorkingArea.Height - _formActions.Height);
                    _formActions.ShowDialog();
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
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
                try
                {
                    if (this._formWebCapture != null)
                    {
                        isClosed = this._formWebCapture.WebcaptureClosed;
                    }
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
                return isClosed;
            }
        }

        #endregion
    }
}
