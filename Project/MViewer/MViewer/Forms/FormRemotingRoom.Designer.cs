namespace MViewer
{
    partial class FormRemotingRoom 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemotingRoom));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.remoteControl = new UIControls.RemoteControl();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.remoteControl);
            this.pnlMain.Location = new System.Drawing.Point(0, 1);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(573, 442);
            this.pnlMain.TabIndex = 1;
            // 
            // remoteControl
            // 
            this.remoteControl.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.remoteControl.Location = new System.Drawing.Point(12, 11);
            this.remoteControl.Name = "remoteControl";
            this.remoteControl.Size = new System.Drawing.Size(547, 416);
            this.remoteControl.TabIndex = 0;
            // 
            // FormRemotingRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 445);
            this.Controls.Add(this.pnlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRemotingRoom";
            this.Text = "Remoting Room";
            this.Activated += new System.EventHandler(this.FormRemotingRoom_Activated);
            this.Deactivate += new System.EventHandler(this.FormRemotingRoom_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRemotingRoom_FormClosing);
            this.Load += new System.EventHandler(this.FormRemotingRoom_Load);
            this.Resize += new System.EventHandler(this.FormRemotingRoom_Resize);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UIControls.RemoteControl remoteControl;
        private System.Windows.Forms.Panel pnlMain;
    }
}