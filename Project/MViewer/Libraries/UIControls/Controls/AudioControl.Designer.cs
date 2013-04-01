namespace UIControls
{
    partial class AudioControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioControl));
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtPartner = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblAudioStatus = new System.Windows.Forms.Label();
            this.pnlAudio = new System.Windows.Forms.Panel();
            this.pnlAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            resources.ApplyResources(this.txtStatus, "txtStatus");
            this.txtStatus.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtStatus.ForeColor = System.Drawing.Color.Red;
            this.txtStatus.Name = "txtStatus";
            // 
            // txtPartner
            // 
            resources.ApplyResources(this.txtPartner, "txtPartner");
            this.txtPartner.Name = "txtPartner";
            // 
            // lblUser
            // 
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblUser.Name = "lblUser";
            // 
            // lblAudioStatus
            // 
            resources.ApplyResources(this.lblAudioStatus, "lblAudioStatus");
            this.lblAudioStatus.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblAudioStatus.Name = "lblAudioStatus";
            // 
            // pnlAudio
            // 
            resources.ApplyResources(this.pnlAudio, "pnlAudio");
            this.pnlAudio.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlAudio.Controls.Add(this.txtStatus);
            this.pnlAudio.Controls.Add(this.lblAudioStatus);
            this.pnlAudio.Name = "pnlAudio";
            // 
            // AudioControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pnlAudio);
            this.Controls.Add(this.txtPartner);
            this.Controls.Add(this.lblUser);
            this.Name = "AudioControl";
            this.pnlAudio.ResumeLayout(false);
            this.pnlAudio.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtPartner;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblAudioStatus;
        private System.Windows.Forms.Panel pnlAudio;
    }
}
