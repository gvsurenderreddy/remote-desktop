using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MViewer
{
    public class TcpServer
    {
        private TcpListener server;

        public TcpServer(string serv, int port)
        {
            server = new TcpListener(IPAddress.Parse(serv), port);
            server.Start();
            Console.WriteLine("Waiting for clients at " + serv + ":" + port + " ...");
            while (true)
            {
                while (!server.Pending())
                {
                    Thread.Sleep(1000);
                }
                ConnectionThread newconnection = new ConnectionThread();
                newconnection.threadListener = this.server;
                Thread newthread = new Thread(new ThreadStart(newconnection.HandleConnection));
                newthread.Start();
            }
        }

    }

}
