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
        private TcpClient client;
        private StreamWriter writer;

        public UserHandler(object c, object p)
        {
            client = (TcpClient) c;
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            StreamReader reader = new StreamReader(client.GetStream());

            List<Product> products = (List<Product>) p;

            int bidOn = -1;

            try
            {
                writer.WriteLine("Vi er klar BITHCESSSS!!");
                writer.WriteLine("Kommandoer: Search, Choose & Bid");
                foreach (var product in products)
                {
                    writer.WriteLine("{0}'s {1} ({2}/{3}); Slutter {4}", product.Name, product.ProductType, product.HighestPrice, product.MinPrice, product.EndTime);
                }

                bool done = false;
                string data;
                while(!done)
                {
                    data = reader.ReadLine().Trim();
                    if (data.IndexOf("Search ") == 0)
                    {
                        foreach (var product in products)
                        {
                            var search = data.Split(new string[] { "Search " }, StringSplitOptions.None);
                            if (product.Name.IndexOf(search[1]) > -1 || product.ProductType.IndexOf(search[1]) > -1)
                            {
                                writer.WriteLine("{0}'s {1} ({2}/{3}); Slutter {4}", product.Name, product.ProductType, product.HighestPrice, product.MinPrice, product.EndTime);
                            }
                        }
                    }
                    else if (data.IndexOf("Choose ") == 0)
                    {
                        products.ForEach(x => x.HammerEvent -= hammer);

                        int i = 0;
                        foreach (var product in products)
                        {
                            var search = data.Split(new string[] { "Choose " }, StringSplitOptions.None);
                            if (product.Name.IndexOf(search[1]) > -1 || product.ProductType.IndexOf(search[1]) > -1)
                            {
                                writer.WriteLine("{0}'s {1} ({2}/{3}); Slutter {4}", product.Name, product.ProductType, product.HighestPrice, product.MinPrice, product.EndTime);
                                bidOn = i;

                                product.HammerEvent += hammer;

                                break;
                            }
                            i++;
                        }
                    }
                    else if (bidOn == -1)
                    {
                        writer.WriteLine("Vælg venligst en vare før du byder.");
                    }
                    else if (data.IndexOf("Bid ") == 0)
                    {
                        var bid = int.Parse(data.Split(new string[] { "Bid " }, StringSplitOptions.None)[1]);
                        Product chosen = products[bidOn];
                        if (chosen.MinPrice < bid && chosen.HighestPrice < bid)
                        {
                            chosen.SetPrice(bid);
                            chosen.SetTime();
                            chosen.SetBidder(client.Client.RemoteEndPoint.ToString());
                            writer.WriteLine("Du er bedste bud..!");
                            Console.WriteLine("Der er blevet budt {0} på {1} ({2})", bid, chosen.Name, chosen.ProductType);
                        }
                    }
                    else
                    {
                        writer.WriteLine("Nathin'");
                    }
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
            }
        }

        private void hammer(int i, string bidder)
        {
            writer.WriteLine("Hammerslag: " + i);
            if (i == 3) Console.WriteLine("Solgt til " + bidder);
        }
    }
}
