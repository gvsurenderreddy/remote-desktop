using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Utils;
using GenericObjects;
using BusinessLogicLayer;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using UIControls;
using StrategyPattern;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region private members

        IRoomCommandInvoker _roomCommandInvoker;
        IHookCommandInvoker _commandInvoker;

        IView _view;
        IModel _model;

        string _language;

        #endregion

        #region c-tor

        public Controller()
        {
            try
            {
                _language = "en-US";
                // initialize the model
                _model = new Model();
                // initalize the view and bind it to the model
                _view = new View(_model);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// method used to initialize app settings
        /// </summary>
        public void InitializeSettings()
        {
            try
            {
                _roomCommandInvoker = new RoomCommandInvoker(SystemConfiguration.Instance.RoomHandlers);
                _commandInvoker = new HookCommandInvoker(SystemConfiguration.Instance.RemotingCommandHandlers);

                ControllerEventHandlers handlers = new ControllerEventHandlers()
                {
                    ClientConnectedObserver = this.ClientConnectedObserver,
                    VideoCaptureObserver = this.VideoCaptureObserver,
                    AudioCaptureObserver = this.OnAudioCaptureReceived,
                    ContactsObserver = this.ContactRequestObserver,
                    RoomButtonObserver = this.OnRoomButtonActionTriggered,
                    WaitRoomActionObserver = this.WaitRoomButtonActionObserver,
                    FileTransferObserver = this.FileTransferObserver,
                    FilePermissionObserver = this.FileTransferPermission,
                    RemotingCommandHandler = this.ExecuteRemotingCommand,
                    RemotingCaptureObserver = this.RemotingCaptureObserver
                };

                _model.IntializeModel(handlers);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to change the language for the UI
        /// </summary>
        /// <param name="language"></param>
        public void ChangeLanguage(string language)
        {
            try
            {
                _language = language;
                _view.ChangeLanguage(language);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void FocusActionsForm()
        {
            // todo: remove FocusActionsForm if not necessary
            _view.FocusActionsForm();
        }

        // todo: convert this to an event handler , use it in the Video Form as observer
        /// <summary>
        /// method used to update the active room and the actions form labels
        /// </summary>
        /// <param name="newIdentity"></param>
        /// <param name="roomType"></param>
        public void OnActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType)
        {
            try
            {
                // update the active room identity
                _view.RoomManager.ActiveRoom = newIdentity;
                // update the actions form button labels
                _view.UpdateLabels(newIdentity, roomType);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: convert this to an event handler , use it in the Main Form as observer
        /// <summary>
        /// method used to notify contacts of updated friendly name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FriendlyNameObserver(object sender, IdentityEventArgs e)
        {
            try
            {
                _model.Identity.UpdateFriendlyName(e.FriendlyName);
                // notify online contacts of updated friendly name
                _model.NotifyContacts(e.FriendlyName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: convert this to an event handler , use it in the View as observer
        /// <summary>
        /// method used to handle room button action signaled via UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRoomButtonActionTriggered(object sender, EventArgs e)
        {
            try
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;
                bool isContactOnline = _model.ClientController.IsContactOnline(args.Identity);
                if (isContactOnline)
                {
                    _roomCommandInvoker.PerformCommand(sender, args);
                }
                else
                {
                    _view.SetMessageText("Partner isn't online...");
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: convert this to an event handler , use it in the Main Form as observer
        /// <summary>
        /// method used to execute specific CRUD contacts operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Contact PerformContactsOperation(object sender, ContactsEventArgs e)
        { 
            Contact contact = null;
            try
            {
                if (e.Operation == GenericEnums.ContactsOperation.Load)
                {
                    // don't need to send signal to the Model
                    _view.NotifyContactsObserver();
                }
                else
                {
                    // add/remove/get/status/name update
                    contact = _model.PerformContactOperation(e);
                    _view.NotifyContactsObserver();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return contact;
        }

        /// <summary>
        /// method used to start app modules
        /// </summary>
        public void StartApplication()
        {
            try
            {
                // bind the observers
                _view.BindObservers(true);

                // open main form
                _view.ShowMainForm(false);
                _view.NotifyContactsObserver();

                // todo: use manual reset event instead of thread.sleep(0)
                Thread.Sleep(200);

                _view.NotifyIdentityObserver();
                _model.ServerController.StartServer();

                Thread.Sleep(200);

                // ping every single contact in the list and update it's status
                _model.PingContacts(null);

                // notify all online contacts that you came on too
                _model.NotifyContacts(GenericEnums.ContactStatus.Online);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
                _view.SetMessageText("Fatal error while starting app");
            }
        }

        /// <summary>
        /// method used to stop ongoing actions and app modules
        /// </summary>
        public void StopApplication()
        {
            try
            {
                // todo: update the StopApplication method with other actions

                // check for running video/audio/remoting s
                bool canExit = _view.ExitConfirmation();
                if (canExit)
                {
                    _view.SetFormMainBackgroundImage("Images/closed.gif");
                    _view.SetMessageText("Closing app...");
                    Thread t = new Thread(delegate()
                    {
                        // stop all active rooms
                        IList<string> partnerIdentities = _model.SessionManager.GetConnectedSessions(GenericEnums.RoomType.Video);
                        foreach (string identity in partnerIdentities)
                        {
                            StopVideo(this, new RoomActionEventArgs()
                                {
                                    Identity = identity,
                                    RoomType = GenericEnums.RoomType.Video,
                                    SignalType = GenericEnums.SignalType.Stop
                                });
                        }
                        // stop my webcapture form
                        StopVideoCapturing();
                    });
                    t.Start();

                    // stop the audio rooms also
                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopAudioPresentation();

                    PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StopRemotingPresentation();

                    // unbind the observers
                    _view.BindObservers(false);

                    _model.ServerController.StopServer();

                    // notify all contacts that you exited the 
                    _model.NotifyContacts(GenericEnums.ContactStatus.Offline);

                    Tools.Instance.Logger.LogInfo("MViewer application stopped");
                    // exit the environment
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        void ClientConnectedObserver(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            _roomCommandInvoker.PerformCommand(sender, args);
        }

        void WaitRoomButtonActionObserver(object sender, EventArgs e)
        {
            try
            {
                // todo: complete implemention of WaitRoomButtonAction for audio signal
                RoomActionEventArgs args = (RoomActionEventArgs)e;
                switch (args.RoomType)
                {
                    case GenericEnums.RoomType.Video:
                        bool freezeVideo = false;
                        if (args.SignalType == GenericEnums.SignalType.Wait)
                        {
                            freezeVideo = true;
                        }
                        // send the freeze signal to the webcapture obj
                        _view.WaitRoomButtonAction(freezeVideo);
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }
        
        // don't remove this one yet because PerformContactsOperation has a return type (cannot be used as event handler)
        void ContactRequestObserver(object sender, EventArgs e)
        {
            PerformContactsOperation(sender, (ContactsEventArgs)e);
        }

        #endregion
    }
}
