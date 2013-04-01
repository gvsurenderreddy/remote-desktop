namespace UIControls
{
    partial class ActionsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActionsControl));
            this.btnVideo = new System.Windows.Forms.Button();
            this.btnAudio = new System.Windows.Forms.Button();
            this.btnRemote = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMuteAudio = new System.Windows.Forms.Button();
            this.btnPauseVideo = new System.Windows.Forms.Button();
            this.btnPauseRemote = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVideo
            // 
            resources.ApplyResources(this.btnVideo, "btnVideo");
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.UseVisualStyleBackColor = true;
            this.btnVideo.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnAudio
            // 
            resources.ApplyResources(this.btnAudio, "btnAudio");
            this.btnAudio.Name = "btnAudio";
            this.btnAudio.UseVisualStyleBackColor = true;
            this.btnAudio.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnRemote
            // 
            resources.ApplyResources(this.btnRemote, "btnRemote");
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.UseVisualStyleBackColor = true;
            this.btnRemote.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Name = "label3";
            // 
            // btnMuteAudio
            // 
            resources.ApplyResources(this.btnMuteAudio, "btnMuteAudio");
            this.btnMuteAudio.Name = "btnMuteAudio";
            this.btnMuteAudio.UseVisualStyleBackColor = true;
            this.btnMuteAudio.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnPauseVideo
            // 
            resources.ApplyResources(this.btnPauseVideo, "btnPauseVideo");
            this.btnPauseVideo.Name = "btnPauseVideo";
            this.btnPauseVideo.UseVisualStyleBackColor = true;
            this.btnPauseVideo.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnPauseRemote
            // 
            resources.ApplyResources(this.btnPauseRemote, "btnPauseRemote");
            this.btnPauseRemote.Name = "btnPauseRemote";
            this.btnPauseRemote.UseVisualStyleBackColor = true;
            this.btnPauseRemote.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnSend
            // 
            resources.ApplyResources(this.btnSend, "btnSend");
            this.btnSend.Name = "btnSend";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label4.Name = "label4";
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlControls.Controls.Add(this.label1);
            this.pnlControls.Controls.Add(this.btnVideo);
            this.pnlControls.Controls.Add(this.label4);
            this.pnlControls.Controls.Add(this.btnAudio);
            this.pnlControls.Controls.Add(this.btnSend);
            this.pnlControls.Controls.Add(this.btnRemote);
            this.pnlControls.Controls.Add(this.btnPauseRemote);
            this.pnlControls.Controls.Add(this.label2);
            this.pnlControls.Controls.Add(this.btnPauseVideo);
            this.pnlControls.Controls.Add(this.label3);
            this.pnlControls.Controls.Add(this.btnMuteAudio);
            resources.ApplyResources(this.pnlControls, "pnlControls");
            this.pnlControls.Name = "pnlControls";
            // 
            // ActionsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pnlControls);
            this.Name = "ActionsControl";
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVideo;
        private System.Windows.Forms.Button btnAudio;
        private System.Windows.Forms.Button btnRemote;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMuteAudio;
        private System.Windows.Forms.Button btnPauseVideo;
        private System.Windows.Forms.Button btnPauseRemote;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlControls;
    }
}
