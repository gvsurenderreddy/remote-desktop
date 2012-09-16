namespace MViewer
{
    partial class FormActions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormActions));
            this.actionsControl1 = new UIControls.ActionsControl();
            this.SuspendLayout();
            // 
            // actionsControl1
            // 
            this.actionsControl1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.actionsControl1.Location = new System.Drawing.Point(0, 0);
            this.actionsControl1.Name = "actionsControl1";
            this.actionsControl1.Size = new System.Drawing.Size(314, 91);
            this.actionsControl1.TabIndex = 0;
            // 
            // FormActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 90);
            this.Controls.Add(this.actionsControl1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormActions";
            this.ShowInTaskbar = false;
            this.Text = "Actions";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormActions_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private UIControls.ActionsControl actionsControl1;
    }
}