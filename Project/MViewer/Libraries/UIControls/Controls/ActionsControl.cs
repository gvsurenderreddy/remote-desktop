using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using GenericDataLayer;

namespace UIControls
{
    public partial class ActionsControl : UserControl
    {
        #region private members

        EventHandler _actionTriggered;

        #endregion

        #region c-tor

        public ActionsControl()
        {
            InitializeComponent();

        }

        public ActionsControl(EventHandler actionTriggered)
        {
            InitializeComponent();

            _actionTriggered = actionTriggered;
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
            GenericEnums.SignalType signalType = GenericEnums.SignalType.Undefined;
            // do specific action , check what button was clicked by looking at the sender
            if (sender == btnAudio || sender == btnVideo || sender == btnRemote)
            {
                signalType = GenericEnums.SignalType.Start;
            }
            else
            {
                signalType = GenericEnums.SignalType.Pause;
            }
            GenericEnums.RoomActionType actionType = GenericEnums.RoomActionType.Undefined;
            if (sender == btnAudio || sender == btnMuteAudio)
            {
                actionType = GenericEnums.RoomActionType.Audio;
            }
            else
            {
                if (sender == btnVideo || sender == btnPauseVideo)
                {
                    actionType = GenericEnums.RoomActionType.Video;
                }
                else
                {
                    actionType = GenericEnums.RoomActionType.Remoteing;
                }
            }

            // finally, update the button text
            ToggleStatusUpdate(signalType, (Button)sender);

            // trigger the event so that the Controller does specific action
            // provide the action type as event arg
            RoomActionEventArgs args = new RoomActionEventArgs()
            {
                ActionType = actionType,
                SignalType = signalType
            };
            _actionTriggered.Invoke(this, args);
        }

        #endregion

        #region private methods

        void ToggleStatusUpdate(GenericEnums.SignalType buttonType, Button button)
        {
            switch (buttonType)
            {
                case GenericEnums.SignalType.Start:
                    if (button.Text == ButtonStatuses.ButtonStartStatus.Start.ToString())
                    {
                        button.Text = ButtonStatuses.ButtonStartStatus.Stop.ToString();
                    }
                    else
                    {
                        button.Text = ButtonStatuses.ButtonStartStatus.Start.ToString();
                    }
                    break;
                case GenericEnums.SignalType.Pause:
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
