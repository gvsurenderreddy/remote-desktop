using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace MViewer
{
    public class ConnectionThread
    {
        public TcpListener threadListener;
        private int connections = 0;

        public void HandleConnection()
        {
            TcpClient client = threadListener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();
            string receivedMsg = string.Empty;
            byte[] buffer = new byte[1024];

            Thread.Sleep(1000);
            while (!ns.DataAvailable) { }

            while (ns.DataAvailable)
            {
                int read = ns.Read(buffer, 0, buffer.Length);
                receivedMsg += Encoding.ASCII.GetString(buffer);
                Array.Clear(buffer, 0, buffer.Length);
                if (read == 0 || ns.DataAvailable == false)
                {
                    // use the controller to ping the contacts and get their status
                    Program.Controller.NotificationReceived();

                    break;
                }
            }

            //connections++;
            //Console.WriteLine("New client accepted: {0} active connections",
            //        connections);
            //string welcome = "Welcome to my test server";
            //data = Encoding.ASCII.GetBytes(welcome);
            //ns.Write(data, 0, data.Length);
            //while(true)
            //{
            //    data = new byte[1024];
            //    recv = ns.Read(data, 0, data.Length);
            //    if (recv == 0)
            //    break;
   
            //    ns.Write(data, 0, recv);
            //}

            ns.Close();
            client.Close();
            connections--;
            Console.WriteLine("Client disconnected: {0} active connections",
                    connections);
        }
    }
}
