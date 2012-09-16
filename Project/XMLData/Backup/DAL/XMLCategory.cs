//====================================================================
// Copyright (C) 2006 Bernad Pakpahan. All rights reserved.
// Email me at bern4d@gmail.com
//====================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace XMLData.DAL
{
    public sealed class XMLCategory
    {
        private XMLCategory() { }
        static DataSet ds = new DataSet();
        static DataView dv = new DataView();
        /// <summary>
        /// Inserts a record into the Category table.
        /// </summary>
        /// 
        public static void save()
        {
            ds.WriteXml(Application.StartupPath + "\\XML\\Category.xml", XmlWriteMode.WriteSchema);
        }
        public static void Insert(string categoryID, string CategoryName)
        {
            DataRow dr = dv.Table.NewRow();
            dr[0] = categoryID;
            dr[1] = CategoryName;
            dv.Table.Rows.Add(dr);
            save();
        }

        /// <summary>
        /// Updates a record in the Category table.
        /// </summary>
        public static void Update(string categoryID, string CategoryName)
        {
            DataRow dr = Select(categoryID);
            dr[1] = CategoryName;
            save();
        }

        /// <summary>
        /// Deletes a record from the Category table by a composite primary key.
        /// </summary>
        public static void Delete(string categoryID)
        {
            dv.RowFilter = "categoryID='" + categoryID + "'";
            dv.Sort = "categoryID";
            dv.Delete(0);
            dv.RowFilter = "";
            save();
        }

        /// <summary>
        /// Selects a single record from the Category table.
        /// </summary>
        public static DataRow Select(string categoryID)
        {
            dv.RowFilter = "categoryID='" + categoryID + "'";
            dv.Sort = "categoryID";
            DataRow dr = null;
            if (dv.Count > 0)
            {
                dr = dv[0].Row;
            }
            dv.RowFilter = "";
            return dr;
        }

        /// <summary>
        /// Selects all records from the Category table.
        /// </summary>
        public static DataView SelectAll()
        {
            ds.Clear();
            ds.ReadXml(Application.StartupPath + "\\XML\\Category.xml", XmlReadMode.ReadSchema);
            dv = ds.Tables[0].DefaultView;
            return dv;
        }
    }
}