using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace UIControls
{
    public partial class AudioControl : UserControl
    {
        #region private members


        #endregion

        #region c-tor

        public AudioControl()
        {
            InitializeComponent();
        }

        #endregion

        #region public methods

        public void SetPartnerName(string friendlyName)
        {
            try
            {
                txtPartner.Text = friendlyName;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ToggleStatusUpdate()
        {
            try
            {
                /// switch bewteen muted/unmuted status
                if (txtStatus.Text.Trim() == "Muted")
                {
                    if (txtStatus.InvokeRequired)
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate { txtStatus.Text = "Active"; }));
                    }
                    else
                    {
                        txtStatus.Text = "Active";
                    }
                }
                else
                {
                    if (txtStatus.InvokeRequired)
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate { txtStatus.Text = "Muted"; }));
                    }
                    else
                    {
                        txtStatus.Text = "Muted";
                    }
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
