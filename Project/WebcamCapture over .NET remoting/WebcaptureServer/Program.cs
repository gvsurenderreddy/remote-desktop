using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WebcaptureServer
{
    static class Program
    {
        public static Form1 FRM;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FRM = new Form1();
            Application.Run(FRM);
        }
    }
}
