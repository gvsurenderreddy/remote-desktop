using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace WpfRemotingServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex mutex = new Mutex(true, "RemotingServer");
        public static ServerMainWindow smw;

        public App():base()
        {
            // prevent the program to be started twice
            // create new mutex
            if (mutex.WaitOne(TimeSpan.Zero, true)) 
            {
                // if creation of mutex is successful
                smw = new ServerMainWindow();
                ServerStaticMembers.ServerView = smw;
                //Application.LoadComponent(smw, new Uri("ServerMainWindow.xaml"));
            }
            else
            {
                MessageBox.Show("Cannot open more than one RemotingServer!");
                Environment.Exit(0);
            }
        }
    }
}
