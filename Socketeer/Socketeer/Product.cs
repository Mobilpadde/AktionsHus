using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Socketeer
{
    public class Product
    {
        public string Name { get; private set; }

        public string ProductType { get; private set; }

        public double MinPrice { get; private set; }

        public double HighestPrice { get; private set; }

        public string HighestBidder { get; private set; }

        public string Id { get; private set; }

        public DateTime EndTime { get; private set; }

        public delegate void HammerDelegetae(int i, string bidder);
        public event HammerDelegetae HammerEvent;

        private Thread[] hammerThreads;

        public Product(string name, string productType, double minPrice, string id)
        {
            Name = name;
            ProductType = productType;
            MinPrice = minPrice;
            HighestPrice = minPrice;
            Id = id;

            hammerThreads = new Thread[]
            {
                new Thread(() => {}),
                new Thread(() => {}),
                new Thread(() => {})
            };
        }

        public void SetBidder(string bidder)
        {
            HighestBidder = bidder;
        }

        public void SetPrice(double price)
        {
            HighestPrice = price;
        }

        public void SetTime()
        {
            DateTime d = DateTime.Now;
            EndTime = d.AddSeconds(18);

            hammerThreads.ToList().ForEach(t => t.Abort());

            hammerThreads[0] = new Thread(() =>
            {
                Thread.Sleep(10000);
                if (HammerEvent != null) HammerEvent(1, HighestBidder);
            });

            hammerThreads[1] = new Thread(() =>
            {
                Thread.Sleep(15000);
                if (HammerEvent != null) HammerEvent(2, HighestBidder);
            });

            hammerThreads[2] = new Thread(() =>
            {
                Thread.Sleep(18000);
                if (HammerEvent != null) HammerEvent(3, HighestBidder);
            });

            hammerThreads.ToList().ForEach(t => t.Start());
        }
    }
}
