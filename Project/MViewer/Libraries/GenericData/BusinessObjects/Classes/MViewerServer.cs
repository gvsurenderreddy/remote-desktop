using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;
using System.Threading;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using Utils;
using System.Drawing;
using System.Drawing.Imaging;

namespace GenericDataLayer
{
    public class MViewerServer : IMViewerService
    {
        #region private members

        //Computer computer = new Computer();
        ControllerEventHandlers _controllerHandlers;
        string _identity;
        readonly object _syncVideoCaptures = new object();

        #endregion

        #region c-tor

        public MViewerServer(ControllerEventHandlers controllerHandlers, string identity)
        {
            _identity = identity;
            _controllerHandlers = controllerHandlers;
        }

        #endregion

        #region public methods

        public void SendWebcamCapture(byte[] capture, string senderIdentity)
        {
            try
            {
                lock (_syncVideoCaptures)
                {
                    //Thread.Sleep(2000);
                    MemoryStream ms = new MemoryStream(capture);
                    //read the Bitmap back
                    Image bmp = (Bitmap)Bitmap.FromStream(ms);

                    _controllerHandlers.VideoCaptureObserver.Invoke(this,
                        new VideoCaptureEventArgs()
                        {
                            Identity = senderIdentity,
                            CapturedImage = bmp
                        });

                    GC.Collect();
                }
            }
            catch (Exception)
            {

            }
        }

        readonly object _sync = new object();
        public void SendMicrophoneCapture(byte[] capture)
        {
            lock (_sync)
            {
                Computer computer = new Computer();
                computer.Audio.Play(capture, AudioPlayMode.Background);
            }
        }

        public void SendRoomAction(string identity, GenericEnums.RoomActionType roomType, GenericEnums.SignalType signalType)
        {
            lock (_syncVideoCaptures)
            {
                // todo: implement SendRoomAction
                switch (signalType)
                {
                    case GenericEnums.SignalType.Pause:

                        break;
                    case GenericEnums.SignalType.Stop:
                        _controllerHandlers.RoomClosingObserver.Invoke(this,
                            new RoomActionEventArgs()
                            {
                                ActionType = roomType,
                                Identity = identity,
                                SignalType = signalType
                            });
                        break;
                }
            }
        }

        public void AddContact(string identity, string friendlyName)
        {
            ContactsEventArgs args = new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Add,
                UpdatedContact = new Contact(-1, friendlyName, identity)
            };
            _controllerHandlers.ContactsObserver.Invoke(this, args);
        }

        public void RemoveContact(string identity)
        {
            ContactsEventArgs args = new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Remove,
                UpdatedContact = new Contact(-1, "", identity)
            };
            _controllerHandlers.ContactsObserver.Invoke(this, args);
        }

        public bool Ping()
        {
            return true;
        }

        #endregion

        //public void SendWebcamCapture2(Stream stream)
        //{
        //    try
        //    {
        //        byte[] capture = new byte[10000000];
        //        stream.Read(capture, 0, capture.Length);
        //        form.DisplayCapture(capture);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }
}
