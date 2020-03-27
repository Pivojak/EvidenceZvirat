using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EvidenceZvirat
{
    /// <summary>
    /// Třída reprezentující jeden finanční záznam (transakce)
    /// </summary>
    public class Finance
    {
        /// <summary>
        /// Jedinečný identifikátor finančního záznamu
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Název transakce
        /// </summary>
        public string Nazev { get; set;}
        /// <summary>
        /// Popis transakce
        /// </summary>
        public string Popis { get; set; }
        /// <summary>
        /// Částka transakce v CZK
        /// </summary>
        public double Castka { get; set; }
        /// <summary>
        /// Datum uskutečnění transakce
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// 0 - Vydaje, 1 - Prijmy
        /// </summary>
        public byte TypOperace { get; set; }
        /// <summary>
        /// Číslo vrhu PRASNICE, ke které má být transakce vztažena
        /// </summary>
        public byte CisloVrhu { get; set; }
        /// <summary>
        /// Prasnice, ke které má být transakce vztažena
        /// </summary>
        public string Matka { get; set; }

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="id">Jedinečný identifikátor finančního záznamu</param>
        /// <param name="nazev">Název transakce</param>
        /// <param name="popis">Popis transakce</param>
        /// <param name="castka">Částka transakce v CZK</param>
        /// <param name="matka">Prasnice, ke které má být transakce vztažena</param>
        /// <param name="vrh">Číslo vrhu PRASNICE, ke které má být transakce vztažena</param>
        /// <param name="typOperace">0 - Vydaje, 1 - Prijmy</param>
        /// <param name="datum"></param>
        public Finance(int id, string nazev, string popis, double castka, string matka, byte vrh,byte typOperace, DateTime datum)
        {
            ID = id;
            Nazev = nazev;
            Popis = popis;
            Castka = castka;
            TypOperace = typOperace;

            Matka = matka;
            CisloVrhu = vrh;
            Datum = datum;
        }

        /// <summary>
        /// Prázdný konstruktor pro XML serializer
        /// </summary>
        public Finance()
        {

        }

        /// <summary>
        /// Přetížená metoda pro výpis hodnot do ListBoxu
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}    \t{1}\t{2}\t{3}", Nazev, Datum.ToShortDateString(),Castka,CisloVrhu);
        }

    }
}
