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
            this.txtStatus.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtStatus.Enabled = false;
            this.txtStatus.ForeColor = System.Drawing.Color.Red;
            this.txtStatus.Location = new System.Drawing.Point(76, 3);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(112, 20);
            this.txtStatus.TabIndex = 0;
            this.txtStatus.Text = "Mute";
            this.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPartner
            // 
            this.txtPartner.Enabled = false;
            this.txtPartner.Location = new System.Drawing.Point(82, 7);
            this.txtPartner.Name = "txtPartner";
            this.txtPartner.Size = new System.Drawing.Size(112, 20);
            this.txtPartner.TabIndex = 8;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(10, 8);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(66, 16);
            this.lblUser.TabIndex = 7;
            this.lblUser.Text = "Partner: ";
            // 
            // lblAudioStatus
            // 
            this.lblAudioStatus.AutoSize = true;
            this.lblAudioStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAudioStatus.Location = new System.Drawing.Point(4, 3);
            this.lblAudioStatus.Name = "lblAudioStatus";
            this.lblAudioStatus.Size = new System.Drawing.Size(59, 16);
            this.lblAudioStatus.TabIndex = 9;
            this.lblAudioStatus.Text = "Status: ";
            // 
            // pnlAudio
            // 
            this.pnlAudio.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlAudio.Controls.Add(this.txtStatus);
            this.pnlAudio.Controls.Add(this.lblAudioStatus);
            this.pnlAudio.Location = new System.Drawing.Point(6, 33);
            this.pnlAudio.Name = "pnlAudio";
            this.pnlAudio.Size = new System.Drawing.Size(191, 28);
            this.pnlAudio.TabIndex = 10;
            // 
            // AudioControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pnlAudio);
            this.Controls.Add(this.txtPartner);
            this.Controls.Add(this.lblUser);
            this.Name = "AudioControl";
            this.Size = new System.Drawing.Size(203, 69);
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
