using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socketeer
{
    public class Client
    {
        private bool running { get; set; }

        private StreamReader reader;
        private StreamWriter writer;
        private TcpClient connection;

        //Constructor, med inputtet port, som er integer.
        public Client(int port)
        {
            //laver en bool værdi, der er = true
            running = true;
            
            //opretter en TcpClient, der har ip-adresse, som string og vores tidligere port
            //Clienten opretter forbindelse til vores server.
            connection = new TcpClient("127.0.0.1", port);
            //Sørger for at der kan skrives og læses i mellem client og server
            NetworkStream stream = connection.GetStream();
            //vores læser
            reader = new StreamReader(stream);
            //vores skriver. Derefter tømmes bufferen
            writer = new StreamWriter(stream) { AutoFlush = true };

            //Viser informationen om den server man er tilsluttet til, og ens lokale port
            Console.WriteLine("Du er " + connection.Client.LocalEndPoint);

            //Her oprettes 2 tråde. En for reader og en for writer, som indsættes i et lamdaudryk. 
            //Reader og writer får en tråd hver, så de er i stand til at udføre handlinger samtidig.
            Thread rThread = new Thread(() => readerThread(reader));
            Thread wThread = new Thread(() => writerThread(writer));
            
            //Starter trådene
            rThread.Start();
            wThread.Start();
        }

        //metode til reader tråden
        private void readerThread(StreamReader reader)
        {
            //Så længe at running er true, skal løkken køre
            while (connection.Connected)
            {
                // Opretter en string der aflæser fra streamen.
                string data = reader.ReadLine();
                //Hvis data-stringen ikke er null og at den heller ikke er en tom string køres dette statement
                if (data != null && data.Trim() != "")
                {
                    // Laver et switch, som lukker, hvis der bliver tastet "exit"
                    switch (data)
                    {
                        case "exit":
                            Exit();
                            break;
                        default:
                            Console.WriteLine(data);
                            break;
                    }
                }
            }
        }

        //Metode til writer tråden
        private void writerThread(StreamWriter writer)
        {
            //Så længe at running er true, skal løkken køre
            while (connection.Connected)
            {
                // Opretter en string der aflæser fra streamen.
                string data = Console.ReadLine();
                //Køres hvis data ikke er en tom string 
                if (data.Trim() != "")
                {
                    //Skriver hvad der står i data-stringen
                    writer.WriteLine(data);   
                }
            }
        }

        //Metode til at lukke
        public void Exit()
        {
            //Sætter running til false, så ingen af while-løkkerne køres.
            running = false;
            //Lukker for reader streamen til serveren
            reader.Close();
            //Lukker til writer streamen til serveren 
            writer.Close();
        }
    }
}
