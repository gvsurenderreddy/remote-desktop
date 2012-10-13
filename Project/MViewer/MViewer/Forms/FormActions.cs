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
        EventHandler _actionButtonPressed;

        #endregion

        #region c-tor

        public FormActions()
        {
            InitializeComponent();
        }

        public FormActions(EventHandler actionsEventHandler)
        {
            InitializeComponent();
            _actionButtonPressed = actionsEventHandler;
            ActionsObserver = new EventHandlers.ActionsEventHandler(ActionTriggered);
        }

        #endregion

        #region event callbacks

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this form should not be closed while the app is running
            e.Cancel = true;
        }

        private void ActionTriggered(object sender, EventArgs e)
        {
            // use the Controller and take specific action when event has been triggered using the Actions control
            _actionButtonPressed.Invoke(sender, e);
        }

        #endregion

        #region private methods

        #endregion
    }
}
