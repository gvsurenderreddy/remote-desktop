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
            // todo: implement InitializeRoom
            _controllerHandlers.ClientConnectedHandler.Invoke(this, new RoomActionEventArgs()
            {
                ActionType = GenericEnums.RoomActionType.Video,
                SignalType = GenericEnums.SignalType.Start,
                Identity = _identity
            });
        }

        public void SendWebcamCapture(byte[] capture)
        {
            Thread.Sleep(2000);

            Image image = ImageConverter.byteArrayToImage(capture);
            _controllerHandlers.VideoCaptureHandler.Invoke(this,
                new VideoCaptureEventArgs()
                {
                    Identity = _identity,
                    CapturedImage = image
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
