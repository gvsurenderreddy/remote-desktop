using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;

namespace MViewer
{
    static class Program
    {
        #region private static members

        static Controller _controller;

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _controller = new Controller(); 
            _controller.StartApplication();

            // todo: use manual reset event instead of thread.sleep(0)
            Thread.Sleep(Timeout.Infinite);
        
        }
    }
}
