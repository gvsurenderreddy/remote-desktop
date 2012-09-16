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

        #endregion

        #region private static methods

        static void SetProperty(Control control, string propertyName, object value)
        {
            foreach (PropertyInfo property in control.GetType().GetProperties())
            {
                if (property.Name.ToLower() == propertyName.ToLower())
                {
                    control.Invoke(new MethodInvoker(delegate
                    {
                        property.SetValue(control, value, null);
                    }
                        ));
                }
            }
        }

        #endregion
    }
}
