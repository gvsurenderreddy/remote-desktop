using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MViewer
{
    public partial class FormActions : Form
    {
        #region private members

        #endregion

        #region c-tor

        public FormActions()
        {
            InitializeComponent();
        }

        #endregion

        #region event callbacks

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this form should not be closed while the app is running
            e.Cancel = true;
        }

        #endregion

        #region private methods

        #endregion
    }
}
