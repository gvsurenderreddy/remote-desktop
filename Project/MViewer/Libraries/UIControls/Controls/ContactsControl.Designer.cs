namespace UIControls
{
    partial class ContactsControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.pnlContacts = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvContacts = new System.Windows.Forms.DataGridView();
            this.pnlContacts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(75, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Contacts";
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(8, 213);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(65, 213);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(61, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(128, 213);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(57, 23);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // pnlContacts
            // 
            this.pnlContacts.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pnlContacts.Controls.Add(this.dgvContacts);
            this.pnlContacts.Controls.Add(this.btnUpdate);
            this.pnlContacts.Controls.Add(this.btnAdd);
            this.pnlContacts.Controls.Add(this.btnRemove);
            this.pnlContacts.Location = new System.Drawing.Point(15, 28);
            this.pnlContacts.Name = "pnlContacts";
            this.pnlContacts.Size = new System.Drawing.Size(192, 243);
            this.pnlContacts.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Red;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(141, 277);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.ContactsControl_ClosePressed);
            // 
            // dgvContacts
            // 
            this.dgvContacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContacts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvContacts.Location = new System.Drawing.Point(0, 0);
            this.dgvContacts.MultiSelect = false;
            this.dgvContacts.Name = "dgvContacts";
            this.dgvContacts.ReadOnly = true;
            this.dgvContacts.RowHeadersVisible = false;
            this.dgvContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContacts.ShowEditingIcon = false;
            this.dgvContacts.Size = new System.Drawing.Size(192, 207);
            this.dgvContacts.TabIndex = 10;
            // 
            // ContactsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlContacts);
            this.Controls.Add(this.label1);
            this.Name = "ContactsControl";
            this.Size = new System.Drawing.Size(223, 305);
            this.pnlContacts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Panel pnlContacts;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvContacts;

    }
}
