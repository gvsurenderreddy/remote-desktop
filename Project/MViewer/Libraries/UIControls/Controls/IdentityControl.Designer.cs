namespace UIControls
{
    partial class IdentityControl
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
            this.lblFriendlyName = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.txtMyID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblFriendlyName
            // 
            this.lblFriendlyName.AutoSize = true;
            this.lblFriendlyName.Location = new System.Drawing.Point(5, 5);
            this.lblFriendlyName.Name = "lblFriendlyName";
            this.lblFriendlyName.Size = new System.Drawing.Size(75, 13);
            this.lblFriendlyName.TabIndex = 0;
            this.lblFriendlyName.Text = "Friendly name:";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(42, 29);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(38, 13);
            this.lblID.TabIndex = 1;
            this.lblID.Text = "My ID:";
            // 
            // txtFriendlyName
            // 
            this.txtFriendlyName.Location = new System.Drawing.Point(86, 4);
            this.txtFriendlyName.Name = "txtFriendlyName";
            this.txtFriendlyName.Size = new System.Drawing.Size(193, 20);
            this.txtFriendlyName.TabIndex = 2;
            this.txtFriendlyName.Leave += new System.EventHandler(this.txtFriendlyName_Leave);
            // 
            // txtMyID
            // 
            this.txtMyID.Location = new System.Drawing.Point(86, 27);
            this.txtMyID.Name = "txtMyID";
            this.txtMyID.Size = new System.Drawing.Size(193, 20);
            this.txtMyID.TabIndex = 3;
            this.txtMyID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMyID_KeyDown);
            // 
            // IdentityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtMyID);
            this.Controls.Add(this.txtFriendlyName);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblFriendlyName);
            this.Name = "IdentityControl";
            this.Size = new System.Drawing.Size(282, 51);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFriendlyName;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtFriendlyName;
        private System.Windows.Forms.TextBox txtMyID;
    }
}
