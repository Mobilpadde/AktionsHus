using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socketeer
{
    class UserHandler
    {
        public UserHandler(object client)
        {
            Socket socket = (Socket) client;
            NetworkStream stream = new NetworkStream(socket);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            StreamReader reader = new StreamReader(stream);

            try
            {
                bool done = false;
                while(!done)
                {
                    writer.WriteLine("Klar motherfucker!");
                    writer.WriteLine(reader.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                writer.Close();
                reader.Close();
                stream.Close();
                socket.Close();
            }
        }
    }
}
