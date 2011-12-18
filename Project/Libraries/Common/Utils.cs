using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Common
{
    public static class Utils
    {
        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = string.Empty;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }


        #region thread safe methods

        static void SetStringValue(ContentControl control, string newContent)
        {
            control.Content = newContent;
        }

        static void SetEnabledValue(ContentControl control, bool newVal)
        {
            control.IsEnabled = newVal;
        }

        public static void UpdateControlContent(Dispatcher dispatcher, ContentControl control, object newContent, ValueType valueType)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                switch (valueType)
                {
                    case ValueType.Boolean:
                        dispatcher.Invoke((Action<ContentControl, bool>)SetEnabledValue, control, newContent);
                        break;
                    case ValueType.String:
                        dispatcher.Invoke((Action<ContentControl, string>)SetStringValue, control, newContent);
                        break;
                }
            });
        }

        public enum ValueType { undefined = 0, Boolean = 1, String = 2 };

        #endregion
    }
}
