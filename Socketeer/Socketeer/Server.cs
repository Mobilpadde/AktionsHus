using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socketeer
{
    public class Server
    {
        public Server(int port)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();
            Console.WriteLine("SERVEREN ER OPPE BITCHES!");

            while(true)
            {
                Socket client = listener.AcceptSocket();
                Console.WriteLine("CLIENTEN ER PÅ BITCHESSSSS!");

                Thread t = new Thread(() => new UserHandler(client));
                t.Start();
            }
        }
       
    }
}
