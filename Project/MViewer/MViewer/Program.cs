using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using GenericDataLayer;

namespace MViewer
{
    static class Program
    {
        #region private static members

        static IController _controller;

        #endregion

        #region main

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

        #endregion

        #region proprieties

        public static IController Controller
        {
            get { return Program._controller; }
        }

        #endregion
    }
}
