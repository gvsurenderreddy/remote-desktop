namespace VideoChatClient
{
    partial class FrmVideoChat
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
            this.btnStartVideo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.nudTimespan = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbServers = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStopAudio = new System.Windows.Forms.Button();
            this.btnStartAudio = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimespan)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartVideo
            // 
            this.btnStartVideo.Location = new System.Drawing.Point(12, 231);
            this.btnStartVideo.Name = "btnStartVideo";
            this.btnStartVideo.Size = new System.Drawing.Size(78, 23);
            this.btnStartVideo.TabIndex = 0;
            this.btnStartVideo.Text = "Start Video";
            this.btnStartVideo.UseVisualStyleBackColor = true;
            this.btnStartVideo.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(12, 260);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(78, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop Video";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // nudTimespan
            // 
            this.nudTimespan.Location = new System.Drawing.Point(299, 260);
            this.nudTimespan.Name = "nudTimespan";
            this.nudTimespan.Size = new System.Drawing.Size(52, 20);
            this.nudTimespan.TabIndex = 3;
            this.nudTimespan.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudTimespan.ValueChanged += new System.EventHandler(this.nudTimespan_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Video Capture timespan (ms)";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(96, 204);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(214, 20);
            this.txtServer.TabIndex = 5;
            this.txtServer.Text = "http://10.19.0.124:8081/WebcaptureService";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Server address";
            // 
            // lbServers
            // 
            this.lbServers.FormattingEnabled = true;
            this.lbServers.Location = new System.Drawing.Point(12, 12);
            this.lbServers.Name = "lbServers";
            this.lbServers.Size = new System.Drawing.Size(298, 186);
            this.lbServers.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(359, 260);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(78, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnStopAudio
            // 
            this.btnStopAudio.Location = new System.Drawing.Point(96, 259);
            this.btnStopAudio.Name = "btnStopAudio";
            this.btnStopAudio.Size = new System.Drawing.Size(78, 23);
            this.btnStopAudio.TabIndex = 10;
            this.btnStopAudio.Text = "Stop Audio";
            this.btnStopAudio.UseVisualStyleBackColor = true;
            // 
            // btnStartAudio
            // 
            this.btnStartAudio.Location = new System.Drawing.Point(96, 230);
            this.btnStartAudio.Name = "btnStartAudio";
            this.btnStartAudio.Size = new System.Drawing.Size(78, 23);
            this.btnStartAudio.TabIndex = 9;
            this.btnStartAudio.Text = "Start Audio";
            this.btnStartAudio.UseVisualStyleBackColor = true;
            // 
            // FrmVideoChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 293);
            this.Controls.Add(this.btnStopAudio);
            this.Controls.Add(this.btnStartAudio);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbServers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudTimespan);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStartVideo);
            this.Name = "FrmVideoChat";
            this.Text = "Video Chat";
            ((System.ComponentModel.ISupportInitialize)(this.nudTimespan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartVideo;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.NumericUpDown nudTimespan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbServers;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStopAudio;
        private System.Windows.Forms.Button btnStartAudio;
    }
}

