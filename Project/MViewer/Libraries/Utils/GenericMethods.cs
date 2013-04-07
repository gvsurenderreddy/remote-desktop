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
        public string GetSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }

        public void ChangeLanguage(string language, Control.ControlCollection controls, Type controlType)
        {
            try
            {
                foreach (Control c in controls)
                {
                    ComponentResourceManager resources = new ComponentResourceManager(controlType);
                    CultureInfo cultureInfo = new CultureInfo(string.IsNullOrEmpty(language) ? "en-US" : language);
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
                            ChangeLanguage(language, c.Controls, controlType);
                        }
                    }
                    catch (Exception ex)
                    {
                        Tools.Instance.Logger.LogError(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }
    }
}
