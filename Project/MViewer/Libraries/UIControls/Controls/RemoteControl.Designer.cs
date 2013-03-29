namespace UIControls
{
    partial class RemoteControl
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
            this.pbRemote = new System.Windows.Forms.PictureBox();
            this.pnlRemote = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtPartner = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemote)).BeginInit();
            this.pnlRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbRemote
            // 
            this.pbRemote.BackColor = System.Drawing.SystemColors.Control;
            this.pbRemote.Location = new System.Drawing.Point(12, 14);
            this.pbRemote.Name = "pbRemote";
            this.pbRemote.Size = new System.Drawing.Size(514, 349);
            this.pbRemote.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRemote.TabIndex = 1;
            this.pbRemote.TabStop = false;
            // 
            // pnlRemote
            // 
            this.pnlRemote.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlRemote.Controls.Add(this.pbRemote);
            this.pnlRemote.Location = new System.Drawing.Point(3, 40);
            this.pnlRemote.Name = "pnlRemote";
            this.pnlRemote.Size = new System.Drawing.Size(539, 373);
            this.pnlRemote.TabIndex = 2;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblUser.Location = new System.Drawing.Point(14, 15);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(66, 16);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "Partner: ";
            // 
            // txtPartner
            // 
            this.txtPartner.Enabled = false;
            this.txtPartner.Location = new System.Drawing.Point(86, 11);
            this.txtPartner.Name = "txtPartner";
            this.txtPartner.Size = new System.Drawing.Size(112, 20);
            this.txtPartner.TabIndex = 4;
            // 
            // RemoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.txtPartner);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.pnlRemote);
            this.Name = "RemoteControl";
            this.Size = new System.Drawing.Size(547, 416);
            this.Resize += new System.EventHandler(this.RemoteControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbRemote)).EndInit();
            this.pnlRemote.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbRemote;
        private System.Windows.Forms.Panel pnlRemote;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtPartner;
    }
}
