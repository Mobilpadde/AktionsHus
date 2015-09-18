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

        //Her er en constructor der tager imod 2 objecter som input.
        public UserHandler(object c, object p)
        {
            //vi sætter vores TcpClient til object c.
            client = (TcpClient) c;
            //her opretter vi både en writer og en reader, og sætter dem til vores client med GetStream - det gør at vi kan bruge den samme stream som vores client har.
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            StreamReader reader = new StreamReader(client.GetStream());

            //her opretter vi en liste af products, og sætter den til vores andet object.
            List<Product> products = (List<Product>) p;
            //vi sætter vores bidOn til -1, fordi man til at starte med ikke har budt på noget endnu, derfor kan den ikke være 0.
            int bidOn = -1;

            //vi laver en try catch her for når vores client kommer på
            try
            {
                //vi laver writelines som skriver at clienten er klar og hvilke kommandoer der kan bruges.
                writer.WriteLine("Vi er klar BITHCESSSS!!");
                writer.WriteLine("Kommandoer: Search, Choose & Bid");
                //her laver foreach, der siger at for hver product vi har i vores products liste så --->
                foreach (var product in products)
                {
                    //... skal den vise produktets navn, type, dens nuværende højeste pris, --||-- mindste pris og sluttidspunkt for varen.
                    writer.WriteLine("{0}'s {1} ({2}/{3}); Slutter {4}", product.Name, product.ProductType, product.HighestPrice, product.MinPrice, product.EndTime);
                }
                //vi laver en boolean der er sat til false og string data.
                bool done = false;
                string data;
                //her laver vi en while looke, hvor vi siger at hvis done ikke er false så -->
                while(!done)
                {
                    //vi sætter vores data string til vores reader og trimmer så vi fjerner mellemrum i starten og slutningen.
                    data = reader.ReadLine().Trim();
                    //vi laver en if, hvor vi siger at hvis data indeholder Search, så går vi videre ind til i loopet.
                    if (data.IndexOf("Search ") == 0)
                    {
                        //en foreach, for hvert product i listen så går vi ind i loopet.
                        foreach (var product in products)
                        {
                            //vi sætter variablen search til: vores data, som vi splitter i en ny string, som også er et char array, dvs. at når vi så søger så kommer der til at stå det vi har søgt efter, uden Search stringen
                            var search = data.Split(new string[] { "Search " }, StringSplitOptions.None);
                            //hvis product navnet indeholdet det vi har søgt efter (hvis den indeholdet det man har søgter efter, så er det større end -1), eller produkt typen indeholder det vi har søgt efter, så går vi ind i if-sætningen.
                            if (product.Name.IndexOf(search[1]) > -1 || product.ProductType.IndexOf(search[1]) > -1)
                            {
                                //her kommer produktet man har søgt efter så frem -> med de forskellige properties.
                                writer.WriteLine("{0}'s {1} ({2}/{3}); Slutter {4}", product.Name, product.ProductType, product.HighestPrice, product.MinPrice, product.EndTime);
                            }
                        }
                    }
                        //Her sker det samme som før, udover her vælger vi det produkt vi har søgt efter.
                    else if (data.IndexOf("Choose ") == 0)
                    {
                        //for hvert product man har fundet i listen, så fjerner den hammer metoden fra HammerEvent.
                        products.ForEach(x => x.HammerEvent -= hammer);

                        //her sker det samme som i vores forrige if...
                        int i = 0;
                        foreach (var product in products)
                        {
                            var search = data.Split(new string[] { "Choose " }, StringSplitOptions.None);
                            if (product.Name.IndexOf(search[1]) > -1 || product.ProductType.IndexOf(search[1]) > -1)
                            {
                                writer.WriteLine("{0}'s {1} ({2}/{3}); Slutter {4}", product.Name, product.ProductType, product.HighestPrice, product.MinPrice, product.EndTime);
                                bidOn = i;

                                //når du har valgt produktet, så bliver hammer tilføjet til HammerEvent, så vores hammerslag kan gå igang.
                                product.HammerEvent += hammer;

                                break;
                            }
                            i++;
                        }
                    }
                        //hvis du ikke har valgt noget, så får du en besked på at det skal du.
                    else if (bidOn == -1)
                    {
                        writer.WriteLine("Vælg venligst en vare før du byder.");
                    }
                        //hvis du skriver Bid i consolen, så går vi ind i vores if:
                    else if (data.IndexOf("Bid ") == 0)
                    {
                        //her sætter vi så det bud man har budt ind i vores bid variabel.
                        var bid = int.Parse(data.Split(new string[] { "Bid " }, StringSplitOptions.None)[1]);
                        //så hvis man har valgt et product, så bliver chosen sat til det produkt man har valgt.
                        Product chosen = products[bidOn];
                        //hvis buddet er større end mindste prisen og højere end den nuværende højeste pris, så går vi ind i vores if:
                        if (chosen.MinPrice < bid && chosen.HighestPrice < bid)
                        {
                            //så bliver den pris på varen sat til ens bud, og tiden går i gang - samtidig sættes HighestBidder til ens eget EndPoint, dvs byderens ip og port.
                            chosen.SetPrice(bid);
                            chosen.SetTime();
                            chosen.SetBidder(client.Client.LocalEndPoint.ToString());
                            writer.WriteLine("Du er bedste bud..!");
                            Console.WriteLine("Der er blevet budt {0} på {1} ({2})", bid, chosen.Name, chosen.ProductType);
                        }
                    }
                    else
                    {
                        writer.WriteLine("Kommandoen {0} eksisterer ikke", data.Trim());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //her lukker vi for writer og reader forbindelsen.
                writer.Close();
                reader.Close();
            }
        }

        //vi laver en hammer metode til vores hammerslag, med inputs int i og string bidder.
        private void hammer(int i, string bidder)
        {
            //en writeline der siger hvor i hammerslaget vi er nået til.
            writer.WriteLine("Hammerslag: " + i);
            //hvis i er = 3, så har man ikke flere chancer for at byde, derfor er varen solgt til vores højste byder...
            if (i == 3)
            {
                Console.WriteLine("Solgt til " + bidder);
                writer.WriteLine("Solgt til " + bidder);
            }
        }
    }
}
