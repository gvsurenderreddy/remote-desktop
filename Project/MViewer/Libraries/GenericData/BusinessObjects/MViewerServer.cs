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

namespace GenericDataLayer
{
    //[ServiceBehaviorAttribute(ConcurrencyMode = ConcurrencyMode.Multiple,
    //   InstanceContextMode = InstanceContextMode.Single
    //    //, IncludeExceptionDetailInFaults=true
    //   )]
    public class MViewerServer : IMViewerService
    {
        public MViewerServer()
        {
            InitializeForm();
        }

        public void InitializeForm()
        {
            Thread t = new Thread(delegate()
            {
                //form.ShowDialog();
            });
            t.Start();
        }

        public void SendWebcamCapture(byte[] capture)
        {
            //form.DisplayCapture(capture);
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
