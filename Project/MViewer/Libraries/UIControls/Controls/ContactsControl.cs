using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UIControls
{
    public partial class ContactsControl : UserControl
    {
        #region private members

        EventHandler _closePressed;

        #endregion

        #region c-tor

        public ContactsControl()
        {
            InitializeComponent();
        }

        public ContactsControl(EventHandler closePressed)
        {
            InitializeComponent();
            _closePressed = closePressed;
        }

        #endregion

        #region event callbacks

        private void ContactsControl_ClosePressed(object sender, EventArgs e)
        {
            if (_closePressed != null)
            {
                _closePressed.Invoke(sender, e);
            }
        }

        private void ContactsControl_Load(object sender, EventArgs e)
        {

        }

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this form should not be closed while the app is running
            e.Cancel = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // todo: implement btnAdd_Click
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // todo: implement btnRemove_Click
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // todo: implement btnUpdate_Click
        }

        #endregion

        #region private methods

        #endregion
    }
}
