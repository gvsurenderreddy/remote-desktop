namespace MViewer
{
    partial class FormConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguration));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.nudVideo = new System.Windows.Forms.NumericUpDown();
            this.nudRemoting = new System.Windows.Forms.NumericUpDown();
            this.nudAudio = new System.Windows.Forms.NumericUpDown();
            this.cbxPrivate = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemoting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAudio)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Video Freq";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Remoting freq";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Audio Freq";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 177);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(120, 177);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(95, 10);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 7;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(95, 39);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 8;
            // 
            // nudVideo
            // 
            this.nudVideo.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudVideo.Location = new System.Drawing.Point(95, 68);
            this.nudVideo.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudVideo.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudVideo.Name = "nudVideo";
            this.nudVideo.Size = new System.Drawing.Size(100, 20);
            this.nudVideo.TabIndex = 9;
            this.nudVideo.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nudRemoting
            // 
            this.nudRemoting.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudRemoting.Location = new System.Drawing.Point(95, 94);
            this.nudRemoting.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.nudRemoting.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudRemoting.Name = "nudRemoting";
            this.nudRemoting.Size = new System.Drawing.Size(100, 20);
            this.nudRemoting.TabIndex = 10;
            this.nudRemoting.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // nudAudio
            // 
            this.nudAudio.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudAudio.Location = new System.Drawing.Point(95, 122);
            this.nudAudio.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudAudio.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudAudio.Name = "nudAudio";
            this.nudAudio.Size = new System.Drawing.Size(100, 20);
            this.nudAudio.TabIndex = 11;
            this.nudAudio.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // cbxPrivate
            // 
            this.cbxPrivate.AutoSize = true;
            this.cbxPrivate.Checked = true;
            this.cbxPrivate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxPrivate.Location = new System.Drawing.Point(95, 152);
            this.cbxPrivate.Name = "cbxPrivate";
            this.cbxPrivate.Size = new System.Drawing.Size(59, 17);
            this.cbxPrivate.TabIndex = 12;
            this.cbxPrivate.Text = "Private";
            this.cbxPrivate.UseVisualStyleBackColor = true;
            this.cbxPrivate.CheckedChanged += new System.EventHandler(this.cbxPrivate_CheckedChanged);
            // 
            // FormConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 212);
            this.Controls.Add(this.cbxPrivate);
            this.Controls.Add(this.nudAudio);
            this.Controls.Add(this.nudRemoting);
            this.Controls.Add(this.nudVideo);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfiguration";
            this.ShowInTaskbar = false;
            this.Text = "Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.nudVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemoting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAudio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.NumericUpDown nudVideo;
        private System.Windows.Forms.NumericUpDown nudRemoting;
        private System.Windows.Forms.NumericUpDown nudAudio;
        private System.Windows.Forms.CheckBox cbxPrivate;
    }
}