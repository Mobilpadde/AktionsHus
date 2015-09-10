using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Product
    {
        public string Name { get; set; }

        public string ProductType { get; set; }

        public double MinPrice { get; set; }

        public double HighestPrice { get; set; }

        public string Id { get; set; }

        public DateTime Time { get; set; }

        public Product(string name, string productType, double minPrice, double highestPrice, string id)
        {
            Name = name;
            ProductType = productType;
            MinPrice = minPrice;
            HighestPrice = highestPrice;
            Id = id;

            SetTime();
        }

        public void SetTime()
        {
            DateTime d = DateTime.Now;
            d.AddSeconds(18);
            Time = d;
        }
    }
}
