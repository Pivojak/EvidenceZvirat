using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceZvirat
{
    public class Ovce: INotifyPropertyChanged
    {
        /// <summary>
        /// Událost vyvolaná změnou v některém z parametrů
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Jedinečný identifikátor OVCE
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Pojmenování ovce chovatelem
        /// </summary>
        public string Jmeno { get; set; }
        /// <summary>
        /// Evidenční číslo ovce, dle chovatele
        /// </summary>
        public string EvidencniCislo { get; set; }
        /// <summary>
        /// Datum narození 
        /// </summary>
        public DateTime DatumNarozeni { get; set; }
        /// <summary>
        /// Bahnice / beran ** jehnička / beránek
        /// </summary>
        public byte Pohlavi { get; set; }
        /// <summary>
        /// Datum posledního stříhání
        /// </summary>
        public DateTime DatumStrihani { get; set; }
        /// <summary>
        /// Instance ovce, jež je OTCEM 
        /// </summary>
        public Ovce Otec { get; set; }
        /// <summary>
        /// Instance ovce, jež je MATKOU
        /// </summary>
        public Ovce Matka { get; set; }
        /// <summary>
        /// Zda se jedna o jehne nebo o dospelou ovci. 0 - jehne, 1 - dospela
        /// </summary>
        public byte Stav { get; set; }
        /// <summary>
        /// Celkový počet živých jehňat při porodech
        /// </summary>
        public int ZivychJehnat { get; set; }
        /// <summary>
        /// Celkový počet mrtvých jehňat při porodech
        /// </summary>
        public int MrtvychJehnat { get; set; }
        /// <summary>
        /// Celkový počet odchovaných jehňat při odstavech
        /// </summary>
        public int OdchovanychJehnat { get; set; }
        /// <summary>
        /// Datum zařazení do chovu
        /// </summary>
        public DateTime ZarazeniDoChovu { get; set; }
        /// <summary>
        /// Datum vyřazení ovce z chovu
        /// </summary>
        public DateTime VyrazeniZChovu { get; set; }
        /// <summary>
        /// Důvod vyřezní ovce z chovu
        /// </summary>
        public string DuvodVyrazeni { get; set; }
        /// <summary>
        /// Název LINIE
        /// </summary>
        public string Linie { get; set; }
        /// <summary>
        /// Kvalitativní kvalifikace OVCE
        /// </summary>
        public string Klasifikace { get; set; }
        /// <summary>
        /// Detailnější informace k ovci
        /// </summary>
        public string Popis { get; set; }

        /// <summary>
        /// Metoda, která vyvolá událost změny
        /// </summary>
        /// <param name="vlasnost"></param>
        public void VyvolejZmenu(string vlasnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlasnost));
        }

        /// <summary>
        /// Prvotní přiřazení jehněte
        /// </summary>
        /// <param name="evidencniCislo">Evidenční číslo ovce, dle chovatele</param>
        public Ovce(string evidencniCislo)
        {
            EvidencniCislo = evidencniCislo;
        }

        /// <summary>
        /// Druhá možnost prvotního přiřazení ovce
        /// </summary>
        /// <param name="id">Jedinečný identifikátor OVCE</param>
        /// <param name="evidencniCislo">Evidenční číslo ovce, dle chovatele</param>
        /// <param name="datumNarozeni">Datum narození OVCE</param>
        public Ovce(int id,string evidencniCislo, DateTime datumNarozeni)
        {
            EvidencniCislo = evidencniCislo;
            DatumNarozeni = datumNarozeni;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Jedinečný identifikátor OVCE</param>
        /// <param name="evidencniCislo">Evidenční číslo ovce, dle chovatele</param>
        /// <param name="datumNarozeni">Datum narození OVCE</param>
        /// <param name="matka">Instance ovce, jež je MATKOU</param>
        /// <param name="otec">Instance ovce, jež je OTEC</param>
        /// <param name="zarazeniChov">Datum zařazení do chovu</param>
        /// <param name="stav">Zda se jedna o jehne nebo o dospelou ovci. 0 - jehne, 1 - dospela</param>
        /// <param name="pohlavi">Bahnice / beran ** jehnička / beránek</param>
        /// <param name="popis">Detailnější informace k ovci</param>
        public Ovce(int id,string evidencniCislo, DateTime datumNarozeni, Ovce matka, Ovce otec, 
        DateTime zarazeniChov, byte stav, byte pohlavi, string popis)
        {
            EvidencniCislo = evidencniCislo;
            DatumNarozeni = datumNarozeni;
            Matka = matka;
            Otec = otec;
            ZarazeniDoChovu = zarazeniChov;
            Stav = stav;
            Pohlavi = pohlavi;
            ID = id;
            Popis = popis;
        }

        /// <summary>
        /// Prázdný konstruktor pro XML seriliazer
        /// </summary>
        public Ovce()
        {

        }

        /// <summary>
        /// Přetížená metoda pro výpis do ListBoxu
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}\t{1}",EvidencniCislo,DatumNarozeni.ToShortDateString());
        }
    }
}
