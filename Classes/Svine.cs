using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceZvirat
{
    /// <summary>
    /// Třída pro PRASNICI, která obohacuje základní třídu prase
    /// </summary>
    public class Svine : Prase
    {
        /// <summary>
        /// Událost při obnovení dat
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Celkový počet živých selat při porodech
        /// </summary>
        public int PocetSelat_zivych { get; set; }
        /// <summary>
        /// Celkový počet mrtvých selat při porodech
        /// </summary>
        public int PocetSelat_mrtvych { get; set; }
        /// <summary>
        /// Celkový počet odchovaných selat při odstavech
        /// </summary>
        public int PocerSelat_odchovanych { get; set; }
        /// <summary>
        /// Definice krmiva pro zvíře
        /// </summary>
        public string Krmivo { get; set; }
        /// <summary>
        /// Průměrný počet selat
        /// </summary>
        public double PrumerSelat { get; set; } 

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="jmeno">Pojmenování zvířete</param>
        /// <param name="poradoveCislo">Evidenční číslo zvířete</param>
        /// <param name="datumNarozeni">Datum narození zvířete</param>
        public Svine(string jmeno,int poradoveCislo, DateTime datumNarozeni) : base(poradoveCislo, datumNarozeni)
        {
            this.Jmeno = jmeno;
            this.EvidencniCislo = poradoveCislo;
            this.DatumNarozeni = datumNarozeni;
        }

        /// <summary>
        /// Prázdný konstruktor pro XML serializer
        /// </summary>
        public Svine()
        {

        }

        /// <summary>
        /// Metoda vyvolávající událost
        /// </summary>
        /// <param name="vlastnost"></param>
        public void VyvolejZmenu(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }



    }
}


 

