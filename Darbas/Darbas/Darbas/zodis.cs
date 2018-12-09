using System;
namespace Darbas
{
    /// <summary>
    /// Klasė, kuri yra skirta duomenims apie žodžius aprašyti
    /// </summary>
    class Zodis
    {
        // Elementai, sąsajos metodai.
        public string ZodzioPavadinimas { get; set; } //Žodis
        public int Pasikartojimai { get; set; } //Jo pasikartojimų skaičius
        public int Ilgis { get; set; } // Zodzio simboliu kiekis

        /// <summary>
        /// Žodžio konstruktorius
        /// </summary>
        /// <param name="zodzioPavadinimas">Žodis</param>
        /// <param name="pasikartojimai">Pasikartojimų skaičius</param>
        /// <param name="ilgis">Eilutės numeris</param>
        public Zodis(string zodzioPavadinimas,int pasikartojimai, int ilgis)
        {
            ZodzioPavadinimas = zodzioPavadinimas;
            Pasikartojimai = pasikartojimai;
            Ilgis = ilgis;
        }

        /// <summary>
        /// Pakeičia ToString metodą
        /// </summary>
        /// <returns>Pakeistą ToString šabloną</returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2:d} {3} {4} {5} {6} {7} {8}", ".", "Žodis:", ZodzioPavadinimas, "|", "Pasikartojimai:", Pasikartojimai, "|", "Ilgis:", Ilgis);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Zodis other))
            {
                return false;
            }
            return ZodzioPavadinimas == other.ZodzioPavadinimas;
        }

        /// <summary>
        /// Dėl pakeisto Equals metodo, pakeičiamas GetHashCode metodas
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ZodzioPavadinimas.GetHashCode();
        }

        //Palyginimas.
        public static bool operator ==(Zodis pirmas, Zodis antras)
        {
            return pirmas.ZodzioPavadinimas == antras.ZodzioPavadinimas;
        }

        public static bool operator !=(Zodis pirmas, Zodis antras)
        {
            return pirmas.ZodzioPavadinimas != antras.ZodzioPavadinimas;
        }
    }
}
