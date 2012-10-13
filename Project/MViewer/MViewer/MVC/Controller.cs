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

        #endregion

        #region c-tor

        public Controller()
        {
            // initialize the model
            _model = new Model();
            // initalize the view
            _view = new View(_model);


        }

        #endregion

        #region public event handlers

        private void WebCamImageCaptured(object source, EventArgs e)
        {
            try
            {
                CaptureEventArgs args = (CaptureEventArgs)e;
                // display the captured picture
                _view.UpdateWebcapture(args.CapturedImage);

                byte[] bytes = ImageConverter.imageToByteArray(args.CapturedImage);

                //while (!_audioCapture.AudioCaptureReady)
                //{
                //    Thread.Sleep(200);
                //}

                // todo: send to server
                // loop on all connected contacts and send them the captures
                //_model.ClientController.GetClient(((CaptureEventArgs)e).
                //_webcamClient.SendWebcamCapture(bytes);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //_webcamCapture.StopCapturing();
            }
        }

        public void NotificationReceived()
        {
            _model.PingContacts();
        }

        public void IdentityUpdated(object sender, IdentityEventArgs e)
        {
            _model.Identity.UpdateFriendlyName(e.FriendlyName);
        }

        public void StartVideoChat(WebcamCapture webcamControl , RoomActionEventArgs e)
        {
            // create Presenter and start the presentation
            int timerInterval = 20;
            int height = 354, width = 311;

            IPresenter presenter = new Presenter(webcamControl, e.Identity, timerInterval, height, width, new EventHandler(this.WebCamImageCaptured));
            _model.PresenterManager.AddPresenter(e.Identity, presenter);
            _model.PresenterManager.StartPresentation(e.Identity);
        }

        public void PerformRoomAction(object sender, RoomActionEventArgs e)
        {
            // todo: perform specific actions when action has been triggered
            switch (e.ActionType)
            {
                case GenericEnums.RoomActionType.Audio:

                    break;
                case GenericEnums.RoomActionType.Remote:

                    break;
                case GenericEnums.RoomActionType.Send:

                    break;
                case GenericEnums.RoomActionType.Video:
                    switch (e.SignalType)
                    {
                        case GenericEnums.SignalType.Start:
                            // start the video chat
                            Thread t = new Thread(delegate()
                            {
                                StartVideoChat(sender, e);
                            });
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            break;
                        case GenericEnums.SignalType.Pause:

                            break;
                    }
                    break;
            }
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

        public void StartApplication()
        {
            // bind the observers
            _view.BindObservers(true);

            // open main form
            _view.ShowMainForm(false);
            NotifyContactsObserver();

            // todo: use manual reset event instead of thread.sleep(0)
            Thread.Sleep(2000);

            _view.NotifyIdentityObserver();

            _model.ServerController.StartServer();

            Thread.Sleep(5000);

            // ping every single contact in the list and update it's status
            NotificationReceived();
        }

        public void StopApplication()
        {
            // unbind the observers
            _view.BindObservers(false);

            _model.ServerController.StopServer();

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

        #region private methods

        void StartVideoChat(object sender, RoomActionEventArgs e)
        {
            _model.ClientController.AddClient(e.Identity);
            //_model.ClientController.StartClient(e.Identity);
            _view.ShowMyWebcamForm(e);

            //int timerInterval = 20;
            //int height = 354, width = 311;
            //WebcamCapture webcamControl = _view.GetWebcaptureControl;

            //IPresenter presenter = new Presenter(webcamControl, e.Identity, timerInterval, height, width, new EventHandler(this.WebCamImageCaptured));
            //_model.PresenterManager.AddPresenter(e.Identity, presenter);
            //_model.PresenterManager.StartPresentation(e.Identity);
        }

        #endregion

        #region proprieties



        #endregion
    }
}
