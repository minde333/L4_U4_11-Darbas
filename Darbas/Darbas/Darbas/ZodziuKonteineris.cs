using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darbas
{
    class ZodziuKonteineris
    {
        // Masyvas.
        private Zodis[] Zodziai { get; set; }
        // Kiekis.
        public int ZodziuSkaicius { get; private set; }

        // Kiekis.
        public ZodziuKonteineris(int size)
        {
            Zodziai = new Zodis[size];
            ZodziuSkaicius = 0;
        }

        // Paimamas elementas.
        public Zodis GautiZodi(int indeksas)
        {
            return Zodziai[indeksas];
        }

        // Pridedama elementas.
        public void PridetiZodi(Zodis zodis)
        {
            Zodziai[ZodziuSkaicius++] = zodis;
        }

        // Palyginimui. 
        public bool Contains(Zodis zodis)
        {
            return Zodziai.Contains(zodis);
        }

        public void Swap(int pirmas, int antras)
        {
            Zodis temp = Zodziai[pirmas];
            Zodziai[pirmas] = Zodziai[antras];
            Zodziai[antras] = temp;

        }

        public int PasikartojancioIndexas(Zodis zodis)
        {
            for (int i = 0; i < ZodziuSkaicius; i++)
            {
                if (Zodziai[i].ZodzioPavadinimas.ToLower() == zodis.ZodzioPavadinimas.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
