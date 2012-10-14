namespace MViewer
{
    partial class FormVideoRoom
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.videoControl = new UIControls.VideoControl();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.videoControl);
            this.pnlMain.Location = new System.Drawing.Point(1, 1);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(411, 408);
            this.pnlMain.TabIndex = 0;
            // 
            // videoControl
            // 
            this.videoControl.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.videoControl.Location = new System.Drawing.Point(11, 11);
            this.videoControl.Name = "videoControl";
            this.videoControl.Size = new System.Drawing.Size(388, 384);
            this.videoControl.TabIndex = 1;
            // 
            // FormVideoRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 408);
            this.Controls.Add(this.pnlMain);
            this.Name = "FormVideoRoom";
            this.Text = "Video Chat";
            this.Resize += new System.EventHandler(this.FormVideoRoom_Resize);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private UIControls.VideoControl videoControl;
    }
}