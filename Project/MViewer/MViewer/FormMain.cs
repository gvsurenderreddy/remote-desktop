using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MViewer
{
    public partial class FormMain : Form
    { 
        #region private members

        FormActions _formActions;

        #endregion

        #region c-tor

        public FormMain()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(OpenActionsForm);
        }

        #endregion

        #region event callbacks

        private void ContactsControl_Load(object sender, EventArgs e)
        {
            // todo: load saved contacts
        }

        private void ContactsControl_ClosePressed(object sender, EventArgs e)
        {
            // todo: perform other specific actions for closing application

            Environment.Exit(0);
        }

        #endregion

        #region private methods

        void OpenActionsForm(Object threadContext)
        {
            // todo: open the Actions form
            _formActions = new FormActions();
            _formActions.StartPosition = FormStartPosition.Manual;
            // todo: position the Actions form at the bottom of the screen
            _formActions.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - _formActions.Width, Screen.PrimaryScreen.WorkingArea.Height - _formActions.Height);
            _formActions.ShowDialog();            
        }

        #endregion
    }
}
