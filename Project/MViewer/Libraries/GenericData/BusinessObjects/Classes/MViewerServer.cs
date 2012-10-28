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

        #endregion

        public MViewerServer(ControllerEventHandlers controllerHandlers, string identity)
        {
            _identity = identity;
            _controllerHandlers = controllerHandlers;
        }

        public void InitializeRoom(string identity, GenericEnums.RoomActionType roomType)
        {
            switch (roomType)
            {
                case GenericEnums.RoomActionType.Video:
                    // initialize video chat form to receive captures from the client
                    _controllerHandlers.ClientConnectedHandler.Invoke(this, new RoomActionEventArgs()
                    {
                        ActionType = GenericEnums.RoomActionType.Video,
                        SignalType = GenericEnums.SignalType.Start,
                        Identity = _identity
                    });

                    // todo: initialize my webcam form so that I can send my captures to the connected contact


                    break;
                case GenericEnums.RoomActionType.Audio:

                    break;
                case GenericEnums.RoomActionType.Remoting:

                    break;
                case GenericEnums.RoomActionType.Send:

                    break;
            }
        }

        public void SendWebcamCapture(byte[] capture)
        {
            Thread.Sleep(2000);
            MemoryStream ms = new MemoryStream(capture);
            //read the Bitmap back
            Image bmp = (Bitmap)Bitmap.FromStream(ms);

            _controllerHandlers.VideoCaptureHandler.Invoke(this,
                new VideoCaptureEventArgs()
                {
                    Identity = _identity,
                    CapturedImage = bmp
                });

            GC.Collect();
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
            // todo: implement SendRoomAction

        }

        public bool Ping()
        {
            return true;
        }

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
