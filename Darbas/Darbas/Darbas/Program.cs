using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Darbas
{
    class MainClass
    {
        const string knyga1 = "Knyga1.txt";// Pirmas tekstinis failas.
        const string knyga2 = "Knyga2.txt";// Antras tekstinis failas.
        const string rodikliai = "Rodikliai.txt";// Spausdinami rezultatai.
        const string manoKnyga = "ManoKnyga.txt";// Spaudinami rezultatai.
        public static char[] skyrikliai = { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t', '\r', '\n' };// Skyrikliai.

        public static void Main(string[] args)
        {
            // Trinami failai, kad nesikartotų tekstas.
            if (File.Exists(manoKnyga) && File.Exists(rodikliai))
            {
                File.Delete(manoKnyga);
                File.Delete(rodikliai);
            }

            //Sukuriami objektai.
            ZodziuKonteineris k1 = new ZodziuKonteineris(1000);
            //Nuskaitomas knyga1.txt failas.
            SudarytiZodziuSarasa(knyga1, skyrikliai, k1);
            ZodziuKonteineris k2 = new ZodziuKonteineris(1000);
            //Nuskaitomas knyga2.txt failas.
            SudarytiZodziuSarasa(knyga2, skyrikliai, k2);
            ZodziuKonteineris k = new ZodziuKonteineris(1000);

            IlgiausiZodziai(k1, k2, k);

            string zodis;
            string zodis1;

            int eilute;
            int eilute1;

            //Metodas su knyga1 ir knyga2 tekstais.
            SpausdinaFaileTeksta(manoKnyga, knyga1, knyga2, skyrikliai);

            //Metodai su sakiniais.
            Trumpiausias(knyga1, out zodis, out eilute);
            Trumpiausias(knyga2, out zodis1, out eilute1);

            //Metodas, kuris spausdina į ekraną.
            SpausdinimasEkrane(k, k.ZodziuSkaicius, zodis, eilute, zodis1, eilute1);
            //Metodas, kuris spausdina į failą.
            SpausdinimasFaile(rodikliai, k, k.ZodziuSkaicius, zodis, eilute, zodis1, eilute1);

            Console.ReadKey();
        }

        // Metodas, kuris nuskaito tekstus.
        static void SudarytiZodziuSarasa(string kn, char[] skyrikliai, ZodziuKonteineris k)
        {
            string[] eilutes = File.ReadAllLines(kn, Encoding.UTF8);

            foreach (string eilute in eilutes)
            {
                string[] zodziai = eilute.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
                foreach (string zodis in zodziai)
                {
                    Zodis zdz = new Zodis(zodis.ToLower(), 0, zodis.Length);
                    k.PridetiZodi(zdz);
                }
            }
        }

        // 1 uzduotis
        static ZodziuKonteineris IlgiausiZodziai(ZodziuKonteineris knyga1zodziai, ZodziuKonteineris knyga2zodziai, ZodziuKonteineris k)
        {
            bool jauYra = false;
            ZodziuKonteineris temp = new ZodziuKonteineris(30);

            for (int i = 0; i < knyga1zodziai.ZodziuSkaicius; i++)
            {
                jauYra = false;
                for (int j = 0; j < knyga2zodziai.ZodziuSkaicius; j++)
                {
                    if (knyga1zodziai.GautiZodi(i).ZodzioPavadinimas.ToLower() == knyga2zodziai.GautiZodi(j).ZodzioPavadinimas.ToLower())
                    {
                        jauYra = true;
                        break;
                    }
                }
                if (jauYra == false)
                {
                    int pasikartojaIndexas = temp.PasikartojancioIndexas(knyga1zodziai.GautiZodi(i));
                    if (pasikartojaIndexas < 0)
                    {
                        temp.PridetiZodi(knyga1zodziai.GautiZodi(i));
                        continue;
                    }
                    temp.GautiZodi(pasikartojaIndexas).Pasikartojimai += 1;
                }
            }
            temp = Rikiavimas(temp);
            for (int i = 0; i <= temp.ZodziuSkaicius; i++)
            {
                if (i >= 10)
                {
                    break;
                }
                k.PridetiZodi(temp.GautiZodi(i));
            }

            return k;

        }

        // 1 uzduoties zodziu rikiavimas
        static ZodziuKonteineris Rikiavimas(ZodziuKonteineris rikiojamas)
        {
            for (int i = 0; i < rikiojamas.ZodziuSkaicius; i++)
            {
                for (int j = 0; j < rikiojamas.ZodziuSkaicius - 1; j++)
                {
                    if (rikiojamas.GautiZodi(j).Ilgis < rikiojamas.GautiZodi(j + 1).Ilgis)
                    {
                        rikiojamas.Swap(j, j + 1);
                    }
                }
            }
            return rikiojamas;
        }

        // Metodas, kuris tikrina du tekstus ir juos jungia į vieną
        // Taip pat spausdinama į failą.
        static string ManoKnygosSudarymas(string manoKnyga, string knyga1, string knyga2, char[] skyrikliai)
        {
            string pirmojiKnyga = File.ReadAllText(knyga1, Encoding.UTF8);
            string antrojiKnyga = File.ReadAllText(knyga2, Encoding.UTF8);
            string tekstas = "";
            int pirmosKnygosPabaiga = 0;
            int antrosKnygosPabaiga = 0;
            string pirmasZodis;

            while (pirmosKnygosPabaiga != pirmojiKnyga.Length || antrosKnygosPabaiga != antrojiKnyga.Length)
            {

                if (pirmosKnygosPabaiga == pirmojiKnyga.Length)
                {
                    tekstas += antrojiKnyga.Substring(0);
                    antrosKnygosPabaiga = antrojiKnyga.Length;
                }
                else
                {
                    pirmasZodis = antrojiKnyga.Split(skyrikliai)[0].ToLower(); // pirmas zodis
                    int pirmoIlgis = pirmasZodis.Length;
                    int pirmasIndeksas = pirmojiKnyga.IndexOf(pirmasZodis); // indeksas, kur randasi pasikartojantis antro teksto pirmas zodis


                    pirmasZodis = pirmojiKnyga.Split(skyrikliai)[1]; // nenukopijuotas pirmasis zodis
                    int antroIlgis = pirmasZodis.Length;

                    int antrasIndeksas = antrojiKnyga.IndexOf(pirmasZodis);

                    if (pirmasIndeksas != -1)
                    {
                        tekstas += pirmojiKnyga.Substring(0, pirmasIndeksas);
                        pirmojiKnyga = pirmojiKnyga.Substring(pirmasIndeksas);
                        pirmosKnygosPabaiga += (pirmasIndeksas + 2 + pirmoIlgis);
                    }
                    else
                    {
                        tekstas += pirmojiKnyga.Substring(0);
                        pirmosKnygosPabaiga = pirmojiKnyga.Length;
                    }

                    if (antrasIndeksas != -1)
                    {
                        tekstas += antrojiKnyga.Substring(0, antrasIndeksas);
                        antrojiKnyga = antrojiKnyga.Substring(antrasIndeksas); // likes tekstas
                        antrosKnygosPabaiga += (antrasIndeksas + 2 + antroIlgis);

                    }
                    else
                    {
                        tekstas += antrojiKnyga.Substring(0);
                        antrosKnygosPabaiga = antrojiKnyga.Length;
                    }
                }
            }
            return tekstas;
        }

        static void SpausdinaFaileTeksta(string manoKnyga, string knyga1, string knyga2, char[] skyrikliai)
        {
            string tekstas = ManoKnygosSudarymas(manoKnyga, knyga1, knyga2, skyrikliai);

            using (StreamWriter writer = new StreamWriter(manoKnyga))
            {
                writer.WriteLine(tekstas);
            }
        }

        // Metodas, kuris tekste  atranda trumpiausia eilutę.
        // Taip pat eilutės ilgį ir vietą.
        static void Trumpiausias(string knyga, out string zodis, out int eilute)
        {
            string[] failoEilutes = File.ReadAllLines(knyga1, Encoding.GetEncoding(1257));

            int min = 10000;
            zodis = "";
            eilute = 0;
            int n = 0;
            foreach (string a in failoEilutes)
            {
                if (a.Length < min && Bandymas3(a) == true)
                {
                    min = a.Length;
                    zodis = a;
                    eilute = n + 1;
                }
                n++;
            }
        }

        // Metodas, kuris patikrina ar nėra mažiau nei 3 žodžiai sakinys.
        static bool Bandymas3(string a)
        {
            string[] zodziaiEiluteje = a.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
            if (zodziaiEiluteje.Length > 3)
            {
                return true;
            }
            return false;
        }

        // Metodas, kuris spausdina į konsolę.
        static void SpausdinimasEkrane(ZodziuKonteineris k, int zodziuSkaicius, string zodis, int eilute, string zodis1, int eilute1)
        {
            Console.WriteLine("Žodžiai ir jų kiekis:");
            Console.WriteLine("");
            for (int i = 0; i < zodziuSkaicius; i++)
            {
                Console.WriteLine("{0}{1}", i + 1, k.GautiZodi(i).ToString() + "\n");
            }

            Console.WriteLine("");
            Console.WriteLine("Trumpiausias pirmo teksto sakinys: " + zodis + "\n" + "  Eilutė:" + eilute + "\n" + "  Eilutės ilgis:" + zodis.Length);
            Console.WriteLine("");
            Console.WriteLine("Trumpiausias antro teksto sakinys: " + zodis1 + "\n" + "  Eilutė:" + eilute1 + "\n" + "  Eilutės ilgis:" + zodis1.Length);
        }

        // Metodas, kuris spausdina i faila.
        static void SpausdinimasFaile(string rodikliai, ZodziuKonteineris k, int zodziuSkaicius, string zodis, int eilute, string zodis1, int eilute1)
        {
            using (StreamWriter writer = new StreamWriter(rodikliai))
            {
                writer.WriteLine("Žodžiai ir jų kiekis.");
                for (int i = 0; i < zodziuSkaicius; i++)
                {
                    writer.WriteLine(i + 1 + ". " + k.GautiZodi(i).ToString() + "\n");
                }

                writer.WriteLine("");
                writer.WriteLine("Trumpiausias pirmo teksto sakinys: " + zodis + "\n" + "  Eilutė:" + eilute + "\n" + "  Eilutės ilgis:" + zodis.Length);
                writer.WriteLine("");
                writer.WriteLine("Trumpiausias antro teksto sakinys: " + zodis1 + "\n" + "  Eilutė:" + eilute1 + "\n" + "  Eilutės ilgis:" + zodis1.Length);
            }
        }

    }
}