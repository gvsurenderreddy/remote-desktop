using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using GenericObjects;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using Utils;

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
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                _controller = new Controller();
                _controller.InitializeSettings();

                Tools.Instance.Logger.LoggerInitialize();

                Tools.Instance.Logger.LogInfo("MViewer application started");

                _controller.StartApplication();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                Tools.Instance.Logger.LogInfo("MViewer application stopped");
            }
        }

        #endregion

        #region proprieties

        // todo: remove the static Controller and use event handlers
        public static IController Controller
        {
            get { return Program._controller; }
        }

        #endregion
    }
}
