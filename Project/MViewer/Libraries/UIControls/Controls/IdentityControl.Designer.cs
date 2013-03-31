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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IdentityControl));
            this.lblFriendlyName = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.txtMyID = new System.Windows.Forms.TextBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFriendlyName
            // 
            resources.ApplyResources(this.lblFriendlyName, "lblFriendlyName");
            this.lblFriendlyName.Name = "lblFriendlyName";
            // 
            // lblID
            // 
            resources.ApplyResources(this.lblID, "lblID");
            this.lblID.Name = "lblID";
            // 
            // txtFriendlyName
            // 
            resources.ApplyResources(this.txtFriendlyName, "txtFriendlyName");
            this.txtFriendlyName.Name = "txtFriendlyName";
            this.txtFriendlyName.TextChanged += new System.EventHandler(this.txtFriendlyName_TextChanged);
            this.txtFriendlyName.Leave += new System.EventHandler(this.txtFriendlyName_Leave);
            // 
            // txtMyID
            // 
            resources.ApplyResources(this.txtMyID, "txtMyID");
            this.txtMyID.Name = "txtMyID";
            this.txtMyID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMyID_KeyDown);
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblLanguage_MouseClick);
            // 
            // IdentityControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.txtMyID);
            this.Controls.Add(this.txtFriendlyName);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblFriendlyName);
            this.Name = "IdentityControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFriendlyName;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtFriendlyName;
        private System.Windows.Forms.TextBox txtMyID;
        private System.Windows.Forms.Label lblLanguage;
    }
}
