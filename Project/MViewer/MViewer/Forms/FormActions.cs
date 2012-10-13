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

namespace MViewer
{
    public partial class FormActions : Form
    {
        #region private members

        public readonly EventHandlers.ActionsEventHandler ActionsObserver;
 
        #endregion

        #region c-tor

        //public FormActions()
        //{
        //    InitializeComponent();
        //}

        public FormActions(
            //EventHandlers.ActionsEventHandler actionsEventHandler
            )
        {
            InitializeComponent();

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
            //ActionsObserver.Invoke(sender, (ActionsEventArgs)e);
            // todo: use the Controller and take specific action when event has been triggered using the Actions control

        }

        #endregion

        #region private methods

        #endregion
    }
}
