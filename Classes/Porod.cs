using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceZvirat
{
    public class Porod
    {
        // ************************************************************* Parametry pro PRASE i OVCE *************************************************************
        /// <summary>
        /// Jedinečný identifikátor porodu
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Živých selat - jehňat
        /// </summary>
        public int Ziva { get; set; }
        /// <summary>
        /// Mrtvých selat - jehňat
        /// </summary>
        public int Mrtva { get; set; }
        /// <summary>
        /// Odchovaných selat - jehňat
        /// </summary>
        public int Odchovana { get; set; }
        /// <summary>
        /// Detailnější popis k porodu
        /// </summary>
        public string PrubehPorodu { get; set; }
        /// <summary>
        /// Skutečné datum porodu zvířete
        /// </summary>
        public DateTime DatumNarozeni { get; set; }

        // ************************************************************* PRASE parametry *************************************************************
        /// <summary>
        /// Kontrola zda je zvíře zabřeznuté 
        /// </summary>
        public bool KontrolaBrezosti { get; set; }
        /// <summary>
        /// Prasnice
        /// </summary>
        public Svine Matka { get; set; }
        /// <summary>
        /// Předpokládané datum porodu
        /// </summary>
        public DateTime DatumPorodu { get; set; }
        private DateTime datumZapusteni;
        /// <summary>
        /// Datum inseminace zvířete
        /// </summary>
        public DateTime DatumZapusteni
        {
            get
            {
                return datumZapusteni;
            }

            set
            {
                datumZapusteni = value.Date;
                DatumPorodu = value.AddDays(115);
                DatumTestuBrezosti = value.AddDays(40);
            }
        }
        /// <summary>
        /// Kontrola zda je prasnice v březosti
        /// </summary>
        public DateTime DatumTestuBrezosti { get; set; }
        // ************************************************************* Parametry OVCE *************************************************************
        /// <summary>
        /// Evidenční číslo matky - ovce
        /// </summary>
        public string MatkaEvidencniCislo { get; set; }
        /// <summary>
        /// Předpokládaný měsíc porodu
        /// </summary>
        public int MesicPorodu { get; set; }
        /// <summary>
        /// Měsíc vpůštění berana do výběhu
        /// </summary>
        public int MesicVpusteniBerana { get; set; }

        /// <summary>
        /// Konstruktor pro Prase
        /// </summary>
        /// <param name="ziva">Zivych selatek</param>
        /// <param name="mrtva">Mrtvych selatek</param>
        /// <param name="odchov">Odchovanych selatek</param>
        /// <param name="matka">Svine matka</param>
        /// <param name="prubehPorod">Textovy popis prubehu porodu</param>
        /// <param name="kontrolaBrezosti">Check zda je svine brezi</param>
        /// <param name="datZapusteni">Datum zapusteni svine</param>
        /// <param name="datumNarozeni">Skutecne datum narozeni selatek</param>
        public Porod(int ziva, int mrtva, int odchov, Svine matka, string prubehPorod, bool kontrolaBrezosti, 
                     DateTime datZapusteni, DateTime datumNarozeni)
        {
            Ziva = ziva;
            Mrtva = mrtva;
            Odchovana = odchov;
            Matka = matka;
            PrubehPorodu = prubehPorod;
            KontrolaBrezosti = kontrolaBrezosti;
            DatumZapusteni = datZapusteni;
            DatumNarozeni = datumNarozeni;
        }

        /// <summary>
        /// Konstruktor pro OVCE
        /// </summary>
        /// <param name="ziva">Zivych jehnat</param>
        /// <param name="mrtva">Mrtvych jehnat</param>
        /// <param name="odchov">Odchovanych jehnat</param>
        /// <param name="matka">Matka ovce</param>
        /// <param name="prubehPorod">Textovy popis prubehu porodu</param>
        /// <param name="mesicZapusteni">Mesic kdy byl beran vpusten do ohrady</param>
        /// <param name="mesicPorodu">Predpokladany mesic porodu</param>
        public Porod(int ziva, int mrtva, int odchov, string evidencniCislo, string prubehPorod, int mesicZapusteni,
                     int mesicPorodu, DateTime datumNarozeni,byte nicProOdliseni)
        {
            Ziva = ziva;
            Mrtva = mrtva;
            Odchovana = odchov;
            MatkaEvidencniCislo = evidencniCislo;
            PrubehPorodu = prubehPorod;
            MesicVpusteniBerana = mesicZapusteni;
            MesicPorodu = mesicPorodu;
            DatumNarozeni = datumNarozeni;
        }

        /// <summary>
        /// Konstruktor pro OVCI
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evidencniCislo"></param>
        /// <param name="mesicVpusteni"></param>
        /// <param name="mesicNarozeni"></param>
        public Porod(int id,string evidencniCislo,int mesicVpusteni, int mesicNarozeni)
        {
            ID = id;
            MatkaEvidencniCislo = evidencniCislo;
            MesicVpusteniBerana = mesicVpusteni;
            MesicPorodu = mesicNarozeni;
        }

        /// <summary>
        /// Konstruktor pro PRASE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="matka"></param>
        /// <param name="datZapusteni"></param>
        public Porod(int id, Svine matka, DateTime datZapusteni)
        {
            ID = id;
            Matka = matka;
            DatumZapusteni = datZapusteni;
        }

        /// <summary>
        /// Prázdný konstuktor pro XML serializer
        /// </summary>
        public Porod()
        {

        }

        public override string ToString()
        {
            return ID.ToString();
        }

    }
}
