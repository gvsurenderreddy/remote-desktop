using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Reflection;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex mutex = new Mutex(true, "RemotingServer");
   

        public App():base()
        {
            // prevent the program to be started twice
            // create new mutex
            if (mutex.WaitOne(TimeSpan.Zero, true)) 
            {
                // if creation of mutex is successful
                log4net.Config.BasicConfigurator.Configure();
                ServerStaticMembers.Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().ToString());
            }
            else
            {
                MessageBox.Show("Cannot open more than one RemotingServer!");
                Environment.Exit(0);
            }
        }
    }
}
