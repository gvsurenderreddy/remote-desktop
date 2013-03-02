using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UIControls;
using Utils;
using GenericDataLayer;

namespace MViewer
{
    public partial class FormActions : Form
    {
        #region private members

        public readonly EventHandlers.ActionsEventHandler ActionsObserver;
        EventHandler _roomActionEventHandler;
        public delegate void UpdateLabelsDel(bool start, bool pause, GenericEnums.RoomType roomType);
        public UpdateLabelsDel myDelegate;

        #endregion

        #region c-tor

        public FormActions()
        {
            InitializeComponent();
            myDelegate = new UpdateLabelsDel(actionsControl1.UpdateLabels);
        }

        public FormActions(EventHandler roomActionEventHandler)
        {
            InitializeComponent();
            _roomActionEventHandler = roomActionEventHandler;
            ActionsObserver = new EventHandlers.ActionsEventHandler(ActionTriggered);
            myDelegate = new UpdateLabelsDel(actionsControl1.UpdateLabels);
        }

        #endregion

        #region event callbacks

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this form should not be closed while the app is running
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        private void ActionTriggered(object sender, EventArgs e)
        {
            // use the Controller and take specific action when event has been triggered using the Actions control
            _roomActionEventHandler.Invoke(sender, e);
        }

        #endregion

        #region public methods

        public void UpdateLabels(bool start, bool pause, GenericEnums.RoomType roomType)
        {
            this.Invoke(myDelegate, start, pause, roomType);
        }

        #endregion
    }
}
