using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socketeer;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Socketeer.Server sq85 = new Socketeer.Server(5000);
        }
    }
}
