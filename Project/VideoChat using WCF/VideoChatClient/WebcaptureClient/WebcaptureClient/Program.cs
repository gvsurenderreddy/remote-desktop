using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VideoChatClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //IDataObject iData = Clipboard.GetDataObject();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmVideoChat());
        }
    }
}
