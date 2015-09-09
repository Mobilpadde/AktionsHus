using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            Socket clientSocket = listener.AcceptSocket();
            Console.WriteLine("CLIENTEN ER PÅ, YEAH!");
            NetworkStream stream = new NetworkStream(clientSocket);
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            writer.AutoFlush = true;

            while(true)
            {
                writer.WriteLine("klar");
                string data = reader.ReadLine();
                writer.WriteLine(data);
            }
        }
       
    }
}
