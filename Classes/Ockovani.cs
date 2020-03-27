using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceZvirat
{
    /// <summary>
    /// Třída reprezentující jeden záznam očkování
    /// </summary>
    public class Ockovani
    {
        /// <summary>
        /// Jedinečný identifikátor
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Datum očkování
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// Název podaného preparátu
        /// </summary>
        public string Pripravek { get; set; }
        /// <summary>
        /// Účel očkování - např. ochrana před určitou nemocí
        /// </summary>
        public string Ucel { get; set; }
        /// <summary>
        /// Detaily k očkování
        /// </summary>
        public string Popis { get; set; }
        /// <summary>
        /// Náklady na očkování v CZK
        /// </summary>
        public int Cena { get; set; }
        /// <summary>
        /// Číslo ovce, která obdržela roztok
        /// </summary>
        public string EvidencniCisloOvce { get; set; }

        /// <summary>
        /// Prázdný konstruktor pro XML serializer
        /// </summary>
        public Ockovani()
        {

        }

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="id">Jedinečný identifikátor</param>
        /// <param name="datum">Datum očkování</param>
        /// <param name="evidencniCislo">Číslo ovce, která obdržela roztok</param>
        /// <param name="pripravek">Název podaného preparátu</param>
        /// <param name="ucel">Účel očkování - např. ochrana před určitou nemocí</param>
        /// <param name="popis">Detaily k očkování</param>
        /// <param name="cena">Náklady na očkování v CZK</param>
        public Ockovani(int id,DateTime datum, string evidencniCislo,string pripravek, string ucel, string popis, int cena)
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
        /// Přetížená metoda pro výpis - v ListBoxu
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}\t{1}\t{2}", Datum.ToShortDateString(), Cena, Pripravek);
        }

    }
}
