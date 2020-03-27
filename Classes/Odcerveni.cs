using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceZvirat
{
    /// <summary>
    /// Třída reprezentující jeden záznam Odčervení u OVCE
    /// </summary>
    public class Odcerveni
    {
        /// <summary>
        /// Jedinečné ID záznamu
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Datum podání připravku
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// Podaný odčervovací přípravek
        /// </summary>
        public string Pripravek { get; set; }
        /// <summary>
        /// Účel odčervení - např. ztráta váhy zvířete
        /// </summary>
        public string Ucel { get; set; }
        /// <summary>
        /// Detaily k podání
        /// </summary>
        public string Popis { get; set; }
        /// <summary>
        /// Zaplacená částka v CZK
        /// </summary>
        public int Cena { get; set; }
        /// <summary>
        /// Číslo ovce, která obdržela roztok
        /// </summary>
        public string EvidencniCisloOvce { get; set; }

        /// <summary>
        /// Prázdný konstruktor pro XML serializer
        /// </summary>
        public Odcerveni()
        {

        }

        /// <summary>
        /// Základní konstruktor 
        /// </summary>
        /// <param name="id">Jedinečné ID záznamu</param>
        /// <param name="datum">Datum podání připravku</param>
        /// <param name="evidencniCislo">Číslo ovce, která obdržela roztok</param>
        /// <param name="pripravek">Podaný odčervovací přípravek</param>
        /// <param name="ucel">Účel odčervení - např. ztráta váhy zvířete</param>
        /// <param name="popis">Detaily k podání</param>
        /// <param name="cena">Zaplacená částka v CZK</param>
        public Odcerveni(int id,DateTime datum, string evidencniCislo, string pripravek, string ucel, string popis, int cena)
        {
            ID = id;
            Datum = datum;
            Pripravek = pripravek;
            Ucel = ucel;
            Popis = popis;
            Cena = cena;
            EvidencniCisloOvce = evidencniCislo;
        }

        /// <summary>
        /// Přetížená metoda pro výpis - např. ListBoxu
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}\t{1}\t{2}", Datum.ToShortDateString(), Cena, Pripravek);
        }

    }
}
