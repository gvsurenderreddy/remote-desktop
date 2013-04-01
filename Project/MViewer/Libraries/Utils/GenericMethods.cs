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
                resources.ApplyResources(c, c.Name, new CultureInfo(lang));
                if (c.Controls != null && c.Controls.Count > 0)
                {
                    ChangeLanguage(lang, c.Controls, c.GetType());
                }
            }
        }
    }
}
