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
        private TcpListener listener;
        private bool run;
        private List<UserHandler> uHandlers;
        private List<TcpClient> clients;
        private List<Product> products;

        public Server(int port, List<Product> products)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(ip, port);
            clients = new List<TcpClient>();
            this.products = products;
        }

        public void Broadcast()
        {
            Thread.Sleep(15000);
            Console.WriteLine("Broadcast start");
            clients.ForEach(c =>
            {
                StreamWriter writer = new StreamWriter(c.GetStream()) { AutoFlush = true };
                writer.WriteLine("Test");
                Console.WriteLine("Broadcasting...");
            });
            Console.WriteLine("Broadcast done");
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("SERVEREN ER OPPE BITCHES!");
            
            run = true;
            while (run)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("CLIENTEN ER PÅ BITCHESSSSS!");

                clients.Add(client);

                Thread t = new Thread(() => new UserHandler(client, products));
                t.Start();
            }
        }

        public void Stop()
        {
            run = false;
        }
    }
}
