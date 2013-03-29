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
            this.btnVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVideo.Location = new System.Drawing.Point(6, 21);
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.Size = new System.Drawing.Size(66, 23);
            this.btnVideo.TabIndex = 0;
            this.btnVideo.Text = "Start";
            this.btnVideo.UseVisualStyleBackColor = true;
            this.btnVideo.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnAudio
            // 
            this.btnAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAudio.Location = new System.Drawing.Point(78, 21);
            this.btnAudio.Name = "btnAudio";
            this.btnAudio.Size = new System.Drawing.Size(66, 23);
            this.btnAudio.TabIndex = 1;
            this.btnAudio.Text = "Start";
            this.btnAudio.UseVisualStyleBackColor = true;
            this.btnAudio.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnRemote
            // 
            this.btnRemote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemote.Location = new System.Drawing.Point(150, 21);
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.Size = new System.Drawing.Size(66, 23);
            this.btnRemote.TabIndex = 2;
            this.btnRemote.Text = "Start";
            this.btnRemote.UseVisualStyleBackColor = true;
            this.btnRemote.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(23, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Video";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(93, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Audio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(159, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Remote";
            // 
            // btnMuteAudio
            // 
            this.btnMuteAudio.Location = new System.Drawing.Point(78, 50);
            this.btnMuteAudio.Name = "btnMuteAudio";
            this.btnMuteAudio.Size = new System.Drawing.Size(66, 23);
            this.btnMuteAudio.TabIndex = 6;
            this.btnMuteAudio.Text = "Mute";
            this.btnMuteAudio.UseVisualStyleBackColor = true;
            this.btnMuteAudio.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnPauseVideo
            // 
            this.btnPauseVideo.Location = new System.Drawing.Point(6, 50);
            this.btnPauseVideo.Name = "btnPauseVideo";
            this.btnPauseVideo.Size = new System.Drawing.Size(66, 23);
            this.btnPauseVideo.TabIndex = 7;
            this.btnPauseVideo.Text = "Pause";
            this.btnPauseVideo.UseVisualStyleBackColor = true;
            this.btnPauseVideo.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnPauseRemote
            // 
            this.btnPauseRemote.Location = new System.Drawing.Point(150, 50);
            this.btnPauseRemote.Name = "btnPauseRemote";
            this.btnPauseRemote.Size = new System.Drawing.Size(66, 23);
            this.btnPauseRemote.TabIndex = 8;
            this.btnPauseRemote.Text = "Pause";
            this.btnPauseRemote.UseVisualStyleBackColor = true;
            this.btnPauseRemote.Click += new System.EventHandler(this.BtnAction_Click);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(227, 21);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(66, 23);
            this.btnSend.TabIndex = 9;
            this.btnSend.Text = "Send file";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label4.Location = new System.Drawing.Point(236, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Transfer";
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
            this.pnlControls.Location = new System.Drawing.Point(7, 7);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(300, 78);
            this.pnlControls.TabIndex = 12;
            // 
            // ActionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pnlControls);
            this.Name = "ActionsControl";
            this.Size = new System.Drawing.Size(314, 91);
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
