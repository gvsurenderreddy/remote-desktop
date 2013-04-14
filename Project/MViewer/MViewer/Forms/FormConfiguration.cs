using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace MViewer
{
    public partial class FormConfiguration : Form
    {
        bool _changed;

        public FormConfiguration()
        {
            try
            {
                _changed = false;
                InitializeComponent();
                txtIP.Text = SystemConfiguration.Instance.MyAddress;
                txtPort.Text = SystemConfiguration.Instance.Port.ToString();
                nudAudio.Value = SystemConfiguration.Instance.PresenterSettings.AudioTimerInterval;
                nudVideo.Value = SystemConfiguration.Instance.PresenterSettings.VideoTimerInterval;
                nudRemoting.Value = SystemConfiguration.Instance.PresenterSettings.RemotingTimerInterval;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                Configuration config = ConfigurationManager.OpenExeConfiguration(
                                   Assembly.GetEntryAssembly().Location);
                config.AppSettings.Settings["MyAddress"].Value = txtIP.Text.Trim();
                config.AppSettings.Settings["port"].Value = txtPort.Text.Trim();
                config.AppSettings.Settings["videoTimerInterval"].Value = nudVideo.Text.Trim();
                config.AppSettings.Settings["remotingTimerInterval"].Value = nudRemoting.Text.Trim();
                config.AppSettings.Settings["audioTimerInterval"].Value = nudAudio.Text.Trim();
                config.Save();
                _changed = true;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_changed)
            {
                MessageBox.Show("Must restart the app for the changes to take effect!", "Restart needed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Close();
        }
    }
}
