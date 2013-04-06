using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Utils
{
    public class ControlCrossThreading
    {
        #region public methods

        public void GetValue(Control control, string propertyName, ref object value)
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

        public void SetValue(Control control, object value, string propertyName)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetGridViewColumnPropery(DataGridView dataGridView, string columnName, object value, string propertyName)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        void GetProperty(Control control, string propertyName, ref object value)
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

        void SetProperty(Control control, string propertyName, object value)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void SetDataGridViewColumnProperty(DataGridView dataGridView, string columnName, object value, string propertyName)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
