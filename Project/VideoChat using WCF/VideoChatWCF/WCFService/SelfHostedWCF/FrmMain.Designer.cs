namespace VideoChatWCF
{
    partial class FrmMain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.contactsControl = new ImageCommon.ContactsControl();
            this.actionsControl1 = new ImageCommon.ActionsControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contactsControl);
            this.panel1.Controls.Add(this.actionsControl1);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 365);
            this.panel1.TabIndex = 0;
            // 
            // contactsControl
            // 
            this.contactsControl.Location = new System.Drawing.Point(3, 7);
            this.contactsControl.Name = "contactsControl";
            this.contactsControl.Size = new System.Drawing.Size(305, 265);
            this.contactsControl.TabIndex = 4;
            // 
            // actionsControl1
            // 
            this.actionsControl1.Location = new System.Drawing.Point(11, 271);
            this.actionsControl1.Name = "actionsControl1";
            this.actionsControl1.Size = new System.Drawing.Size(297, 90);
            this.actionsControl1.TabIndex = 3;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 373);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMain";
            this.Text = "Conference";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ImageCommon.ContactsControl contactsControl;
        private ImageCommon.ActionsControl actionsControl1;




    }
}