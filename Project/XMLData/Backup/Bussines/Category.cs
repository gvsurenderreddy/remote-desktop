//====================================================================
// Copyright (C) 2006 Bernad Pakpahan. All rights reserved.
// Email me at bern4d@gmail.com
//====================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using XMLData.DAL;
using System.Data;

namespace XMLData.Bussines
{
    /// <summary>
    /// Summary description for Category.
    /// </summary>
    public class Category
    {

        private string categoryID;
        private string categoryName;
        public Category()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
    }
}
    public class CategoryList
    {
        public static Category GetCategory(string categoryID)
        {
            DataRow iDr = null;
            iDr = XMLCategory.Select(categoryID);
            Category cat = null;
            if (iDr != null)
            {
                cat = new Category();
                cat.CategoryID = iDr[0] != DBNull.Value ? iDr[0].ToString() : string.Empty; ;
                cat.CategoryName = iDr[1] != DBNull.Value ? iDr[1].ToString() : string.Empty;
            }
            return cat;
        }
        public static IList GetCategoryList()
        {

            return XMLCategory.SelectAll();

        }

        public static void UpdateCategory(Category cat)
        {
            XMLCategory.Update(cat.CategoryID, cat.CategoryName);
        }

        public static void InsertCategory(Category cat)
        {
            XMLCategory.Insert(cat.CategoryID, cat.CategoryName);
        }

        public static void DeleteCategory(string categoryID)
        {
            XMLCategory.Delete(categoryID);
        }
    }

}