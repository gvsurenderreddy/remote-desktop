//====================================================================
// Copyright (C) 2006 Bernad Pakpahan. All rights reserved.
// Email me at bern4d@gmail.com
//====================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using XMLData.Bussines;

namespace XMLData
{
    public partial class Form1 : Form
    {
        Category cat;
        public Form1()
        {
            InitializeComponent();
        }
        void populate()
        {
            IList list = CategoryList.GetCategoryList();
            this.dataGrid1.DataSource = list;
        }
        void loadData(string item)
        {
            cat = CategoryList.GetCategory(item);
            if (cat != null)
            {
                this.txtCode.Text = cat.CategoryID;
                this.txtDesc.Text = cat.CategoryName;
            }
        }
        private void frmCategory_Load(object sender, System.EventArgs e)
        {
            populate();
        }

        private void dataGrid1_Click(object sender, System.EventArgs e)
        {
            loadData(dataGrid1[this.dataGrid1.CurrentRowIndex, 0].ToString());
        }
        private void IsAlreadyExist()
        {
            cat = CategoryList.GetCategory(this.txtCode.Text);
            if (cat != null)
            {
                strAction = "update";
            }
            else
            {
                strAction = "insert";
            }

        }
        private bool isValidate()
        {
            if (txtCode.Text != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void clearForm()
        {
            this.txtCode.Text = "";
            this.txtDesc.Text = "";
        }
 
        public void CancelData()
        {
            clearForm();
        }
        public void CloseForm()
        {
            this.Close();
        }
        public void DeleteData()
        {
            try
            {
                DialogResult rslt = MessageBox.Show("Are sure want to delete this record no : " +
                    txtCode.Text + " ?", "[Confirmation]", MessageBoxButtons.YesNo);
                if (rslt == DialogResult.Yes)
                {
                    string item;
                    item = txtCode.Text;
                    CategoryList.DeleteCategory(item);
                    bResult = true;
                }
            }
            catch (Exception exp)
            {
                bResult = false;
                strExp = "Record " + txtCode.Text.Trim() + " failed delete to datasource\n Message : " + exp.Message;
                MessageBox.Show(strExp, "[Status Dialog]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            populate();
            clearForm();

        }
        public void NewEntry()
        {
            clearForm();
            this.txtCode.Focus();

        }
        public void PrintReport()
        {
        }
        public void RefreshData()
        {
            clearForm();
            populate();
        }
        public void SaveOrUpdateData()
        {
            if (isValidate())
            {
                IsAlreadyExist();
                SaveOrUpdateAction();
                if (!bResult)
                {
                    MessageBox.Show(strExp, "[Modified Dialog]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show(strExp, "[Modified Dialog]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        public void SaveOrUpdateAction()
        {
            cat = new Category();
            cat.CategoryID = this.txtCode.Text;
            cat.CategoryName = this.txtDesc.Text;
            if (strAction.Equals("insert"))
            {
                try
                {
                    CategoryList.InsertCategory(cat);
                    bResult = true;
                    strExp = "Record " + cat.CategoryID.Trim() + " successfully insert to datasource";
                }
                catch (Exception exp)
                {
                    bResult = false;
                    strExp = "Record " + cat.CategoryID.Trim() + " failed insert to datasource\n Message : " + exp.Message;
                }
            }
            else
            {
                try
                {
                    CategoryList.UpdateCategory(cat);
                    bResult = true;
                    strExp = "Record " + cat.CategoryID.Trim() + " successfully update to datasource";
                }
                catch (Exception exp)
                {
                    bResult = false;
                    strExp = "Record " + cat.CategoryID.Trim() + " failed update to datasource\n Message : " + exp.Message;
                }
            }
            populate();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            NewEntry();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveOrUpdateData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

    }
}