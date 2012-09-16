namespace MViewer
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
            this.identityControl1 = new UIControls.IdentityControl();
            this.contactsControl = new UIControls.ContactsControl(this.ContactsControl_ClosePressed);
            this.SuspendLayout();
            // 
            // identityControl1
            // 
            this.identityControl1.Location = new System.Drawing.Point(3, 0);
            this.identityControl1.Name = "identityControl1";
            this.identityControl1.Size = new System.Drawing.Size(282, 51);
            this.identityControl1.TabIndex = 1;
            // 
            // contactsControl
            // 
            this.contactsControl.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.contactsControl.Location = new System.Drawing.Point(288, 0);
            this.contactsControl.Name = "contactsControl";
            this.contactsControl.Size = new System.Drawing.Size(223, 305);
            this.contactsControl.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(510, 305);
            this.Controls.Add(this.identityControl1);
            this.Controls.Add(this.contactsControl);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private UIControls.ContactsControl contactsControl;
        private UIControls.IdentityControl identityControl1;



    }
}

