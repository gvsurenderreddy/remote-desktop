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
    public partial class ActionsControl : UserControl
    {
        #region private members


        #endregion

        #region c-tor

        public ActionsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region event callbacks

        private void btnSend_Click(object sender, EventArgs e)
        {
            // send the file to the selected contact

            // todo: determine which contact window is activated and save the contact ID
            ulong contactID = 0;

            string filePath = string.Empty;
            FileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = fileDialog.FileName;

                // todo: provide the file path and contact ID to the File transfer module

            }
        }

        private void BtnAction_Click(object sender, EventArgs e)
        {
            ButtonStatuses.ButtonType buttonType = ButtonStatuses.ButtonType.Undefined;
            // todo: do specific action , check what button was clicked by looking at the sender
            if (sender == btnAudio || sender == btnVideo || sender == btnRemote)
            {
                // todo: perform specific action when the start button was pressed

                buttonType = ButtonStatuses.ButtonType.Start;
            }
            else
            {
                // todo: perform specific action when the pause button was pressed

                buttonType = ButtonStatuses.ButtonType.Pause;
            }

            // finally, update the button text
            ToggleStatusUpdate(buttonType, (Button)sender);
        }

        #endregion

        #region private methods

        void ToggleStatusUpdate(ButtonStatuses.ButtonType buttonType, Button button)
        {
            switch (buttonType)
            {
                case ButtonStatuses.ButtonType.Start:
                    if (button.Text == ButtonStatuses.ButtonStartStatus.Start.ToString())
                    {
                        button.Text = ButtonStatuses.ButtonStartStatus.Stop.ToString();
                    }
                    else
                    {
                        button.Text = ButtonStatuses.ButtonStartStatus.Start.ToString();
                    }
                    break;
                case ButtonStatuses.ButtonType.Pause:
                    if (button.Text == ButtonStatuses.ButtonPauseStatus.Pause.ToString())
                    {
                        button.Text = ButtonStatuses.ButtonPauseStatus.Resume.ToString();
                    }
                    else
                    {
                        button.Text = ButtonStatuses.ButtonPauseStatus.Pause.ToString();
                    }
                    break;
            }
        }

        #endregion
    }
}
