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

        //opretter et delegate, der tager imod inputtene integer i og stringen bidder
        public delegate void HammerDelegetae(int i, string bidder);
        //Her oprettes et event for delegaten HammerDelegate
        public event HammerDelegetae HammerEvent;

        //Der oprettes et array af threads der holder styr på hammerslag, for de forskellige auktioner
        private Thread[] hammerThreads;

        //En constructor der tager imod 4 inputs: 3 strings og en double
        public Product(string name, string productType, double minPrice, string id)
        {            
            Name = name;
            ProductType = productType;
            MinPrice = minPrice;
            HighestPrice = minPrice;
            Id = id;

            //Tilføjer 3 tråde til vores array af threads
            hammerThreads = new Thread[]
            {
                new Thread(() => {}),
                new Thread(() => {}),
                new Thread(() => {})
            };
        }
        
        //Metode der overskriver bidder med highest bidder.
        public void SetBidder(string bidder)
        {
            HighestBidder = bidder;
        }

        //Sætter prisen til Highestprice
        public void SetPrice(double price)
        {
            HighestPrice = price;
        }
        
        //Metode til at sætte auktionen på tælling
        public void SetTime()
        {
            //Laver en Datetime variabel, der tager den aktuelle tid
            DateTime d = DateTime.Now;
            //Sluttidspunktet er den aktuelle tid + 18 sekunder
            EndTime = d.AddSeconds(18);
            // Her konvertes hammetreads array'et til en liste, og lukker hver tråd t i listen.
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
