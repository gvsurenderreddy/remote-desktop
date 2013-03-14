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
using GenericDataLayer;

namespace GenericDataLayer
{
    public class MViewerServer : IMViewerService
    {
        #region private members

        //Computer computer = new Computer();
        ControllerEventHandlers _controllerHandlers;
        string _identity;
        ManualResetEvent _syncVideoCaptures = new ManualResetEvent(true);
        readonly object _syncAudioCaptures = new object();
        
        #endregion

        #region c-tor

        public MViewerServer(ControllerEventHandlers controllerHandlers, string identity)
        {
            _identity = identity;
            _controllerHandlers = controllerHandlers;
        }

        #endregion

        #region public methods

        public void SendRemotingCapture(byte[] capture, string senderIdentity)
        {
            // todo: implement SendRemotingCapture
            _controllerHandlers.RemotingCaptureObserver.Invoke(this,
                new RemotingCaptureEventArgs()
                {
                    Identity = senderIdentity,
                    Capture = capture
                });
        }

        public bool SendingPermission(string senderIdentity, string fileName, long fileSize)
        {
            bool canSend = false;

            TransferInfo transferInfo = new TransferInfo()
                {
                    FileName = fileName,
                    FileSize = fileSize
                };

            // request permission from the user
            _controllerHandlers.FilePermissionObserver.Invoke(this, 
                new RoomActionEventArgs()
                {
                    Identity = senderIdentity,
                    RoomType = GenericEnums.RoomType.Send,
                    TransferInfo = transferInfo
                });
            canSend = transferInfo.HasPermission;
            return canSend;
        }

        public void SendFile(byte[] fileStream, string fileName)
        {
            // SendFile
            _controllerHandlers.FileTransferObserver.Invoke(fileStream, new RoomActionEventArgs()
            {
                RoomType = GenericEnums.RoomType.Send,
                TransferInfo = new TransferInfo() { FileName = fileName }
            });
        }

        public void UpdateContactStatus(string senderIdentity, GenericEnums.ContactStatus newStatus)
        {
            // propagate the update to the UI, through the controller
            _controllerHandlers.ContactsObserver.Invoke(this, new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Status,
                UpdatedContact = new Contact(-1, senderIdentity, newStatus)
            });
        }

        public void UpdateFriendlyName(string senderIdentity, string newFriendlyName)
        {
            // propagate the update to the UI, through the controller
            _controllerHandlers.ContactsObserver.Invoke(this, new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Update,
                UpdatedContact = new Contact(-1, newFriendlyName, senderIdentity)
            });
        }

        public void SendWebcamCapture(byte[] capture, string senderIdentity)
        {
            try
            {
                // wait for the room command to finish (might be a stop signal)
                _syncVideoCaptures.WaitOne();
               
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
            catch (Exception)
            {

            }
        }

        public void SendMicrophoneCapture(byte[] capture)
        {
            lock (_syncAudioCaptures)
            {
                Computer computer = new Computer();
                computer.Audio.Play(capture, AudioPlayMode.Background);
            }
        }

        public void WaitRoomButtonAction(string senderIdentity, GenericEnums.RoomType roomType, bool wait)
        {
            _controllerHandlers.WaitRoomActionObserver.Invoke(null,
                new RoomActionEventArgs()
                {
                    Identity = senderIdentity,
                    RoomType = roomType,
                    SignalType = wait == true ? GenericEnums.SignalType.Wait: GenericEnums.SignalType.RemoveWait
                });
        }

        public void SendRoomButtonAction(string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType)
        {
            // todo: complete implementation of SendRoomButtonAction
            switch(roomType)
            {
                case GenericEnums.RoomType.Video:
                    _syncVideoCaptures.Reset();
                    switch (signalType)
                    {
                        case GenericEnums.SignalType.Stop:
                            _controllerHandlers.RoomButtonObserver.Invoke(this,
                                new RoomActionEventArgs()
                                {
                                    RoomType = roomType,
                                    Identity = identity,
                                    SignalType = signalType
                                });
                            break;
                    }
                    _syncVideoCaptures.Set();
                break;
                case GenericEnums.RoomType.Audio:

                break;
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

        //public void SendWebcamCapture2(FileStream stream)
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
