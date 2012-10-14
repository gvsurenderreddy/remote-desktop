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
    //[ServiceBehaviorAttribute(ConcurrencyMode = ConcurrencyMode.Multiple,
    //   InstanceContextMode = InstanceContextMode.Single
    //    //, IncludeExceptionDetailInFaults=true
    //   )]
    public class MViewerServer : IMViewerService
    {
        #region private members

        FrmVideoChatRoom form;
        //Computer computer = new Computer();

        #endregion

        public MViewerServer()
        {
            
        }

        public void InitializeRoom(string identity, GenericEnums.RoomActionType roomType)
        {
            // todo: implement InitializeRoom
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
