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

        #endregion

        #region c-tor

        public Controller()
        {
            // initialize the model
            _model = new Model();
            // initalize the view and bind it to the model
            _view = new View(_model);
        }

        #endregion

        #region event handlers

        public void InitializeSettings()
        {
            _roomCommandInvoker = new RoomCommandInvoker(SystemConfiguration.Instance.RoomHandlers);
            _commandInvoker = new HookCommandInvoker(SystemConfiguration.Instance.RemotingCommandHandlers);

            ControllerEventHandlers handlers = new ControllerEventHandlers()
            {
                ClientConnectedObserver = this.ClientConnectedObserver,
                VideoCaptureObserver = this.VideoCaptureObserver,
                AudioCaptureObserver = this.OnAudioCaptureReceived,
                ContactsObserver = this.ContactRequestObserver,
                RoomButtonObserver = this.RoomButtonAction,
                WaitRoomActionObserver = this.WaitRoomButtonActionObserver,
                FileTransferObserver = this.FileTransferObserver,
                FilePermissionObserver = this.FileTransferPermission,
                RemotingCommandHandler = this.ExecuteRemotingCommand,
                RemotingCaptureObserver = this.RemotingCaptureObserver
            };

            _model.IntializeModel(handlers);
        }

        #endregion

        #region public methods

        public void FocusActionsForm()
        {
            // todo: remove FocusActionsForm if not necessary
            _view.FocusActionsForm();
        }

        // todo: convert this to an event handler , use it in the Video Form as observer
        public void OnActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType)
        {
            // update the active room identity
            _view.RoomManager.ActiveRoom = newIdentity;
            // update the actions form button labels
            _view.UpdateLabels(newIdentity, roomType);
        }

        // todo: convert this to an event handler , use it in the Main Form as observer
        public void IdentityObserver(object sender, IdentityEventArgs e)
        {
            _model.Identity.UpdateFriendlyName(e.FriendlyName);
            // notify online contacts of updated friendly name
            _model.NotifyContacts(e.FriendlyName);
        }

        // todo: convert this to an event handler , use it in the View as observer
        public void RoomButtonAction(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            bool isContactOnline = _model.ClientController.IsContactOnline(args.Identity);
            if (isContactOnline)
            {
                _roomCommandInvoker.PerformCommand(sender, args);
            }
            else
            {
                _view.SetResultText("Partner isn't online...");
            }
        }

        // todo: convert this to an event handler , use it in the Main Form as observer
        public Contact PerformContactsOperation(object sender, ContactsEventArgs e)
        {
            Contact contact = null;
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
            return contact;
        }

        public void StartApplication()
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

        public void StopApplication()
        {
            // todo: update the StopApplication method with other actions

            // check for running video/audio/remoting s
            bool canExit = _view.ExitConfirmation();
            if (canExit)
            {
                _view.SetResultText("Closing app...");
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

                // exit the environment
                Environment.Exit(0);
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
            // todo: complete implemention of WaitRoomButtonAction
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
        
        // don't remove this one yet because PerformContactsOperation has a return type (cannot be used as event handler)
        void ContactRequestObserver(object sender, EventArgs e)
        {
            PerformContactsOperation(sender, (ContactsEventArgs)e);
        }

        #endregion

        #region proprieties

        public string MyIdentity()
        {
            return _model.Identity.MyIdentity;
        }

        #endregion
    }
}
