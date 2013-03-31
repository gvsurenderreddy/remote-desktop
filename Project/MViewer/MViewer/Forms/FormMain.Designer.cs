﻿namespace MViewer
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.identityControl = new UIControls.IdentityControl(this.IdentityUpdated);
            this.contactsControl = new UIControls.ContactsControl(this.OnContactsControl_ClosePressed, this.OnContactsUpdated, this.OnSelectedContactChanged);
            this.SuspendLayout();
            // 
            // identityControl
            // 
            this.identityControl.Location = new System.Drawing.Point(3, 9);
            this.identityControl.Name = "identityControl";
            this.identityControl.Size = new System.Drawing.Size(282, 51);
            this.identityControl.TabIndex = 0;

            this.lblActionResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblActionResult
            // 
            this.lblActionResult.AutoSize = true;
            this.lblActionResult.Location = new System.Drawing.Point(12, 283);
            this.lblActionResult.Name = "lblActionResult";
            this.lblActionResult.Size = new System.Drawing.Size(0, 13);
            this.lblActionResult.TabIndex = 0;
            this.lblActionResult.Visible = false;
            // 
            // contactsControl
            // 
            this.contactsControl.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.contactsControl.Location = new System.Drawing.Point(291, 0);
            this.contactsControl.Name = "contactsControl";
            this.contactsControl.Size = new System.Drawing.Size(223, 305);
            this.contactsControl.TabIndex = 1;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(510, 305);
            this.Controls.Add(this.contactsControl);
            this.Controls.Add(this.identityControl);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
            this.ResumeLayout(false);
            this.Activated += new System.EventHandler(this.FormMain_Activated);

        }

        #endregion

        private UIControls.IdentityControl identityControl;
        private UIControls.ContactsControl contactsControl;
        private System.Windows.Forms.Label lblActionResult;
    }
}

