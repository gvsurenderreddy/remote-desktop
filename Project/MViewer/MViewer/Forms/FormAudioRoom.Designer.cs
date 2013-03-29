namespace MViewer.Forms
{
    partial class FormAudioRoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudioRoom));
            this.audioControl = new UIControls.AudioControl();
            this.SuspendLayout();
            // 
            // audioControl
            // 
            this.audioControl.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.audioControl.Location = new System.Drawing.Point(12, 12);
            this.audioControl.Name = "audioControl";
            this.audioControl.Size = new System.Drawing.Size(203, 69);
            this.audioControl.TabIndex = 0;
            // 
            // FormAudioRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 93);
            this.Controls.Add(this.audioControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAudioRoom";
            this.Text = "Audio Room";
            this.ResumeLayout(false);

        }

        #endregion

        private UIControls.AudioControl audioControl;
    }
}