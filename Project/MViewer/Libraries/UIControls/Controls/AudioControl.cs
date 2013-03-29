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

        #region event callbacks

        

        #endregion

        #region private methods

        

        #endregion

        #region public methods

        public void SetPartnerName(string friendlyName)
        {
            txtPartner.Text = friendlyName;
        }

        public void ToggleStatusUpdate()
        {
            /// switch bewteen muted/unmuted status
            if (txtStatus.Text.Trim() == ButtonStatuses.AudioStatus.Mute.ToString())
            {
                txtStatus.Text = ButtonStatuses.AudioStatus.Unmute.ToString();
            }
            else
            {
                txtStatus.Text = ButtonStatuses.AudioStatus.Mute.ToString();
            }
        }

        #endregion
    }
}
