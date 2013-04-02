using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class GenericMethods
    {
        public void ChangeLanguage(string lang, Control.ControlCollection controls, Type controlType)
        {
            foreach (Control c in controls)
            {
                ComponentResourceManager resources = new ComponentResourceManager(controlType);
                CultureInfo cultureInfo = new CultureInfo(lang);
                if (c.InvokeRequired)
                {
                    c.Invoke(new MethodInvoker(delegate()
                    {
                        resources.ApplyResources(c, c.Name, cultureInfo);
                    }));
                }
                else
                {
                    resources.ApplyResources(c, c.Name, cultureInfo);
                }
                try
                {
                    if (c.Controls != null && c.Controls.Count > 0)
                    {
                        ChangeLanguage(lang, c.Controls, controlType);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
