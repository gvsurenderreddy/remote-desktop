using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebcamCaptureLib;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Threading;
using AudioCaptureLib;

namespace VideoChatClient
{
    public partial class FrmVideoChat : Form
    {
        #region private members

      

        #endregion

        #region c-tor

        public FrmVideoChat()
        {
            InitializeComponent();

           
        }

        #endregion

        #region override events


        #endregion

        #region capture events

       

        #endregion

        #region callbacks

        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(delegate()
            {
                FrmVideoCapture videoChat = new FrmVideoCapture(txtServer.Text.Trim(), int.Parse( nudTimespan.Value.ToString()));
                videoChat.ShowDialog();
            });
            t.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            
        }
        
        private void nudTimespan_ValueChanged(object sender, EventArgs e)
        {
            // todo: update the capture timespan
            //_webcamCapture.CaptureTimespan = int.Parse(nudTimespan.Value.ToString());

            //RestartTimer();
        }

        #endregion

        #region public methods



        #endregion

        #region private methdos

        //void RestartTimer()
        //{
        //    _webcamCapture.StopCapturing();
        //    _webcamCapture.StartCapturing();
        //}
        
        #endregion


    }
}
