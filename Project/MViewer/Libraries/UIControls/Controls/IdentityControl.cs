﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UIControls.CrossThreadOperations;
using Utils;
using GenericObjects;
using System.Globalization;
using System.Threading;

namespace UIControls
{
    public partial class IdentityControl : UserControl
    {
        #region private members

        EventHandler _onIdentityUpdated;
        EventHandler _onLanguageChanged;
        bool _textChanged;

        #endregion

        #region c-tor

        public IdentityControl()
        {
            InitializeComponent();
            //InitializeComponent("en-US");
        }

        public IdentityControl(EventHandler onIdentityUpdated, EventHandler onlanguageChanged)
        {
            InitializeComponent();
            //InitializeComponent("en-US");
            _onIdentityUpdated = onIdentityUpdated;
            _onLanguageChanged = onlanguageChanged;
        }

        #endregion

        #region event callbacks

        private void txtMyID_KeyDown(object sender, KeyEventArgs e)
        {
            int lengthBeforeUpdate = txtMyID.Text.Length;
            string stringBefore = txtMyID.Text;
            MessageBox.Show("Autogenerated property!", "Permission denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            int lengthAfterUpdate = txtMyID.Text.Length;
            if (lengthAfterUpdate > lengthBeforeUpdate)
            {
                txtMyID.Text = stringBefore;
            } 
            _textChanged = false;
        }

        private void txtFriendlyName_Leave(object sender, EventArgs e)
        {
            if (_textChanged)
            {
                // update the friendly name
                string newFriendlyName = txtFriendlyName.Text;
                _onIdentityUpdated.Invoke(this, new IdentityEventArgs()
                {
                    FriendlyName = newFriendlyName
                });
            }
            _textChanged = false;
        }

        private void txtFriendlyName_TextChanged(object sender, EventArgs e)
        {
            _textChanged = true;
        }

        #endregion

        #region private methods

        

        #endregion

        #region public methods

        public void UpdateFriendlyName(string newFriendlyName)
        {
            ControlCrossThreading.SetValue(txtFriendlyName, newFriendlyName, "Text");
            _textChanged = false;
        }

        public void UpdateMyID(string newID)
        {
            ControlCrossThreading.SetValue(txtMyID, newID, "Text");
        }

        #endregion

        private void lblLanguage_MouseClick(object sender, MouseEventArgs e)
        {
            if (lblLanguage.Text.ToLower().Equals(GenericEnums.Language.RO.ToString().ToLower()))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ro-RO");
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ro-RO");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            }
            this.InitializeComponent();
            this.Refresh();
            this.Update();
        }
    }
}
