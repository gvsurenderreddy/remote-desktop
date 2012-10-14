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
            this.pbWebcam = new System.Windows.Forms.PictureBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebcam)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbWebcam
            // 
            this.pbWebcam.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pbWebcam.Location = new System.Drawing.Point(12, 13);
            this.pbWebcam.Name = "pbWebcam";
            this.pbWebcam.Size = new System.Drawing.Size(360, 323);
            this.pbWebcam.TabIndex = 0;
            this.pbWebcam.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlMain.Controls.Add(this.pbWebcam);
            this.pnlMain.Location = new System.Drawing.Point(12, 12);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(388, 352);
            this.pnlMain.TabIndex = 1;
            // 
            // FormMyWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(412, 375);
            this.Controls.Add(this.pnlMain);
            this.Name = "FormMyWebcam";
            this.Text = "My Webcam";
            this.Resize += new System.EventHandler(this.FormMyWebcam_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbWebcam)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWebcam;
        private System.Windows.Forms.Panel pnlMain;
    }
}