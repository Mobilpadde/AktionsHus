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

        public Client(int port)
        {
            running = true;

            TcpClient connection = new TcpClient("127.0.0.1", port);
            NetworkStream stream = connection.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            /*Thread rThread = new Thread(() => readerThread(reader));
            Thread wThread = new Thread(() => writerThread(writer));

            rThread.Start();
            wThread.Start();*/

            string text;
            while (running)
            {
                text = reader.ReadLine();
                Console.WriteLine(text);

                text = Console.ReadLine();
                writer.WriteLine(text);
            }
        }

        private void readerThread(StreamReader reader)
        {
            while (running)
            {
                string data = reader.ReadLine();
                if (data.Trim() != "")
                {
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

        private void writerThread(StreamWriter writer)
        {
            while (running)
            {
                string data = Console.ReadLine();
                if (data.Trim() != "")
                {
                    writer.WriteLine(data);
                }
            }
        }

        public void Exit()
        {
            running = false;
            reader.Close();
            writer.Close();
        }
    }
}
