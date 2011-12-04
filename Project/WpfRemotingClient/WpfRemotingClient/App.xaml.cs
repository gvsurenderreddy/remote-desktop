using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace WpfRemotingClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex mutex;

        public App()
            : base()
        {
            string serverHost = ConfigurationManager.AppSettings["server"];
            mutex = new Mutex(true, serverHost);
            // prevent the program to be started twice
            // create new mutex
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                // if creation of mutex is successful

            }
            else
            {
                MessageBox.Show("Cannot open more than one RemotingServer!");
                Environment.Exit(0);
            }

        }
    }
}
