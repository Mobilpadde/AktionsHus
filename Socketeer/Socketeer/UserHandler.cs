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
        public UserHandler(object c, object p)
        {
            TcpClient client = (TcpClient) c;
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            StreamReader reader = new StreamReader(client.GetStream());

            List<Product> products = (List<Product>) p;

            int bidOn = -1;

            try
            {
                writer.WriteLine("Vi er klar BITHCESSSS!!");
                foreach (var product in products)
                {
                    writer.WriteLine("{0} ({1}/{2})", product.Name, (product.HighestPrice < product.MinPrice ? product.MinPrice : product.HighestPrice), product.MinPrice);
                }

                bool done = false;
                string data;
                while(!done)
                {
                    data = reader.ReadLine().Trim();
                    if (data.Contains("Search "))
                    {
                        foreach (var product in products)
                        {
                            var search = data.Split(new string[] { "Search " }, StringSplitOptions.None);
                            if (product.Name.IndexOf(search[1]) > -1)
                            {
                                writer.WriteLine("{0} ({1}/{2})", product.Name, (product.HighestPrice < product.MinPrice ? product.MinPrice : product.HighestPrice), product.MinPrice);
                            }
                        }
                    }
                    else if (data.Contains("Choose "))
                    {
                        int i = 0;
                        foreach (var product in products)
                        {
                            var search = data.Split(new string[] { "Choose " }, StringSplitOptions.None);
                            if (product.Name.IndexOf(search[1]) > -1)
                            {
                                writer.WriteLine("{0} ({1}/{2})", product.Name, (product.HighestPrice < product.MinPrice ? product.MinPrice : product.HighestPrice), product.MinPrice);
                                bidOn = i;
                                break;
                            }
                            i++;
                        }
                    }
                    else if (bidOn == -1)
                    {
                        writer.WriteLine("Vælg venligst en vare før du byder.");
                    }
                    else if (data.Contains("Bid "))
                    {
                        var bid = int.Parse(data.Split(new string[] { "Bid " }, StringSplitOptions.None)[1]);
                        Product chosen = products[bidOn];
                        if (chosen.MinPrice < bid && chosen.HighestPrice < bid)
                        {
                            chosen.HighestPrice = bid;
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
    }
}
