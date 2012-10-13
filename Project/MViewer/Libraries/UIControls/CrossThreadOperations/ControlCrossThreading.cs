using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace UIControls.CrossThreadOperations
{
    public static class ControlCrossThreading
    {
        #region public static methods

        public static void GetValue(Control control, string propertyName, ref object value)
        {
            if (control.InvokeRequired)
            {
                GetProperty(control, propertyName, ref value);
            }
            else
            {
                GetProperty(control, propertyName, ref value);
            }
        }


        public static void SetValue(Control control, object value, string propertyName)
        {
            if (control.InvokeRequired)
            {
                SetProperty(control, propertyName, value);
            }
            else
            {
                SetProperty(control, propertyName, value);
            }
        }

        public static void SetGridViewColumnPropery(DataGridView dataGridView, string columnName, object value, string propertyName)
        {
            if (dataGridView.InvokeRequired)
            {
                SetDataGridViewColumnProperty(dataGridView, columnName, value, propertyName);
            }
            else
            {
                SetDataGridViewColumnProperty(dataGridView, columnName, value, propertyName);
            }
        }

        #endregion

        #region private static methods

        static void GetProperty(Control control, string propertyName, ref object value)
        {
            object value2 = null;
            foreach (PropertyInfo property in control.GetType().GetProperties())
            {
                if (property.Name.ToLower() == propertyName.ToLower())
                {
                    control.Invoke
                    (
                        new MethodInvoker
                        (
                            delegate
                            {
                                value2 = property.GetValue(control, null);
                            }
                        )
                    );
                }
            }
            value = value2;
        }

        static void SetProperty(Control control, string propertyName, object value)
        {
            foreach (PropertyInfo property in control.GetType().GetProperties())
            {
                if (property.Name.ToLower() == propertyName.ToLower())
                {
                    control.Invoke
                    (
                        new MethodInvoker
                        (
                            delegate
                            {
                                property.SetValue(control, value, null);
                            }
                        )
                    );
                }
            }
        }

        static void SetDataGridViewColumnProperty(DataGridView dataGridView, string columnName, object value, string propertyName)
        {
            foreach (PropertyInfo property in dataGridView.Columns[columnName].GetType().GetProperties())
            {
                if (property.Name.ToLower() == propertyName.ToLower())
                {
                    dataGridView.Invoke
                    (
                        new MethodInvoker
                        (
                            delegate
                            {
                                property.SetValue(dataGridView.Columns[columnName], value, null);
                            }
                        )
                    );
                }
            }
        }

        #endregion
    }
}
