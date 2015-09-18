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

        //En contructor med 2 parametre: en integer port, og en liste products
        public Server(int port, List<Product> products)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(ip, port);
            clients = new List<TcpClient>();
            this.products = products;
        }

        //En metode der tager to inputs: string og tcpClient
        public void Broadcast(string text, TcpClient excludeClient)
        {
            //Foreach loop, der kører for hver client
            clients.ForEach(c =>
            {
                //Hvis clienten ikke er = excludeClient køres dette statement
                if (c != excludeClient)
                {
                    //laver en writer, så clienterne kan skrive til streamen
                    StreamWriter writer = new StreamWriter(c.GetStream()) { AutoFlush = true };
                    writer.WriteLine(text);   
                }
            });
        }

        //En metode kaldet Start
        public void Start()
        {
            //starter listener
            listener.Start();
            //Når serveren er startet viser programmet linjen
            Console.WriteLine("SERVEREN ER OPPE!");
            //Boolsk udtryk der er = true
            run = true;
            //While løkke, der kører så længe run er true
            while (run)
            {
                //Lytter efter klienter, hvis der er nogen bliver de accepteret
                TcpClient client = listener.AcceptTcpClient();
                //Meddeler at der er en klient, der er tilsluttet
                Console.WriteLine("EN CLIENT ER TILSLUTTET!");
                //Tilføjer klienten
                clients.Add(client);
                //Laver en ny tråd til hver klient
                Thread t = new Thread(() => new UserHandler(client, products));
                t.Start();
            }
        }
        //Metode der sætter run til false, så løkkerne stopper.
        public void Stop()
        {
            run = false;
        }
    }
}
