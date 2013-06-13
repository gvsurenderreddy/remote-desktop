namespace MViewer
{
    partial class FormMyWebcam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMyWebcam));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pbWebcam = new System.Windows.Forms.PictureBox();
            this.cbxWebcamStatus = new System.Windows.Forms.CheckBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebcam)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlMain.Controls.Add(this.cbxWebcamStatus);
            this.pnlMain.Controls.Add(this.pbWebcam);
            this.pnlMain.Location = new System.Drawing.Point(12, 12);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(388, 352);
            this.pnlMain.TabIndex = 1;
            // 
            // pbWebcam
            // 
            this.pbWebcam.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pbWebcam.Image = global::MViewer.Properties.Resources.closed_web_camera;
            this.pbWebcam.Location = new System.Drawing.Point(12, 26);
            this.pbWebcam.Name = "pbWebcam";
            this.pbWebcam.Size = new System.Drawing.Size(360, 323);
            this.pbWebcam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbWebcam.TabIndex = 0;
            this.pbWebcam.TabStop = false;
            // 
            // cbxWebcamStatus
            // 
            this.cbxWebcamStatus.AutoSize = true;
            this.cbxWebcamStatus.Checked = true;
            this.cbxWebcamStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxWebcamStatus.Location = new System.Drawing.Point(12, 3);
            this.cbxWebcamStatus.Name = "cbxWebcamStatus";
            this.cbxWebcamStatus.Size = new System.Drawing.Size(58, 17);
            this.cbxWebcamStatus.TabIndex = 1;
            this.cbxWebcamStatus.Text = "Closed";
            this.cbxWebcamStatus.UseVisualStyleBackColor = true;
            this.cbxWebcamStatus.CheckedChanged += new System.EventHandler(this.cbxWebcamStatus_CheckedChanged);
            // 
            // FormMyWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(412, 375);
            this.Controls.Add(this.pnlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMyWebcam";
            this.Text = "My Webcam";
            this.Resize += new System.EventHandler(this.FormMyWebcam_Resize);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebcam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWebcam;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.CheckBox cbxWebcamStatus;
    }
}