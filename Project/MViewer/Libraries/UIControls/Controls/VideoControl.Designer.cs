namespace UIControls
{
    partial class VideoControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoControl));
            this.txtPartner = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.pnlVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.SuspendLayout();
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
            // pnlVideo
            // 
            this.pnlVideo.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlVideo.Controls.Add(this.pbVideo);
            resources.ApplyResources(this.pnlVideo, "pnlVideo");
            this.pnlVideo.Name = "pnlVideo";
            // 
            // pbVideo
            // 
            this.pbVideo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.pbVideo, "pbVideo");
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.TabStop = false;
            // 
            // VideoControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pnlVideo);
            this.Controls.Add(this.txtPartner);
            this.Controls.Add(this.lblUser);
            this.Name = "VideoControl";
            this.Resize += new System.EventHandler(this.VideoControl_Resize);
            this.pnlVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPartner;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel pnlVideo;
        private System.Windows.Forms.PictureBox pbVideo;

    }
}
