using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socketeer;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socketeer.Client c = new Socketeer.Client(5000);
            Console.Read();
        }
    }
}
