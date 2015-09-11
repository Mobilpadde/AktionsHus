using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Socketeer;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>
            {
                new Product("Arne Jacobsen", "Stol", 40000, double.MinValue, Guid.NewGuid().ToString()),
                new Product("Bo Henriksen", "Vase", 500, double.MinValue, Guid.NewGuid().ToString()),
                new Product("Italinano", "Komode", 2100, double.MinValue, Guid.NewGuid().ToString()),
                new Product("Claude Monet", "Maleri", 400000, double.MinValue, Guid.NewGuid().ToString())
            };
            
            Socketeer.Server sq85 = new Socketeer.Server(5000, products);
            Thread t = new Thread(() => sq85.Broadcast());
            t.Start();
            sq85.Start();
        }
    }
}
