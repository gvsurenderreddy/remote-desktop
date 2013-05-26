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
using Abstraction;

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
                    VideoCaptureObserver = this.OnVideoCaptureReceived,
                    AudioCaptureObserver = this.OnAudioCaptureReceived,
                    ContactsObserver = this.ContactsObserver,
                    RoomButtonObserver = this.OnRoomButtonActionTriggered,
                    WaitRoomActionObserver = this.WaitRoomButtonActionObserver,
                    FileTransferObserver = this.FileTransferObserver,
                    ConferencePermissionObserver = this.ConferencePermissionObserver,
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

        // todo: optional - convert this to an event handler , use it in the Video Form as observer
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
                switch (roomType)
                {
                    case GenericEnums.RoomType.Audio:
                        ((ActiveRooms)_view.RoomManager.ActiveRooms).AudioRoomIdentity = newIdentity;
                        break;
                    case GenericEnums.RoomType.Video:
                        ((ActiveRooms)_view.RoomManager.ActiveRooms).VideoRoomIdentity = newIdentity;
                        break;
                    case GenericEnums.RoomType.Remoting:
                        ((ActiveRooms)_view.RoomManager.ActiveRooms).RemotingRoomIdentity = newIdentity;
                        break;
                }
                // update the actions form button labels
                _view.UpdateLabels(newIdentity, roomType);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: optional - convert this to an event handler , use it in the Main Form as observer
        /// <summary>
        /// method used to notify contacts of updated friendly name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FriendlyNameObserver(object sender, EventArgs e)
        {
            try
            {
                IdentityEventArgs args = (IdentityEventArgs)e;
                _model.Identity.UpdateFriendlyName(args.FriendlyName);
                // notify online contacts of updated friendly name
                _model.NotifyContacts(args.FriendlyName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        // todo: optional - convert this to an event handler , use it in the View as observer
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

        // todo: optional - convert this to an event handler , use it in the Main Form as observer
        /// <summary>
        /// method used to execute specific CRUD contacts operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public ContactBase PerformContactsOperation(object sender, EventArgs e)
        {
            ContactsEventArgs args = (ContactsEventArgs)e;
            ContactBase contact = null;
            try
            {
                if (args.Operation == GenericEnums.ContactsOperation.Load)
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
                // check for running video/audio/remoting s
                bool canExit = _view.ExitConfirmation();
                if (canExit)
                {
                    _view.SetFormMainBackgroundImage("Images/closed.gif");
                    _view.SetMessageText("Closing app...");
                    Thread t = new Thread(delegate()
                    {
                        try
                        {
                            StopVideo(this, new RoomActionEventArgs()
                            {
                                Identity = ((ActiveRooms)_view.RoomManager.ActiveRooms).VideoRoomIdentity,
                                RoomType = GenericEnums.RoomType.Video,
                                SignalType = GenericEnums.SignalType.Stop
                            });

                            // stop my webcapture form
                            StopVideoCapturing();
                        }
                        catch (Exception ex)
                        {
                            Tools.Instance.Logger.LogError(ex.ToString());
                        }
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

        void ConferencePermissionObserver(object sender, EventArgs e)
        {
            try
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;
                bool canSend = _view.RequestConferencePermission(args.Identity, args.RoomType);
                args.HasPermission = canSend;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void ClientConnectedObserver(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            _roomCommandInvoker.PerformCommand(sender, args);
        }

        void WaitRoomButtonActionObserver(object sender, EventArgs e)
        {
            try
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;
                bool freeze = false;
                if (args.SignalType == GenericEnums.SignalType.Wait)
                {
                    freeze = true;
                }
                // send the freeze signal to the webcapture obj
                _view.WaitRoomButtonAction(freeze, args.RoomType);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }
        
        // don't remove this one yet because PerformContactsOperation has a return type (cannot be used as event handler)
        void ContactsObserver(object sender, EventArgs e)
        {
            PerformContactsOperation(sender, (ContactsEventArgs)e);
        }

        #endregion
    }
}
