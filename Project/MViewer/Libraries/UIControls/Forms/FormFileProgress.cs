using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIControls.Forms
{
    public partial class FormFileProgress : Form
    {
        bool _isRunning;
        readonly object _syncProgress = new object();

        public FormFileProgress(string fileName, string partner)
        {
            InitializeComponent();

            this.Text = this.Text + ": " + fileName + " to " + partner;
            txtFilename.Text = fileName;
            txtPartner.Text = partner;

            _isRunning = true;
            pbFileProgress.Value = 0;
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartProgress));
            //StartProgress(null);
        }

        public void StopProgress()
        {
            lock (_syncProgress)
            {
                _isRunning = false;
            }
        }

        delegate void UpdateProgress();

        void ResetPB()
        {
            pbFileProgress.Value = 0; pbFileProgress.Refresh();
        }

        void UpdatePB()
        {
            pbFileProgress.Value += 1; pbFileProgress.Refresh();
        }

        void StartProgress(object state)
        {
            while (_isRunning)
            {
                if (pbFileProgress.Value < pbFileProgress.Maximum)
                {
                    pbFileProgress.BeginInvoke(new UpdateProgress(
                    UpdatePB
                        )
                    );
                }
                else
                {
                    pbFileProgress.BeginInvoke(new UpdateProgress(
                      ResetPB
                          )
                      );
                    
                }
               
                lock (_syncProgress)
                {
                    if (!_isRunning)
                    {
                        break;
                    }
                }
            }
        }
    }
}
