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

namespace GenericDataLayer
{
    public class MViewerServer : IMViewerService
    {
        #region private members

        FrmVideoChatRoom form;
        //Computer computer = new Computer();
        EventHandler _clientConnected;
        string _identity;

        #endregion

        public MViewerServer(EventHandler clientConnected, string identity)
        {
            _identity = identity;
            _clientConnected = clientConnected;
        }

        public void InitializeRoom(string identity, GenericEnums.RoomActionType roomType)
        {
            // todo: implement InitializeRoom
            _clientConnected.Invoke(this, new RoomActionEventArgs()
            {
                ActionType = GenericEnums.RoomActionType.Video,
                SignalType = GenericEnums.SignalType.Start,
                Identity = _identity
            });
        }

        public void InitializeForm()
        {
            Thread t = new Thread(delegate()
            {
                form = new FrmVideoChatRoom();
                form.ShowDialog();
            });
            t.Start();
        }

        public void SendWebcamCapture(byte[] capture)
        {
            if (form == null)
            { 
                InitializeForm(); 
            }
            Thread.Sleep(2000);
            form.DisplayCapture(capture);
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
