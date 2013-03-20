using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIControls
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

            // Display the ProgressBar control.
            pbFileProgress.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            pbFileProgress.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            pbFileProgress.Maximum = 50;
            // Set the initial value of the ProgressBar.
            pbFileProgress.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            pbFileProgress.Step = 1;

        }

        public void StartPB()
        {
            _isRunning = true;
            while (_isRunning)
            {
                if (pbFileProgress.Value < pbFileProgress.Maximum)
                {
                    Thread.Sleep(500);
                    UpdatePB();
                }
                else
                {
                    //pbFileProgress.Value = 1;
                    if (pbFileProgress.InvokeRequired)
                    {
                        pbFileProgress.Invoke(new MethodInvoker(delegate { pbFileProgress.Value = 1; }));
                    }
                }

                lock (_syncProgress)
                {
                    if (!_isRunning)
                    {
                        break;
                    }
                }
            }
            this.Invoke(new MethodInvoker(delegate() { this.Close(); }));
        }

        public void StopProgress()
        {
            lock (_syncProgress)
            {
                _isRunning = false;
            }
        }
        delegate void MyHandlerDelegate();

        void UpdatePB()
        {
            //pbFileProgress.PerformStep();
            if (pbFileProgress.InvokeRequired)
            {
                pbFileProgress.Invoke(new MethodInvoker(delegate { pbFileProgress.PerformStep(); }));
            }

        }

    }
}
