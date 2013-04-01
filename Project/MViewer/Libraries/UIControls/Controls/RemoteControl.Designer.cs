namespace UIControls
{
    partial class RemoteControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteControl));
            this.pbRemote = new System.Windows.Forms.PictureBox();
            this.pnlRemote = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtPartner = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemote)).BeginInit();
            this.pnlRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbRemote
            // 
            this.pbRemote.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.pbRemote, "pbRemote");
            this.pbRemote.Name = "pbRemote";
            this.pbRemote.TabStop = false;
            // 
            // pnlRemote
            // 
            this.pnlRemote.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlRemote.Controls.Add(this.pbRemote);
            resources.ApplyResources(this.pnlRemote, "pnlRemote");
            this.pnlRemote.Name = "pnlRemote";
            // 
            // lblUser
            // 
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblUser.Name = "lblUser";
            // 
            // txtPartner
            // 
            resources.ApplyResources(this.txtPartner, "txtPartner");
            this.txtPartner.Name = "txtPartner";
            // 
            // RemoteControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.txtPartner);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.pnlRemote);
            this.Name = "RemoteControl";
            this.Resize += new System.EventHandler(this.RemoteControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbRemote)).EndInit();
            this.pnlRemote.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbRemote;
        private System.Windows.Forms.Panel pnlRemote;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtPartner;
    }
}
