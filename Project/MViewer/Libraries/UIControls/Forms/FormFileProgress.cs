using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace UIControls
{
    public partial class FormFileProgress : Form
    {
        #region private members

        bool _isRunning;
        readonly object _syncProgress = new object();

        #endregion

        #region c-tor

        public FormFileProgress(string fileName, string partner)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ChangeLanguage(string language)
        {
            Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(FormFileProgress));
        }

        public void StartPB()
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopProgress()
        {
            lock (_syncProgress)
            {
                _isRunning = false;
            }
        }

        #endregion

        #region private methods

        void UpdatePB()
        {
            try
            {
                if (pbFileProgress.InvokeRequired)
                {
                    pbFileProgress.Invoke(new MethodInvoker(delegate { pbFileProgress.PerformStep(); }));
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
