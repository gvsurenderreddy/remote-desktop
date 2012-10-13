namespace GenericData
{
    partial class FrmVideoChatRoom
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
            this.pbChatRoom = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbChatRoom)).BeginInit();
            this.SuspendLayout();
            // 
            // pbChatRoom
            // 
            this.pbChatRoom.Location = new System.Drawing.Point(12, 12);
            this.pbChatRoom.Name = "pbChatRoom";
            this.pbChatRoom.Size = new System.Drawing.Size(341, 297);
            this.pbChatRoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbChatRoom.TabIndex = 0;
            this.pbChatRoom.TabStop = false;
            // 
            // FrmVideoChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 362);
            this.Controls.Add(this.pbChatRoom);
            this.Name = "FrmVideoChatRoom";
            this.Text = "Video Chat Room";
            this.Resize += new System.EventHandler(this.FrmVideoChatRoom_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbChatRoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbChatRoom;
    }
}