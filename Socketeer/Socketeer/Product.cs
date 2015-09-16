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

        // Her deklarere vi en hammerThread af typen Thread
        private Thread hammerThread;

        //En constructor der tager imod 4 inputs: 3 strings og en double
        public Product(string name, string productType, double minPrice, string id)
        {            
            Name = name;
            ProductType = productType;
            MinPrice = minPrice;
            HighestPrice = minPrice;
            Id = id;

            // Her instiansere vi vores hammerThread
            hammerThread = new Thread(() => {});
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

            hammerThread.Abort();
            hammerThread = new Thread(() =>
            {
                Thread.Sleep(10000);
                if (HammerEvent != null) HammerEvent(1, HighestBidder);

                Thread.Sleep(5000);
                if (HammerEvent != null) HammerEvent(2, HighestBidder);

                Thread.Sleep(3000);
                if (HammerEvent != null) HammerEvent(3, HighestBidder);
            });
            hammerThread.Start();
        }
    }
}
