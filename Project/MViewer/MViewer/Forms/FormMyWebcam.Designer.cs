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
            ((System.ComponentModel.ISupportInitialize)(this.pbWebcam)).BeginInit();
            this.SuspendLayout();
            // 
            // pbWebcam
            // 
            this.pbWebcam.Location = new System.Drawing.Point(1, 2);
            this.pbWebcam.Name = "pbWebcam";
            this.pbWebcam.Size = new System.Drawing.Size(354, 311);
            this.pbWebcam.TabIndex = 0;
            this.pbWebcam.TabStop = false;
            // 
            // FormMyWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 315);
            this.Controls.Add(this.pbWebcam);
            this.Name = "FormMyWebcam";
            this.Text = "My Webcam";
            ((System.ComponentModel.ISupportInitialize)(this.pbWebcam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWebcam;
    }
}