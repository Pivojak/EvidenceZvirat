using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EvidenceZvirat
{
    public class SpravceZvirat: INotifyPropertyChanged
    {
        #region Konstruktor / atributy / kolekce
        public event PropertyChangedEventHandler PropertyChanged;

        // ***********************************************  Kolekce pro PRASE ***********************************************
        /// <summary>
        /// Kolekce všech selat, tedy pro obě PRASNICE
        /// </summary>
        public ObservableCollection<Prase> SeznamSelat { get; set; }
        /// <summary>
        /// Veterinární záznamy pro všechna Prasat i Prasnice
        /// </summary>
        public ObservableCollection<Veterina> SeznamVeterinarnichZaznamu_Prase { get; set; }
        /// <summary>
        /// Vybrané veterinární záznamy pro konkrétní PRASE
        /// </summary>
        public ObservableCollection<Veterina> ZaznamySelete { get; set; }
        /// <summary>
        /// Kolekce všech porodů PRASNIC
        /// </summary>
        public ObservableCollection<Porod> PorodyPrase { get; set; }
        /// <summary>
        /// Vytříděnná selata pro první PRASNICI - chovatel pojmenoval jako MARUSKA
        /// </summary>
        public ObservableCollection<Prase> SeleMaruska { get; set; }
        /// <summary>
        /// Vytříděnná selata pro druhou PRASNICI - chovatel pojmenoval jako BARUSKA
        /// </summary>
        public ObservableCollection<Prase> SeleBaruska { get; set; }
        /// <summary>
        /// Všechny výdajové transakce pro PRASE
        /// </summary>
        public ObservableCollection<Finance> VydajePrase { get; set; }
        /// <summary>
        /// Všechny příjmové transakce pro PRASE
        /// </summary>
        public ObservableCollection<Finance> PrijmyPrase { get; set; }

        // ***********************************************  Kolekce pro OVCE ***********************************************
        /// <summary>
        /// Kolekce pro uložení dospělých OVCI
        /// </summary>
        public ObservableCollection<Ovce> SeznamOvci { get; set; }
        /// <summary>
        /// Kolekce pro uložení JEHŇAT
        /// </summary>
        public ObservableCollection<Ovce> SeznamJehnat { get; set; }
        /// <summary>
        /// Kolekce pro uložení BERANU
        /// </summary>
        public ObservableCollection<Ovce> SeznamBeranu { get; set; }
        /// <summary>
        /// Kolekce pro všechy porody
        /// </summary>
        public ObservableCollection<Porod> PorodyOvce { get; set; }
        /// <summary>
        /// Kolekce pro všechy veterinární záznamy
        /// </summary>
        public ObservableCollection<Veterina> SeznamVeterinarnichZaznamu_Ovce { get; set; }
        /// <summary>
        /// Kolekce pro výdajové transakce
        /// </summary>
        public ObservableCollection<Finance> VydajeOvce { get; set; }
        /// <summary>
        /// Kolekce pro příjmové transakce 
        /// </summary>
        public ObservableCollection<Finance> PrijmyOvce { get; set; }
        /// <summary>
        /// Kolekce pro všecha odčervení 
        /// </summary>
        public ObservableCollection<Odcerveni> SeznamOdcerveni { get; set;}
        /// <summary>
        /// Kolekce pro všechna očkování
        /// </summary>
        public ObservableCollection<Ockovani> SeznamOckovani { get; set; }

        // ***********************************************  Cesty pro uložení dat na C/user/appData ***********************************************
        private string cestaOvce;
        private string cestaJehnata;
        private string cestaBerani;
        private string cestaPrase;
        private string cestaVeterina;
        private string cestaVeterina_Ovce;
        private string cestaPrijmyPrase;
        private string cestaVydajePrase;
        private string cestaPrijmyOvce;
        private string cestaVydajeOvce;
        private string cestaID;
        private string cestaPorodyPrase;
        private string cestaPorodyOvce;
        private string cestaOckovani;
        private string cestaOdcerveni;
        private string cesta = "";

        /// <summary>
        /// Uložená hodnota zisku OVCE
        /// </summary>
        public double ZiskOvce { get; set; }
        /// <summary>
        /// Hodnota zisku PRASE
        /// </summary>
        public double ZiskPrase { get; set; }
        /// <summary>
        /// První PRASNICE projektu - chovatel pojmenoval jako MARUSKA
        /// </summary>
        public Svine Maruska { get; set; }
        /// <summary>
        /// Druhá PRASNICE projektu - chovatel pojmenoval jako BARUSKA
        /// </summary>
        public Svine Baruska { get; set; }
        private Ovce matka_pomocna;
        private Ovce otec_pomocna;

        // ***********************************************  Identifikátory pro všechny třídy ***********************************************
        public int ID_Porod_Prase { get; set; }
        public int ID_Porod_Ovce { get; set; }
        public int ID_Sele { get; set; }
        public int ID_Ovce { get; set; }
        public int ID_Jehne { get; set; }
        public int ID_Beran { get; set; }
        public int ID_Veterina_Prase { get; set; }
        public int ID_Finance_Prase { get; set; }
        public int ID_Finance_Ovce { get; set; }
        public int ID_Veterina_Ovce { get; set; }
        public int ID_Odcerveni { get; set; }
        public int ID_Ockovani{ get; set; }

        /// <summary>
        /// Základní konstruktor správce aplikace
        /// </summary>
        public SpravceZvirat()
        {
            // Počáteční hodnoty ID - přidalších startech budou přemazány hodnotami ze souboru
            ID_Finance_Prase = 0;
            ID_Finance_Ovce = 0;
            ID_Ovce = 0;
            ID_Jehne = 0;
            ID_Beran = 0;
            ID_Porod_Prase = 0;
            ID_Sele = 2;
            ID_Veterina_Prase = 0;
            ID_Veterina_Ovce = 0;
            ID_Odcerveni = 0;
            ID_Ockovani = 0;
            ID_Porod_Ovce = 0;

            // Vytvoření cesty do složky AppData na disku C - do složky EvidenceZvirat, ktera se případně vytvoří
            try
            {
                cesta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EvidenceZvirat");
                if (!Directory.Exists(cesta))
                    Directory.CreateDirectory(cesta);
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", cesta);
            }

            // Uložení cest ke všem souborům obsahujícím všechny kolekce projektu - složka EvidenceZvirat v AppData na disku C
            cestaOvce = Path.Combine(cesta,"ovce.xml");
            cestaJehnata = Path.Combine(cesta, "jehnata.xml");
            cestaBerani = Path.Combine(cesta, "berani.xml");
            cestaPrase = Path.Combine(cesta, "prase.xml");
            cestaVeterina = Path.Combine(cesta, "veterina.xml");
            cestaVeterina_Ovce = Path.Combine(cesta, "vetrina_ovce.xml");
            cestaPrijmyPrase = Path.Combine(cesta, "prijmyPrase.xml");
            cestaVydajePrase = Path.Combine(cesta, "vydajePrase.xml");
            cestaPrijmyOvce = Path.Combine(cesta, "prijmyOvce.xml");
            cestaVydajeOvce = Path.Combine(cesta, "vydajeOvce.xml");
            cestaID = Path.Combine(cesta, "id.xml");
            cestaPorodyPrase = Path.Combine(cesta, "porodyPrase.xml");
            cestaPorodyOvce = Path.Combine(cesta, "porodyOvce.xml");
            cestaOckovani = Path.Combine(cesta, "ockovani.xml");
            cestaOdcerveni = Path.Combine(cesta, "odcerveni.xml");

            // PRASE
            SeznamSelat = new ObservableCollection<Prase>();
            SeznamVeterinarnichZaznamu_Prase = new ObservableCollection<Veterina>();
            ZaznamySelete = new ObservableCollection<Veterina>();
            SeleBaruska = new ObservableCollection<Prase>();
            SeleMaruska = new ObservableCollection<Prase>();
            VydajePrase = new ObservableCollection<Finance>();
            PrijmyPrase = new ObservableCollection<Finance>();
            PorodyPrase = new ObservableCollection<Porod>();

            // OVCE
            SeznamOvci = new ObservableCollection<Ovce>();
            SeznamJehnat = new ObservableCollection<Ovce>();
            SeznamBeranu = new ObservableCollection<Ovce>();
            SeznamVeterinarnichZaznamu_Ovce = new ObservableCollection<Veterina>();
            VydajeOvce = new ObservableCollection<Finance>();
            PrijmyOvce = new ObservableCollection<Finance>();
            PorodyOvce = new ObservableCollection<Porod>();
            SeznamOckovani = new ObservableCollection<Ockovani>();
            SeznamOdcerveni = new ObservableCollection<Odcerveni>();

            // Definice dvou PRASNIC, které má chovatel
            
            Maruska = new Svine("Maruska",0, new DateTime(2017,5,1));
            Baruska = new Svine("Baruska",1, new DateTime(2018, 5, 1));

            // Prazdne ovce pro ulozeni zakladnich ovci, jejich matky a ovce
            matka_pomocna = new Ovce(ID_Ovce,"0", new DateTime(1, 1, 1));
            ID_Ovce++;
            otec_pomocna = new Ovce(ID_Beran,"0", new DateTime(1, 1, 1));
            ID_Beran++;
        }

        /// <summary>
        /// Metoda, ktera dokaze vyvolat zmenu na Observaiblle Collection a tim zmenit seznam
        /// </summary>
        /// <param name="vlastnost">Nazev zmeneneho atributu, vlastnosti</param>
        protected void VyvolejZmenu(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }

        #endregion

        // **************************************

        #region Finance

        /// <summary>
        /// Spočítá zisk pro daný druh zvířete
        /// </summary>
        /// <param name="zvire">0 - Prase, 1 - Ovce</param>
        public void SpocitejZisk(byte zvire)
        {
            double sumaVydej = 0;
            double sumaPrijem = 0;
            bool pocitej = true;
            ObservableCollection<Finance> Vydaje = new ObservableCollection<Finance>();
            ObservableCollection<Finance> Prijmy = new ObservableCollection<Finance>();
            // Prase - přiřezení požadovaných kolekcí
            if (zvire == 0)
            {
                Vydaje = VydajePrase;
                Prijmy = PrijmyPrase;
            }
            // Ovce - přiřezení požadovaných kolekcí
            else if (zvire == 1)
            {
                Vydaje = VydajeOvce;
                Prijmy = PrijmyOvce;
            }
            else
                pocitej = false;

            // Výpočet zisků
            if (Vydaje != null && Prijmy != null && pocitej)
            {
                foreach (Finance d in Vydaje)
                    sumaVydej += d.Castka;

                foreach (Finance d in Prijmy)
                    sumaPrijem += d.Castka;
                if (zvire == 0)
                    ZiskPrase = sumaPrijem - sumaVydej;
                else if (zvire == 1)
                    ZiskOvce = sumaPrijem - sumaVydej;
            }
            // Vyvolání metody a následně události, která oznamuje změnu v Atributech, aby se data na formuláři aktualizovali
            VyvolejZmenu("ZiskPrase");
            VyvolejZmenu("ZiskOvce");
        }

        /// <summary>
        /// Metoda spocita naklady na dany vrh, pro danou matku apod - pro OVCE i PRASE
        /// </summary>
        /// <param name="typBilance">0 - Vydaje, 1 - Prijmy</param>
        /// <param name="zvire">0 - Prase, 1 - Ovce</param>
        /// <param name="konkretniZvire">0 - Marus, 1 - Barus, 2 - Ostatni, 3 - Vrh, 4 - Ovce, 5 - Rok</param>
        /// <param name="evidencniCislo">Evidencni cislo ovce</param>
        /// <param name="cisloVrhu">Cislo vrhu</param>
        /// <returns></returns>
        public double SpocitejBilanci(byte typBilance, byte zvire, byte konkretniZvire, string evidencniCislo, byte cisloVrhu, int cisloRoku)
        {
            ObservableCollection<Finance> seznam = new ObservableCollection<Finance>();
            string pocitanyObjekt = "";
            double sumaVydaje = 0;
            // Výdaje
            if (typBilance == 0)
            {
                // Prase
                if (zvire == 0)
                {
                    seznam = VydajePrase;
                    // První prasnice
                    if (konkretniZvire == 0)
                        pocitanyObjekt = "maruska";
                    // Druhá prasnice
                    else if (konkretniZvire == 1)
                        pocitanyObjekt = "baruska";
                    // Vedlejší výdaje - stavby apod.
                    else if (konkretniZvire == 2)
                        pocitanyObjekt = "ostatni";

                }
                // Ovce
                else if (zvire == 1)
                {
                    seznam = VydajeOvce;
                    pocitanyObjekt = evidencniCislo;
                    // Ostatní - stavby apod
                    if (konkretniZvire == 2)
                        pocitanyObjekt = "ostatni";
                }
            }
            // Příjmy
            else if (typBilance == 1)
            {
                // Prase
                if (zvire == 0)
                {
                    seznam = PrijmyPrase;
                    if (konkretniZvire == 0)
                        pocitanyObjekt = "maruska";

                    else if (konkretniZvire == 1)
                        pocitanyObjekt = "baruska";
                    else if (konkretniZvire == 2)
                        pocitanyObjekt = "ostatni";

                }
                // Ovce
                else if (zvire == 1)
                {
                    seznam = PrijmyOvce;
                    pocitanyObjekt = evidencniCislo;
                    if (konkretniZvire == 2)
                        pocitanyObjekt = "ostatni";
                }
            }
            // Vypocet Bilance pro danou PRASNICI, ostatní (stavby apod.) nebo OVCI
            if (konkretniZvire == 0 || konkretniZvire == 1 || konkretniZvire == 2 || konkretniZvire == 4)
            {
                foreach (Finance f in seznam)
                {
                    if (f.Matka == pocitanyObjekt)
                        sumaVydaje += f.Castka;
                }

            }

            // Vypocet bilance pro dany rok pro OVCE
            if (cisloRoku > 0 && konkretniZvire == 5)
            {
                foreach (Finance f in seznam)
                {
                    if (f.Datum.Year == cisloRoku)
                        sumaVydaje += f.Castka;
                }
            }

            // Vypocet bilance pro dany vrh
            // Nebo tako vypocet pro vybranou generaci u ovci
            if (cisloVrhu != 0 && konkretniZvire == 3)
            {
                foreach (Finance f in seznam)
                {
                    if (f.CisloVrhu == cisloVrhu)
                        sumaVydaje += f.Castka;
                }
            }
            return sumaVydaje;
        }

        /// <summary>
        /// Metoda, ktera pracuje s financnimi zaznamy jak pro PRASE, tak pro OVCE
        /// </summary>
        /// <param name="naz">Nazev transakce</param>
        /// <param name="pop">Popis transakce</param>
        /// <param name="cast">Castka v CZK</param>
        /// <param name="matka">Matka nebo vztazna ovce, tedy jeji EV. cislo</param>
        /// <param name="vrh">Vrh u prasat nebo generace u ovci, ke které se má transakce vztahovat</param>
        /// <param name="datum">Datum transakce</param>
        /// <param name="typ">0 - Vydaje, 1 - Prijmy</param>
        /// <param name="zvire">0 - Prase, 1 - Ovce</param>
        /// <param name="operace">0 - Novy zaznam, 1 - Uprava stavajiciho</param>
        /// <param name="zaznam">Financni zaznam pro upravu stavajiciho</param>
        public bool ZadejFinanceZaznam(string naz, string pop, string cast, string matka, string vrh, string datum, byte typ, byte zvire, byte operace, Finance zaznam)
        {
            // pomocná kolekce, které se přiřadí reference na požadovanou kolekci na dalších řádcích
            ObservableCollection<Finance> seznam = new ObservableCollection<Finance>();

            byte vrhPomocna = 0;
            bool odpoved = false;
            double castkaPomocna = 0;
            DateTime datumPomocna;
            // Prase
            if (zvire == 0)
            {
                // Výdaje
                if (typ == 0)
                    seznam = VydajePrase;
                else
                    seznam = PrijmyPrase;
            }
            // Ovce
            else if (zvire == 1)
            {
                // Výdaje
                if (typ == 0)
                    seznam = VydajeOvce;
                else
                    seznam = PrijmyOvce;
            }

            // ---------------------- Ošetření vstupů pomocí vyjímek ----------------------
            if (datum == "")
                throw new ArgumentException("Nezadal jsi žádné datum transakce.");
            else
                datumPomocna = DateTime.Parse(datum).Date;
            if (datumPomocna > DateTime.Now)
                throw new ArgumentException("Zadal jsi datum z budoucnosti.");
            if (naz == "")
                throw new ArgumentException("Nezadal jsi žádný název transakce.");
            if (cast == "")
                throw new ArgumentException("Nezadal jsi žádnou částku.");
            if (pop == "")
                pop = "Nic";
            // Vrh u prasatek | Generace pro ovce
            if (vrh != "")
                vrhPomocna = byte.Parse(vrh);
            if (cast != "")
                castkaPomocna = double.Parse(cast);
            // Kontrola a doplneni pro prase
            if (zvire == 0)
            {
                if (matka.StartsWith("mar"))
                    matka = "maruska";
                if (matka.StartsWith("bar"))
                    matka = "baruska";
                if (matka != "maruska" && matka != "baruska" && vrhPomocna == 0)
                    matka = "ostatni";
            }
            // Kontrola a doplneni pro OVCE
            if (zvire == 1)
            {

                if (matka == "" && vrhPomocna == 0)
                    matka = "ostatni";
                if (matka == "" && vrhPomocna > 0)
                    matka = "zadna";
            }

            int pomocna;

            // Vytvoreni noveho zaznamu
            if (operace == 0)
            {
                int ID_Finance = 0;
                // Prase
                if (zvire == 0)
                {
                    ID_Finance = ID_Finance_Prase;
                    ID_Finance_Prase++;
                }
                // Ovce
                else if (zvire == 1)
                {
                    ID_Finance = ID_Finance_Ovce;
                    ID_Finance_Ovce++;
                }
                // Přidání záznamu do kolekce
                seznam.Add(new Finance(ID_Finance, naz, pop, castkaPomocna, matka, vrhPomocna, typ, datumPomocna));
                odpoved = true;
            }
            // Úprava stavajiciho zaznamu    
            else if (operace == 1)
            {
                pomocna = seznam.IndexOf(zaznam);
                seznam[pomocna].CisloVrhu = vrhPomocna;
                seznam[pomocna].Castka = castkaPomocna;
                seznam[pomocna].Popis = pop;
                seznam[pomocna].Matka = matka;
                seznam[pomocna].Nazev = naz;
                seznam[pomocna].Datum = datumPomocna;
                odpoved = true;
            }
            VyvolejZmenu("VydajeOvce");
            VyvolejZmenu("PrijmyOvce");
            VyvolejZmenu("VydajePrase");
            VyvolejZmenu("PrijmyPrase");

            UlozVydaje(0);
            UlozVydaje(1);
            UlozPrijmy(0);
            UlozPrijmy(1);
            UlozID();
            return odpoved;
        }

        /// <summary>
        /// Odebrání záznamu ze seznamu financí (pro Prase či Ovci - Příjem či Výdaj)
        /// </summary>
        /// <param name="zaznam">Object FINANCE, tedy vybraný záznam z ListBox</param>
        /// <param name="zvire">0 - Prase, 1 - Ovce</param>
        /// <param name="typ">0 - Výdaj, 1 - Příjem</param>
        public bool OdeberFinancniZaznam(Finance zaznam, byte zvire, byte typ)
        {
            bool odpoved = false;
            ObservableCollection<Finance> seznam = new ObservableCollection<Finance>();
            if (zvire == 0)
            {
                if (typ == 0)
                    seznam = VydajePrase;
                else
                    seznam = PrijmyPrase;
            }
            else if (zvire == 1)
            {
                if (typ == 0)
                    seznam = VydajeOvce;
                else
                    seznam = PrijmyOvce;
            }
            if (zaznam != null)
            {
                int zalohaID = seznam.IndexOf(zaznam);
                seznam.Remove(zaznam);
                foreach (Finance f in seznam)
                {
                    if (f.ID > zalohaID)
                        f.ID--;
                }
                if (zvire == 0)
                    ID_Finance_Prase--;
                else if (zvire == 1)
                    ID_Finance_Ovce--;
                VyvolejZmenu("VydajeOvce");
                VyvolejZmenu("PrijmyOvce");
                VyvolejZmenu("VydajePrase");
                VyvolejZmenu("PrijmyPrase");

                UlozVydaje(0);
                UlozVydaje(1);
                UlozPrijmy(0);
                UlozPrijmy(1);
                UlozID();
                odpoved = true;
            }

            return odpoved;
        }

        /// <summary>
        /// Seřadí příjmy a výdaje pro PRASE sestupně, tedy poslední nahoře
        /// </summary>
        public void SeradFinance()
        {
            // Výdaje
            IEnumerable<Finance> serazeny = VydajePrase.OrderByDescending(a => a.ID);
            VydajePrase = new ObservableCollection<Finance>();
            foreach (Finance f in serazeny)
            {
                VydajePrase.Add(f);
            }
            // Příjmy
            IEnumerable<Finance> serazeny2 = PrijmyPrase.OrderByDescending(a => a.ID);
            PrijmyPrase = new ObservableCollection<Finance>();
            foreach (Finance f in serazeny2)
            {
                PrijmyPrase.Add(f);
            }
            VyvolejZmenu("VydajePrase");
            VyvolejZmenu("PrijmyPrase");
        }

        #endregion

        // **************************************

        #region Ovce

        /// <summary>
        /// Metoda pro práci s ovcí - Přidání, úprava, odebrání nebo změna mladé ovce na matku 
        /// </summary>
        /// <param name="operace">0 - Nová ovce, 1 - Úprava, 2 - Odebrání, 3 - Mladá ovce se stane MATKOU</param>
        /// <param name="stav">0 - Jehně, 1 - Dospělá ovce</param>
        /// <param name="pohlavi">0 - Žena, 1 - Muž</param>
        /// <param name="jmeno">Pojmenování ovce chovatelem</param>
        /// <param name="evidencniCislo">Evidenční číslo ovce</param>
        /// <param name="datStrihani">Datum posledního stříhání ovce</param>
        /// <param name="datZarazeni">Datum zařezení ovce do chovu</param>
        /// <param name="datVyrazeni">Datum vyřazení z chovu</param>
        /// <param name="datNarozeni">Datum narození</param>
        /// <param name="upravovanaOvce">Upravovaná ovce</param>
        /// <param name="linie">Linie - rodokmenová</param>
        /// <param name="duvodVyrazeni">Dúvod vyřazení z chovu</param>
        /// <param name="klasifikace">Kvalita ovce - kvalifikace</param>
        /// <param name="popis">Detailnější popis zvířete</param>
        /// <param name="matka_cislo">Evidenční číslo matky</param>
        /// <param name="otec_cislo">Evidenční číslo otce</param>
        /// <param name="mrtvychJehnat">Mrtvých jehňat při porodu</param>
        /// <param name="zivychJehnat">Živých jehňat při porodu</param>
        /// <param name="odchovJehnat">Odchovaných jehňat</param>
        /// <returns></returns>
        public bool ZadejOvci(byte operace, byte stav, byte pohlavi, string jmeno, string evidencniCislo,
                              string datStrihani, string datZarazeni, string datVyrazeni, string datNarozeni,
                              Ovce upravovanaOvce, string linie, string duvodVyrazeni, string klasifikace,
                              string popis, string matka_cislo, string otec_cislo, string mrtvychJehnat, string zivychJehnat, string odchovJehnat)
        {
            // --------------- Pomocné proměnné ---------------
            ObservableCollection<Ovce> seznam = new ObservableCollection<Ovce>();
            DateTime datumNarozeni_pomocna;
            DateTime datumZarazeni_pomocna;
            DateTime datumVyrazeni_pomocna;
            DateTime datumStrihani_pomocna;

            Ovce matka = null;
            Ovce otec = null;

            int zivychJehnat_pomocna = 0;
            int mrtvychJehnat_pomocna = 0;
            int odchovanychJehnat_pomocna = 0;
            bool odpoved = false;
            int zalohaID = 0;
            string pomocna = "";

            // Mladá ovce
            if (stav == 0)
            {
                zalohaID = ID_Jehne;
                seznam = SeznamJehnat;
                pomocna = "SeznamJehnat";
            }
            // Dospelá ovce, beran
            else if (stav == 1)
            {
                // Muž - 1, BERAN
                if (pohlavi == 1)
                {
                    zalohaID = ID_Beran;
                    seznam = SeznamBeranu;
                    pomocna = "SeznamBeranu";
                }
                // Žena - 0, OVCE
                else if (pohlavi == 0)
                {
                    zalohaID = ID_Ovce;
                    seznam = SeznamOvci;
                    pomocna = "SeznamOvci";
                }

            }
            // Přidání nové nebo úprava stávající ovce
            if (operace == 0 || operace == 1)
            {
                // Trimovani a převedení na malá písmena vstupních stringu u ev. čísla, a čísla matky a otce
                if (otec_cislo != "")
                    otec_cislo = otec_cislo.Trim().ToLower();
                if (matka_cislo != "")
                    matka_cislo = matka_cislo.Trim().ToLower();
                if (evidencniCislo != "")
                    evidencniCislo = evidencniCislo.Trim().ToLower();
                // Vyhledání konkrétní ovce - nezačíná 0 - tedy jedná o ovci, která je v chovu evidována
                //      - Ovce, která začíná 0... v evidenčním číslu, není evidována v chovu
                //      - například se může jednat o novou ovci, která nemá rodiče v chovu, ale je nutné evidovat jejich čísla, proto budou začínat 0
                if (!matka_cislo.StartsWith("0") && !otec_cislo.StartsWith("0"))
                {
                    foreach (Ovce o in SeznamOvci)
                    {
                        if (o.EvidencniCislo == matka_cislo)
                            matka = o;
                    }
                    foreach (Ovce o in SeznamBeranu)
                    {
                        if (o.EvidencniCislo == otec_cislo)
                            otec = o;
                    }
                }
                // Jedná se o ovci, která není v chovu viz. Výše
                else
                {
                    matka_cislo = matka_cislo.Remove(0, 1);
                    otec_cislo = otec_cislo.Remove(0, 1);
                    matka = new Ovce(matka_cislo);
                    otec = new Ovce(otec_cislo);
                }

                // ------------------------------- Ošetření vstupů -------------------------------
                // Evidencni cislo
                if (evidencniCislo == "")
                    throw new ArgumentException("Nezadal jsi pořadové číslo");
                // Datum narozeni
                if (datNarozeni == "")
                    throw new ArgumentException("Nezadal jsi žádné datum narození");
                else
                    datumNarozeni_pomocna = DateTime.Parse(datNarozeni);
                if (datumNarozeni_pomocna > DateTime.Now)
                    throw new ArgumentException("Zadal jsi datum narození z budoucnosti");
                // Datum zarazeni
                if (datZarazeni == "")
                    throw new ArgumentException("Nezadal jsi žádné datum zařazení");
                else
                    datumZarazeni_pomocna = DateTime.Parse(datZarazeni);
                // Matka
                if (matka_cislo == "")
                    throw new ArgumentException("Nezadal jsi žádnou matku");
                // Otec
                if (otec_cislo == "")
                    throw new ArgumentException("Nezadal jsi žádného otce");
                // Pohlavi a stav
                if (stav > 1 || stav < 0)
                    throw new ArgumentException("Nevybral jsi zda se jedna o jehně nebo o dospělou ovci");
                if (pohlavi > 1 || pohlavi < 0)
                    throw new ArgumentException("Nevybral jsi pohlaví ovce.");

                // Přidání nové ovce
                if (operace == 0)
                {
                    seznam.Add(new Ovce(zalohaID, evidencniCislo, datumNarozeni_pomocna, matka, otec, datumZarazeni_pomocna, stav, pohlavi, popis));
                    odpoved = true;
                    if (stav == 0)
                        ID_Jehne++;
                    else if (stav == 1)
                    {
                        if (pohlavi == 0)
                            ID_Ovce++;
                        else if (pohlavi == 1)
                            ID_Beran++;
                    }
                }
                // Úprava stávající
                else if (operace == 1)
                {
                    // U kazdeho parametru je kontrola, zda opravdu neco obsahuje, aby se nezapisovalo nic do hodnot
                    int a = seznam.IndexOf(upravovanaOvce);
                    seznam[a].DatumNarozeni = datumNarozeni_pomocna;
                    if (DateTime.TryParse(datStrihani, out datumStrihani_pomocna))
                        seznam[a].DatumStrihani = datumStrihani_pomocna;
                    if (DateTime.TryParse(datVyrazeni, out datumVyrazeni_pomocna))
                        seznam[a].VyrazeniZChovu = datumVyrazeni_pomocna;
                    if (datZarazeni != "")
                        seznam[a].ZarazeniDoChovu = datumZarazeni_pomocna;
                    if (duvodVyrazeni != "")
                        seznam[a].DuvodVyrazeni = duvodVyrazeni;
                    if (evidencniCislo != "")
                        seznam[a].EvidencniCislo = evidencniCislo;
                    if (jmeno != "")
                        seznam[a].Jmeno = jmeno;
                    if (klasifikace != "")
                        seznam[a].Klasifikace = klasifikace;
                    if (linie != "")
                        seznam[a].Linie = linie;
                    if (int.TryParse(mrtvychJehnat, out mrtvychJehnat_pomocna))
                        seznam[a].MrtvychJehnat = mrtvychJehnat_pomocna;
                    if (int.TryParse(odchovJehnat, out odchovanychJehnat_pomocna))
                        seznam[a].OdchovanychJehnat = odchovanychJehnat_pomocna;
                    if (int.TryParse(zivychJehnat, out zivychJehnat_pomocna))
                        seznam[a].ZivychJehnat = zivychJehnat_pomocna;
                    if (popis != "")
                        seznam[a].Popis = popis;
                    seznam[a].Pohlavi = pohlavi;
                    odpoved = true;
                }

            }
            // Smazani Ovce, Berana, Jehnete
            else if (operace == 2)
            {
                int pomocnaID = 0;
                if (stav == 0)
                    ID_Jehne--;
                else if (stav == 1)
                {
                    if (pohlavi == 0)
                        ID_Ovce--;
                    else if (pohlavi == 1)
                        ID_Beran--;
                }
                pomocnaID = seznam.IndexOf(upravovanaOvce);
                seznam.Remove(upravovanaOvce);
                foreach (Ovce o in seznam)
                {
                    if (o.ID > pomocnaID)
                        o.ID--;
                }
                odpoved = true;

            }
            // Podminka, pro zmenu jehnete na matku. Lze take zamenit beranka za BERANA
            else if (operace == 3)
            {
                int pomocnaID = SeznamJehnat.IndexOf(upravovanaOvce);
                SeznamJehnat.Remove(upravovanaOvce);
                ID_Jehne--;
                // Validace ID
                foreach (Ovce o in SeznamJehnat)
                {
                    if (o.ID > pomocnaID)
                        o.ID--;
                }
                // Zařazení do příslusné kolecke - OVCE nebo BERAN
                if (upravovanaOvce.Pohlavi == 0)
                {
                    SeznamOvci.Add(upravovanaOvce);

                    upravovanaOvce.ID = ID_Ovce;
                    ID_Ovce++;
                }

                else if (upravovanaOvce.Pohlavi == 1)
                {
                    SeznamBeranu.Add(upravovanaOvce);
                    upravovanaOvce.ID = ID_Beran;
                    ID_Beran++;
                }
                upravovanaOvce.Stav = stav;
                odpoved = true;
            }
            VyvolejZmenu(pomocna);
            UlozOvce();
            return odpoved;
        }

        /// <summary>
        /// Seřadí kolekci porodů pro OVCE sestupně, tedy poslední nahoře
        /// </summary>
        public void SeradOvceBeranyJehnata()
        {
            IEnumerable<Ovce> serazeny = SeznamOvci.OrderByDescending(a => a.ID);
            SeznamOvci = new ObservableCollection<Ovce>();
            foreach (Ovce f in serazeny)
            {
                SeznamOvci.Add(f);
            }
            VyvolejZmenu("SeznamOvci");

            IEnumerable<Ovce> serazeny2 = SeznamBeranu.OrderByDescending(a => a.ID);
            SeznamBeranu = new ObservableCollection<Ovce>();
            foreach (Ovce f in serazeny2)
            {
                SeznamBeranu.Add(f);
            }
            VyvolejZmenu("SeznamBeranu");

            IEnumerable<Ovce> serazeny3 = SeznamJehnat.OrderByDescending(a => a.ID);
            SeznamJehnat = new ObservableCollection<Ovce>();
            foreach (Ovce f in serazeny3)
            {
                SeznamJehnat.Add(f);
            }
            VyvolejZmenu("SeznamJehnat");
        }

        /// <summary>
        /// Výpočet celkových hodnot narozených, mrtvých a odchovaných selat u OVCE
        /// </summary>
        /// <param name="operace"></param>
        /// <param name="matkaOvce">Ovce pro, kterou se spočte počet jehňat</param>
        public void SpoctiPotomkyOvce(byte operace, Ovce matkaOvce)
        {
            int[] suma_puvodni = new int[3];
            suma_puvodni[0] = matkaOvce.ZivychJehnat;
            suma_puvodni[1] = matkaOvce.MrtvychJehnat;
            suma_puvodni[2] = matkaOvce.OdchovanychJehnat;
            int[] suma = new int[3];
            // Projdou se všechny záznamy o porodu
            foreach (Porod p in PorodyOvce)
            {
                // Pokud se číslo matky u porodu shoduje s čílem zadané ovce, odečtou se zadané hodnoty
                //      - Mrtvá, živá a odchovaná jehňata
                if (p.MatkaEvidencniCislo == matkaOvce.EvidencniCislo)
                {
                    if (p.Ziva > 0)
                        suma[0] += p.Ziva;

                    if (p.Mrtva > 0)
                        suma[1] += p.Mrtva;

                    if (p.Odchovana > 0)
                        suma[2] += p.Odchovana;
                }
            }
            // Pokud se data aktualizovali, nové hodnoty se uloží do atributů dané ovce
            if (suma_puvodni[0] != suma[0])
                matkaOvce.ZivychJehnat = suma[0];
            if (suma_puvodni[1] != suma[1])
                matkaOvce.MrtvychJehnat = suma[1];
            if (suma_puvodni[2] != suma[2])
                matkaOvce.OdchovanychJehnat = suma[2];
            // Aktualizace vlastností, aby se data obnovili i na formuláři
            matkaOvce.VyvolejZmenu("ZivychJehnat");
            matkaOvce.VyvolejZmenu("MrtvychJehnat");
            matkaOvce.VyvolejZmenu("OdchovanychJehnat");
        }

        /// <summary>
        /// Vybere jehňata dané ovce podle evidenčního čísla
        /// </summary>
        /// <param name="evidencniCislo">Evidencni cislo matky, pro kterou se hledaji jehnata</param>
        /// <returns></returns>
        public ObservableCollection<Ovce> VyberJehnata(string evidencniCislo)
        {
            ObservableCollection<Ovce> nalezene = new ObservableCollection<Ovce>();
            foreach (Ovce o in SeznamJehnat)
            {
                if (o.Matka.EvidencniCislo == evidencniCislo)
                    nalezene.Add(o);
            }
            return nalezene;
        }

        #endregion

        // **************************************

        #region Prase

        /// <summary>
        /// Metoda pro přidání / úpravu PRASETE uloženého v kolekci SEZNAM SELAT
        /// </summary>
        /// <param name="jmeno">Pojmenování chovatele</param>
        /// <param name="vaha">Váha prasete v KG</param>
        /// <param name="popis">Detailnější popis kusu</param>
        /// <param name="zena">Žena</param>
        /// <param name="muz">Muž</param>
        /// <param name="maruska">První prasnice - matka</param>
        /// <param name="baruska">Druhá prasnice - matka</param>
        /// <param name="datumNarozeni">Datum narození selete</param>
        /// <param name="upravovaneSele">Upravované sele</param>
        public void ZadejSele(string jmeno, string vaha, string popis, bool? zena, bool? muz, bool? maruska, bool? baruska, string datumNarozeni, Prase upravovaneSele)
        {
            // Pomocné proměnné
            DateTime datumNarozeni_pomocna;
            int vaha_pomocna = 0;
            string pohlavi = "Nezadané";
            Svine matka_pomocna = null;
            // Ošetření vstupu
            if (maruska == null && baruska == null)
                throw new ArgumentException("Nezadal jsi žádnou matku!");
            if (datumNarozeni == "")
                throw new ArgumentException("Nezadal jsi žádné datum narození!");
            else
                datumNarozeni_pomocna = DateTime.Parse(datumNarozeni);
            if (datumNarozeni_pomocna > DateTime.Now)
                throw new ArgumentException("Zadal jsi datum narození z budoucnosti");
            if (maruska == true)
                matka_pomocna = Maruska;
            else if (baruska == true)
                matka_pomocna = Baruska;
            if (muz == true)
                pohlavi = "Muž";
            else if (zena == true)
                pohlavi = "Žena";
            if (vaha != "")
                vaha_pomocna = int.Parse(vaha);
            if (popis == "")
                popis = "Žádný";
            // Přídání nového prasete do kolekce
            if (upravovaneSele == null)
            {
                SeznamSelat.Add(new Prase(ID_Sele, datumNarozeni_pomocna, matka_pomocna));
                Trid();
                ID_Sele++;
            }
            // Úprava stávajícího prasete    
            else
            {
                int pomocna = SeznamSelat.IndexOf(upravovaneSele);
                SeznamSelat[pomocna].DatumNarozeni = datumNarozeni_pomocna;
                SeznamSelat[pomocna].Pohlavi = pohlavi;
                SeznamSelat[pomocna].Matka = matka_pomocna;
                SeznamSelat[pomocna].Popis = popis;
                SeznamSelat[pomocna].Jmeno = jmeno;
                SeznamSelat[pomocna].Vaha = vaha_pomocna;
            }
            UlozPrase();
        }

        /// <summary>
        /// Selata se rozdělí do dvou kolekcí - SeleMaruska a SeleBaruska - tedy podle své matky 
        /// </summary>
        public void Trid()
        {
            // Volání pomocné metody, která provede samotné třídění
            Roztrid(1);
            Roztrid(2);
            VyvolejZmenu("SeleMaruska");
            VyvolejZmenu("SeleBaruska");
        }

        /// <summary>
        /// Roztřídění selátek k matkám, podle zadaného čísla matky
        /// </summary>
        /// <param name="matka">1 - Maruška, 2 - Baruška</param>
        /// <returns></returns>
        private void Roztrid(int matka)
        {
            ObservableCollection<Prase> zalozniList = new ObservableCollection<Prase>();
            // První prasnice - pojmenovaná MARUSKA
            if (matka == 1)
            {
                foreach (Prase p in SeznamSelat)
                {
                    if (p.Matka.Jmeno == "Maruska")
                        zalozniList.Add(p);
                }
                SeleMaruska = zalozniList;

            }
            // Druhá prasnice - pojmenovaná BARUSKA
            else if (matka == 2)
            {
                foreach (Prase p in SeznamSelat)
                {
                    if (p.Matka.Jmeno == "Baruska")
                        zalozniList.Add(p);
                }
                SeleBaruska = zalozniList;

            }
        }

        /// <summary>
        /// Metoda pro odebrání prase z kolekce SEZNAM SELAT
        /// </summary>
        /// <param name="prase">Prase, které bude odebráno</param>
        public void OdeberPrase(Prase prase)
        {
            int id = prase.EvidencniCislo;
            SeznamSelat.Remove(prase);
            foreach (Prase p in SeznamSelat)
            {
                if (p.EvidencniCislo > id)
                    p.EvidencniCislo--;
            }
            ID_Sele--;
            Trid();
            UlozPrase();
        }

        /// <summary>
        /// Vytvoří více prasat současně, například při porodu - ukládáno do kolekce SEZNAM SELAT
        /// </summary>
        /// <param name="pocet">Počet prasat</param>
        /// <param name="datumNarozeni">Datum narození prasat</param>
        /// <param name="matka">Prasnice, která je matkou</param>
        public void GenerujSelata(int pocet, DateTime datumNarozeni, Svine matka)
        {
            // + 2 protoze Svine maji cislo 1 a 2
            int pomocna = (SeznamSelat.Count) + 2;
            // Ošetření vstupů
            if (matka == null)
                throw new ArgumentException("Nezadal jsi matku!!");
            if (datumNarozeni > DateTime.Now)
                throw new ArgumentException("Zadal jsi datum z budoucnosti");
            if (pocet <= 0)
                throw new ArgumentException("Nezadal jsi žádná selátka");
            for (int i = 0; i < pocet; i++)
            {
                // Přidání selátek do kolekce
                SeznamSelat.Add(new Prase(i + pomocna, datumNarozeni, matka));
                ID_Sele++;
            }
            Trid();
            UlozPrase();
        }

        /// <summary>
        /// Výpočet celkových hodnot narozených, mrtvých a odchovaných selat u PRASNICE
        /// </summary>
        /// <param name="matka">Jedna je dvou PRASNIC</param>
        public void SpoctiPotomkyPrase(Svine matka)
        {
            // pomocné proměnné
            int[] suma_puvodni = new int[3];
            suma_puvodni[0] = matka.PocetSelat_zivych;
            suma_puvodni[1] = matka.PocetSelat_mrtvych;
            suma_puvodni[2] = matka.PocerSelat_odchovanych;
            int[] suma = new int[3];
            // Projdou se porody dané PRASNICE a u každého se zaznamená počet živých, mrtvých a odchovaných selat pro výpočet CELKEM
            foreach (Porod p in PorodyPrase)
            {
                // Jméno prasnice se shoduje s hledanou
                if (p.Matka.Jmeno == matka.Jmeno)
                {
                    if (p.Ziva > 0)
                        suma[0] += p.Ziva;

                    if (p.Mrtva > 0)
                        suma[1] += p.Mrtva;

                    if (p.Odchovana > 0)
                        suma[2] += p.Odchovana;
                }
            }
            // Data se změnila, tedy je nutné je upravit na aktuální hodnotu
            if (suma_puvodni[0] != suma[0])
                matka.PocetSelat_zivych = suma[0];
            if (suma_puvodni[1] != suma[1])
                matka.PocetSelat_mrtvych = suma[1];
            if (suma_puvodni[2] != suma[2])
                matka.PocerSelat_odchovanych = suma[2];
            // vyvolání změny pro aktualizaci dat na formuláři
            matka.VyvolejZmenu("PocetSelat_zivych");
            matka.VyvolejZmenu("PocetSelat_mrtvych");
            matka.VyvolejZmenu("PocerSelat_odchovanych");
        }

        #endregion

        // **************************************

        #region Porod

        /// <summary>
        /// Zadání záznamu o porodu zvířete (Ovce nebo Prase)
        /// </summary>
        /// <param name="druhZvire">0 - prase, 1 - ovce</param>
        /// <param name="konkZvire">0 - maruska, 1 - baruska</param>
        /// <param name="operace">0 - nový, 1 - úprava, 2 - odebrání</param>
        /// <param name="ziva">Počet živých selat</param>
        /// <param name="mrtva">Mrtvá selata při porodu</param>
        /// <param name="odchov">Odchovaných selat</param>
        /// <param name="prubPorod">Popis průběhu porodu</param>
        /// <param name="kontrolBrezost">Kontrola zda je březí</param>
        /// <param name="datumTestuBrezost">Datum testu březosti</param>
        /// <param name="datZapus">Datum zapuštění</param>
        /// <param name="datNaroz">Datum skutečného narození selat</param>
        /// <param name="evidencniCisloOvce">Evidencni cislo ovce</param>
        public bool ZadejPorod(byte druhZvire, byte konkZvire, byte operace, string evidencniCisloOvce, Porod zaznam, string ziva, string mrtva, string odchov, string prubPorod, bool kontrolBrezost, string datumTestuBrezost,
                               string datZapus, string mesicNarozeni, string datNaroz)
        {
            bool odpoved = false;
            ObservableCollection<Porod> seznam = new ObservableCollection<Porod>();
            Svine matka = null;
            // Pomocné proměnné
            int zivaPomocna = 0;
            int mrtvaPomocna = 0;
            int odchovPomocna = 0;
            DateTime datumzapusteni_pomocna;
            DateTime datumNarozeni_pomocna;
            // Nastavení požadované kolekce a u prasete konkrétní PRASNICE
            // PRASE
            if (druhZvire == 0)
            {
                seznam = PorodyPrase;
                if (konkZvire == 0)
                    matka = Maruska;
                else
                    matka = Baruska;
            }
            // OVCE
            else if (druhZvire == 1)
            {
                seznam = PorodyOvce;
            }

            // Parsovani počtu selat
            if (ziva != "")
                zivaPomocna = int.Parse(ziva);
            if (mrtva != "")
                mrtvaPomocna = int.Parse(mrtva);
            if (odchov != "")
                odchovPomocna = int.Parse(odchov);
            // Kontrola hodnot v selatkach
            if (zivaPomocna < 0 || mrtvaPomocna < 0 || odchovPomocna < 0)
                throw new ArgumentException("Zadal jsi zápornou hodnotu do selátek");

            // OVCE
            if (druhZvire == 1)
            {
                // Přidání nového nebo úprava stávajícího záznamu
                if (operace == 0 || operace == 1)
                {
                    // Pomocné proměnné
                    int mesicNarozeni_pomocna;
                    int mesicZapusteni_pomocna;
                    DateTime datumNarozeni_pomocna_ovce;
                    // Ošetření vyjímek
                    if (mesicNarozeni == "")
                        throw new ArgumentException("Nezadal jsi žádný předpokládáný měsíc narozeni jehňat.");
                    else
                        mesicNarozeni_pomocna = int.Parse(mesicNarozeni);

                    if (datZapus == "")
                        throw new ArgumentException("Nezadal jsi žádný měsíc vpuštění beranu do ohrady");
                    else
                        mesicZapusteni_pomocna = int.Parse(datZapus);
                    // Nový záznam
                    if (operace == 0)
                    {
                        seznam.Add(new Porod(ID_Porod_Ovce, evidencniCisloOvce, mesicZapusteni_pomocna, mesicNarozeni_pomocna));
                        VyvolejZmenu("PorodyOvce");
                        ID_Porod_Ovce++;
                        UlozPorody();
                        UlozID();
                        odpoved = true;
                    }
                    // Úprava stávajícího
                    else if (operace == 1)
                    {
                        int a = seznam.IndexOf(zaznam);
                        if (datNaroz != "")
                        {
                            datumNarozeni_pomocna_ovce = DateTime.Parse(datNaroz);
                            seznam[a].DatumNarozeni = datumNarozeni_pomocna_ovce;
                        }

                        seznam[a].MesicPorodu = mesicNarozeni_pomocna;
                        seznam[a].MesicVpusteniBerana = mesicZapusteni_pomocna;
                        seznam[a].Ziva = zivaPomocna;
                        seznam[a].Mrtva = mrtvaPomocna;
                        seznam[a].Odchovana = odchovPomocna;
                        seznam[a].PrubehPorodu = prubPorod;

                        VyvolejZmenu("PorodyOvce");
                        UlozPorody();
                        odpoved = true;
                    }
                }
                // Odebrání záznamu
                else if (operace == 2)
                {
                    int id = zaznam.ID;
                    if (zaznam != null)
                        seznam.Remove(zaznam);

                    foreach (Porod p in seznam)
                    {
                        if (p.ID > id)
                            p.ID--;
                    }
                    // Snizeni budouciho ID, protoze jedno ID v seznamu ubylo
                    ID_Porod_Ovce--;
                    VyvolejZmenu("PorodyOvce");
                    UlozPorody();
                    UlozID();
                    odpoved = true;
                }
            }

            // PRASE
            if (operace == 0 && druhZvire == 0)
            {
                // Ošetření vstupu
                if (datZapus == "")
                    throw new ArgumentException("Nezadal jsi žádné datum zapuštění");
                else
                    datumzapusteni_pomocna = DateTime.Parse(datZapus);

                seznam.Add(new Porod(ID_Porod_Prase, matka, datumzapusteni_pomocna));
                VyvolejZmenu("PorodyPrase");
                ID_Porod_Prase++;
                UlozPorody();
                UlozID();
                odpoved = true;
            }
            // Uprava stavajiciho zaznamu PRASE
            else if (operace == 1 && druhZvire == 0)
            {
                int a = PorodyPrase.IndexOf(zaznam);
                // Zapusteni
                if (datZapus == "")
                    throw new ArgumentException("Nezadal jsi žádné datum zapuštění");
                else
                    datumzapusteni_pomocna = DateTime.Parse(datZapus);
                // Datum narozeni
                if (datNaroz != "")
                {
                    DateTime.TryParse(datNaroz, out datumNarozeni_pomocna);
                    seznam[a].DatumNarozeni = datumNarozeni_pomocna;
                }

                seznam[a].DatumZapusteni = datumzapusteni_pomocna;
                seznam[a].KontrolaBrezosti = kontrolBrezost;
                seznam[a].Mrtva = mrtvaPomocna;
                seznam[a].Ziva = zivaPomocna;
                seznam[a].Odchovana = odchovPomocna;
                seznam[a].PrubehPorodu = prubPorod;
                VyvolejZmenu("PorodyPrase");
                UlozPorody();
                odpoved = true;
            }
            // Odebrani zaznamu PRASE
            else if (operace == 2)
            {
                int id = zaznam.ID;
                seznam.Remove(zaznam);
                // Snizeni vsech ID nad smazanym
                foreach (Porod p in seznam)
                {
                    if (p.ID > id)
                        p.ID--;
                }
                // Snizeni budouciho ID, protoze jedno ID v seznamu ubylo
                ID_Porod_Prase--;
                VyvolejZmenu("PorodyPrase");
                UlozPorody();
                UlozID();
                odpoved = true;
            }
            return odpoved;
        }

        /// <summary>
        /// Seřadí kolekci porodů PRASE sestupně, tedy poslední nahoře
        /// </summary>
        public void SeradPorodyPrase()
        {
            IEnumerable<Porod> serazeny = PorodyPrase.OrderByDescending(a => a.ID);
            PorodyPrase = new ObservableCollection<Porod>();
            foreach (Porod f in serazeny)
            {
                PorodyPrase.Add(f);
            }
            VyvolejZmenu("PorodyPrase");
        }

        /// <summary>
        /// Vybere pro zadanou PRASNICI porody, aby bylo možné je zobrazit v daném ListBoxu
        /// </summary>
        /// <param name="sv">Prasnice pro kterou budou porody vybírány</param>
        /// <returns></returns>
        public ObservableCollection<Porod> VyberPorodyPrase(Svine sv)
        {
            string jmeno = "";
            ObservableCollection<Porod> seznamVybranych = new ObservableCollection<Porod>();
            if (sv.Jmeno == "Maruska")
                jmeno = "Maruska";
            else if (sv.Jmeno == "Baruska")
                jmeno = "Baruska";

            if (PorodyPrase.Count > 0)
            {
                foreach (Porod p in PorodyPrase)
                {
                    if (p.Matka.Jmeno == jmeno)
                        seznamVybranych.Add(p);
                }
            }
            return seznamVybranych;
        }

        /// <summary>
        /// Vyhledani konkretnich porodu pro jednu z ovci
        /// </summary>
        /// <param name="evidencniCislo">Evidencni cislo ovce</param>
        /// <returns></returns>
        public ObservableCollection<Porod> VyberPorodyOvce(string evidencniCislo)
        {
            ObservableCollection<Porod> nalezene = new ObservableCollection<Porod>();
            foreach (Porod p in PorodyOvce)
            {
                if (p.MatkaEvidencniCislo == evidencniCislo)
                    nalezene.Add(p);
            }

            return nalezene;
        }

        #endregion

        // **************************************

        #region Ockovani a odcerveni

        /// <summary>
        /// Slouzi pro pridani, upravu a odebrani zaznamu z evidence o ockovani nebo odcerveni
        /// </summary>
        /// <param name="volba">0 - Ockovani, 1 - Odcerveni</param>
        /// <param name="operace">0 - Novy zaznam, 1 - Uprava, 2 - Odebrani</param>
        /// <param name="evidencniCislo">Evidencni cislo vztazne ovce</param>
        /// <param name="datum">Datum ukonu</param>
        /// <param name="pripravek">Podany pripravek</param>
        /// <param name="ucel">Ucel ockovani / odcerveni </param>
        /// <param name="popis">Doplnujici popis</param>
        /// <param name="cena">Castka v CZK</param>
        /// <returns>Odpověď, zda se proces podařil</returns>
        public bool ZadejOckovaniOdcerveni(byte volba, byte operace, string evidencniCislo, string datum, string pripravek, string ucel, string popis, string cena, Ockovani zaznamOckovani, Odcerveni zaznamOdcerveni)
        {
            bool odpoved = false;
            int cena_pomocna = 0;
            DateTime datum_pomocna = DateTime.Now;
            // Nový záznam nebo úprava stávajícího
            if (operace != 2)
            {
                // --------------------------- Ošetření vstupů ---------------------------
                // Popis
                if (popis == "")
                    popis = "Žádný";
                // Ucel
                if (ucel == "")
                    ucel = "Nezadáno";
                // Pripravek
                if (pripravek == "")
                    throw new ArgumentException("Nezadal jsi podaný přípravek");
                // Cena
                if (cena == "")
                    throw new ArgumentException("Nezadal jsi cenu.");
                else
                    cena_pomocna = int.Parse(cena);

                if (cena_pomocna < 0)
                    throw new ArgumentException("Zadaná cena je menší než 0. Uprav ji minimálně na 0");
                // Datum
                if (datum == "")
                    throw new ArgumentException("Nezadal jsi datum provedeného očkování / odčervení");
                else
                    datum_pomocna = DateTime.Parse(datum);

                if (datum_pomocna > DateTime.Now)
                    throw new ArgumentException("Zadané datum je z budoucnosti. Zkontroluj zadání");
            }

            // --------------------------- Očkování ---------------------------
            if (volba == 0)
            {
                // Nový záznam
                if (operace == 0)
                {
                    SeznamOckovani.Add(new Ockovani(ID_Ockovani, datum_pomocna, evidencniCislo, pripravek, ucel, popis, cena_pomocna));
                    ID_Ockovani++;
                    VyvolejZmenu("SeznamOckovani");
                    odpoved = true;
                    UlozOckovani();
                    UlozID();
                }
                // Úprava stávajícího
                else if (operace == 1 && zaznamOckovani != null)
                {
                    int a = SeznamOckovani.IndexOf(zaznamOckovani);
                    SeznamOckovani[a].Datum = datum_pomocna;
                    SeznamOckovani[a].Cena = cena_pomocna;
                    SeznamOckovani[a].Popis = popis;
                    SeznamOckovani[a].Pripravek = pripravek;
                    SeznamOckovani[a].Ucel = ucel;

                    VyvolejZmenu("SeznamOckovani");
                    odpoved = true;
                    UlozOckovani();
                }
                // Odebrání záznamu
                else if (operace == 2 && zaznamOckovani != null)
                {
                    int zalohaID = zaznamOckovani.ID;

                    if (zaznamOckovani != null)
                        SeznamOckovani.Remove(zaznamOckovani);

                    foreach (Ockovani o in SeznamOckovani)
                    {
                        if (o.ID > zalohaID)
                            o.ID--;
                    }


                    ID_Ockovani--;
                    VyvolejZmenu("SeznamOckovani");
                    odpoved = true;
                    UlozOckovani();
                    UlozID();
                }
            }
            // --------------------------- Odčervení ---------------------------
            else if (volba == 1)
            {
                // Nový záznam
                if (operace == 0)
                {
                    SeznamOdcerveni.Add(new Odcerveni(ID_Odcerveni, datum_pomocna, evidencniCislo, pripravek, ucel, popis, cena_pomocna));
                    ID_Odcerveni++;
                    VyvolejZmenu("SeznamOdcerveni");
                    odpoved = true;
                    UlozOdcerveni();
                    UlozID();
                }
                // Úprava stávajícího
                else if (operace == 1 && zaznamOdcerveni != null)
                {
                    int a = SeznamOdcerveni.IndexOf(zaznamOdcerveni);
                    SeznamOdcerveni[a].Datum = datum_pomocna;
                    SeznamOdcerveni[a].Cena = cena_pomocna;
                    SeznamOdcerveni[a].Popis = popis;
                    SeznamOdcerveni[a].Pripravek = pripravek;
                    SeznamOdcerveni[a].Ucel = ucel;

                    VyvolejZmenu("SeznamOdcerveni");
                    odpoved = true;
                    UlozOdcerveni();
                }
                // Odebrání záznamu
                else if (operace == 2 && zaznamOdcerveni != null)
                {
                    int zalohaID = zaznamOdcerveni.ID;

                    if (SeznamOdcerveni != null)
                        SeznamOdcerveni.Remove(zaznamOdcerveni);

                    foreach (Odcerveni o in SeznamOdcerveni)
                    {
                        if (o.ID > zalohaID)
                            o.ID--;
                    }

                    ID_Odcerveni--;
                    VyvolejZmenu("SeznamOdcerveni");
                    odpoved = true;
                    UlozOdcerveni();
                    UlozID();
                }
            }
            return odpoved;
        }

        /// <summary>
        /// Vybere očkování pro konkrétní OVCI
        /// </summary>
        /// <param name="evCislo">Evidenční číslo dané ovce</param>
        /// <returns></returns>
        public ObservableCollection<Ockovani> VyberOckovani(string evCislo)
        {
            ObservableCollection<Ockovani> nalezene = new ObservableCollection<Ockovani>();
            if (SeznamOckovani.Count > 0)
            {
                foreach (Ockovani o in SeznamOckovani)
                {
                    if (o.EvidencniCisloOvce == evCislo)
                        nalezene.Add(o);
                }
            }
            return nalezene;
        }

        /// <summary>
        /// Vybere odčervení pro konkrétní OVCI
        /// </summary>
        /// <param name="evCislo">Evidenční číslo dané ovce</param>
        /// <returns></returns>
        public ObservableCollection<Odcerveni> VyberOdcerveni(string evCislo)
        {
            ObservableCollection<Odcerveni> nalezene = new ObservableCollection<Odcerveni>();
            if (SeznamOdcerveni.Count > 0)
            {
                foreach (Odcerveni o in SeznamOdcerveni)
                {
                    if (o.EvidencniCisloOvce == evCislo)
                        nalezene.Add(o);
                }
            }
            return nalezene;
        }

        #endregion

        // **************************************

        #region Veterina

        /// <summary>
        /// Přídání nebo úprava veterinárních záznamů - kolekce SeznamVeterinarnichZaznamu_Prase * Ovce
        /// </summary>
        /// <param name="sele">Vztažné sele</param>
        /// <param name="vet">Při úpravě stávajícího záznamu</param>
        /// <param name="ucel">Účel veterinární návštevy</param>
        /// <param name="ukony">Veterinární úkony</param>
        /// <param name="datum">Datum návštevy veterináře</param>
        /// <param name="leky">Podané léčivo</param>
        /// <param name="cislo">Číslo veterinární návštevy</param>
        /// <param name="cena">Cena zákroku</param>
        /// <param name="ukon">0 - nový záznam, 1 - úprava záznamu</param>
        public void ZadejVeterinarniZaznam(Prase sele, Ovce ovce, Veterina vet, string ucel, string ukony, string datum, string leky, string cena, byte ukon)
        {

            ObservableCollection<Veterina> seznam;
            // Výběr kolekce, do které bude uloženo - Ovce nebo Prase
            if (sele != null)
                seznam = SeznamVeterinarnichZaznamu_Prase;
            else
                seznam = SeznamVeterinarnichZaznamu_Ovce;
            // Pomocné proměnné
            DateTime datum_pomocna;
            int cena_pomocna;
            // Ošetření vstupu, pomocí vyvolání vyjímek se zprávou pro uživatele
            if (ukony == "")
                ukony = "Žádné";
            if (leky == "")
                leky = "Žádné";
            if (ucel == "")
                throw new ArgumentException("Nezadal jsi žádný účel návštavy veterináře");
            if (datum == "")
                throw new ArgumentException("Nezadal jsi žádné datum");
            else
                datum_pomocna = DateTime.Parse(datum);

            if (cena == "")
                throw new ArgumentException("Nezadal jsi žádnou cenu zákroku");
            else
                cena_pomocna = int.Parse(cena);
            if (cena_pomocna < 0)
                throw new ArgumentException("Zadal jsi zápornou cenu");
            // Nový veterinární záznam
            if (ukon == 0)
            {
                // Zadava se veterinarni zaznam pro PRASE
                if (ovce == null)
                {
                    seznam.Add(new Veterina(ucel, ukony, datum_pomocna, leky, sele, null, ID_Veterina_Prase, cena_pomocna));
                    ID_Veterina_Prase++;
                }

                // Zadava se veterinarni zaznam pro OVCI
                else
                {
                    seznam.Add(new Veterina(ucel, ukony, datum_pomocna, leky, null, ovce, ID_Veterina_Ovce, cena_pomocna));
                    ID_Veterina_Ovce++;
                }

            }
            // Uprava stavajiciho veterinarniho zaznamu    
            else if (ukon == 1)
            {
                int a = seznam.IndexOf(vet);
                seznam[a].PodaneLecivo = vet.PodaneLecivo;
                seznam[a].Ucel = vet.Ucel;
                seznam[a].DatumNavstevy = vet.DatumNavstevy;
                seznam[a].ProvedeneUkony = vet.ProvedeneUkony;
                seznam[a].Cena = vet.Cena;
                seznam[a].CisloNavstevy = vet.CisloNavstevy;
                VyvolejZmenu("SeznamVeterinarnichZaznamu_Prase");
                VyvolejZmenu("SeznamVeterinarnichZaznamu_Ovce");
            }
            UlozVeterina();
            VyvolejZmenu("SeznamVeterinarnichZaznamu_Prase");
            VyvolejZmenu("SeznamVeterinarnichZaznamu_Ovce");
        }

        /// <summary>
        /// Pro zadané zvíře (Ovce nebo Prase) vybere VETERINÁRNÍ ZÁZNAMY
        /// </summary>
        /// <param name="sele">Prase pro které se vyhledají veterinární záznamy</param>
        /// <param name="ovce">Ovce pro kterou se vyhledají veterinární záznamy</param>
        /// <returns></returns>
        public ObservableCollection<Veterina> VyberZaznamy(Prase sele, Ovce ovce)
        {
            // Kolekce pro nalezené záznamy
            ObservableCollection<Veterina> nalezene = new ObservableCollection<Veterina>();
            // Záznamy prase
            if (sele != null)
            {
                foreach (Veterina v in SeznamVeterinarnichZaznamu_Prase)
                {
                    if (v.VztaznePrase.EvidencniCislo == sele.EvidencniCislo)
                    {
                        nalezene.Add(v);
                    }
                }
                if (nalezene != null)
                {
                    ZaznamySelete = nalezene;
                    VyvolejZmenu("ZaznamySelete");
                }
            }
            // Záznamy ovce
            else if (ovce != null)
            {
                foreach (Veterina v in SeznamVeterinarnichZaznamu_Ovce)
                {
                    if (v.VztaznaOvce.EvidencniCislo == ovce.EvidencniCislo)
                    {
                        nalezene.Add(v);
                    }
                }

            }
            return nalezene;
        }

        #endregion

        // **************************************

        #region Save / Load

        /// <summary>
        /// Ulozeni vydaju pro dane zvire
        /// </summary>
        /// <param name="a">0 - Prase, 1 - Ovce</param>
        public void UlozVydaje(byte a)
        {
            // Pomocná kolekce
            ObservableCollection<Finance> Vydaje = new ObservableCollection<Finance>();
            string cestaVydaje = "";
            // OVCE
            if (a == 1)
            {
                Vydaje = VydajeOvce;
                cestaVydaje = cestaVydajeOvce;
            }
            // PRASE
            else if (a == 0)
            {
                Vydaje = VydajePrase;
                cestaVydaje = cestaVydajePrase;
            }
            // Instance serializéru
            XmlSerializer serializer = new XmlSerializer(Vydaje.GetType());
            // Projde se kolekce a uloží se v XML do souboru
            using (StreamWriter sr = new StreamWriter(cestaVydaje))
            {
                serializer.Serialize(sr, Vydaje);
            }
        }

        /// <summary>
        /// Nacteni prijmu
        /// </summary>
        /// <param name="a">0 - Prase, 1 - Ovce</param>
        public void NactiPrijmy(byte a)
        {
            // Pomocná kolekce
            ObservableCollection<Finance> Prijem = new ObservableCollection<Finance>();
            string cestaPrijmy = "";
            // OVCE
            if (a == 1)
            {
                cestaPrijmy = cestaPrijmyOvce;
            }
            // PRASE
            else if (a == 0)
            {
                cestaPrijmy = cestaPrijmyPrase;
            }
            // Instance serializéru
            XmlSerializer serializer = new XmlSerializer(Prijem.GetType());
            if (File.Exists(cestaPrijmy))
            {
                // Projde se XML a rozdělí se na jednotlivé objekty a ty se uloží do kolekce
                using (StreamReader sr = new StreamReader(cestaPrijmy))
                {
                    Prijem = (ObservableCollection<Finance>)serializer.Deserialize(sr);
                }
            }
            else
                Prijem = new ObservableCollection<Finance>();

            if (a == 0)
                PrijmyPrase = Prijem;
            else if (a == 1)
                PrijmyOvce = Prijem;
        }

        /// <summary>
        /// Nacteni vydaju
        /// </summary>
        /// <param name="a">0 - Prase, 1 - Ovce</param>
        public void NactiVydaje(byte a)
        {
            ObservableCollection<Finance> Vydaje = new ObservableCollection<Finance>();
            string cestaVydaje = "";
            if (a == 1)
            {
                cestaVydaje = cestaVydajeOvce;
            }
            else if (a == 0)
            {
                cestaVydaje = cestaVydajePrase;
            }

            XmlSerializer serializer = new XmlSerializer(Vydaje.GetType());
            if (File.Exists(cestaVydaje))
            {
                using (StreamReader sr = new StreamReader(cestaVydaje))
                {
                    Vydaje = (ObservableCollection<Finance>)serializer.Deserialize(sr);
                }
            }
            else
                Vydaje = new ObservableCollection<Finance>();

            if (a == 0)
                VydajePrase = Vydaje;
            else if (a == 1)
                VydajeOvce = Vydaje;
        }

        /// <summary>
        /// Uložení prijmy pro dane zvire
        /// </summary>
        /// <param name="a">0 - Prase, 1 - Ovce</param>
        public void UlozPrijmy(byte a)
        {
            // Pomocná kolekce
            ObservableCollection<Finance> Prijem = new ObservableCollection<Finance>();
            string cestaPrijmy = "";
            // OVCE
            if (a == 1)
            {
                Prijem = PrijmyOvce;
                cestaPrijmy = cestaPrijmyOvce;
            }
            // PRASE
            else if (a == 0)
            {
                Prijem = PrijmyPrase;
                cestaPrijmy = cestaPrijmyPrase;
            }
            // instance serializéru
            XmlSerializer serializer = new XmlSerializer(Prijem.GetType());
            // Projde se kolekce a uloží se v XML do souboru
            using (StreamWriter sr = new StreamWriter(cestaPrijmy))
            {
                serializer.Serialize(sr, Prijem);
            }
        }

        /// <summary>
        /// Uložení všech ovcí - kolekce jehňat, ovcí a beranů
        /// </summary>
        public void UlozOvce()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamOvci.GetType());
            using (StreamWriter sw = new StreamWriter(cestaOvce))
            {
                serializer.Serialize(sw, SeznamOvci);
            }

            using(StreamWriter sw = new StreamWriter(cestaJehnata))
            {
                serializer.Serialize(sw, SeznamJehnat);
            }

            using (StreamWriter sw = new StreamWriter(cestaBerani))
            {
                serializer.Serialize(sw, SeznamBeranu);
            }

        }

        /// <summary>
        /// Načtení dat pro kolekci jehňat, beranů a ovcí
        /// </summary>
        public void NactiOvce()
        {

            XmlSerializer serializer = new XmlSerializer(SeznamOvci.GetType());
            if (File.Exists(cestaOvce))
            {
                using (StreamReader sr = new StreamReader(cestaOvce))
                {
                    SeznamOvci = (ObservableCollection<Ovce>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamOvci = new ObservableCollection<Ovce>();

            if (File.Exists(cestaBerani))
            {
                using (StreamReader sr = new StreamReader(cestaBerani))
                {
                    SeznamBeranu = (ObservableCollection<Ovce>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamBeranu = new ObservableCollection<Ovce>();

            if (File.Exists(cestaJehnata))
            {
                using (StreamReader sr = new StreamReader(cestaJehnata))
                {
                    SeznamJehnat = (ObservableCollection<Ovce>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamJehnat = new ObservableCollection<Ovce>();
        }

        /// <summary>
        /// Uložení všech záznamů očkování - kolekce SeznamOckovani
        /// </summary>
        public void UlozOckovani()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamOckovani.GetType());
            using (StreamWriter sw = new StreamWriter(cestaOckovani))
            {
                serializer.Serialize(sw, SeznamOckovani);
            }
        }

        /// <summary>
        /// Načtení všech záznamů o očkování - kolekce SeznamOckovani
        /// </summary>
        public void NactiOckovani()
        {

            XmlSerializer serializer = new XmlSerializer(SeznamOckovani.GetType());
            if (File.Exists(cestaOckovani))
            {
                using (StreamReader sr = new StreamReader(cestaOckovani))
                {
                    SeznamOckovani = (ObservableCollection<Ockovani>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamOckovani = new ObservableCollection<Ockovani>();
        }

        /// <summary>
        /// Uložení všech záznamů o odčervení - kolekce SeznamOdcerveni
        /// </summary>
        public void UlozOdcerveni()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamOdcerveni.GetType());
            using (StreamWriter sw = new StreamWriter(cestaOdcerveni))
            {
                serializer.Serialize(sw, SeznamOdcerveni);
            }
        }

        /// <summary>
        /// Načtení všech záznamů o odčervení - kolekce SeznamOdcerveni
        /// </summary>
        public void NactiOdcerveni()
        {

            XmlSerializer serializer = new XmlSerializer(SeznamOdcerveni.GetType());
            if (File.Exists(cestaOdcerveni))
            {
                using (StreamReader sr = new StreamReader(cestaOdcerveni))
                {
                    SeznamOdcerveni = (ObservableCollection<Odcerveni>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamOdcerveni = new ObservableCollection<Odcerveni>();
        }

        /// <summary>
        /// Uložení všech porodů Ovce i Prase - kolekce PorodyPrase, PorodyOVce
        /// </summary>
        public void UlozPorody()
        {
            XmlSerializer serializer = new XmlSerializer(PorodyPrase.GetType());
            using (StreamWriter sw = new StreamWriter(cestaPorodyPrase))
            {
                serializer.Serialize(sw, PorodyPrase);
            }

            using (StreamWriter sw = new StreamWriter(cestaPorodyOvce))
            {
                serializer.Serialize(sw, PorodyOvce);
            }
        }

        /// <summary>
        /// Načtení všech porodů Ovce i Prase - kolekce PorodyPrase, PorodyOvce
        /// </summary>
        public void NactiPorody()
        {

            XmlSerializer serializer = new XmlSerializer(PorodyPrase.GetType());
            if (File.Exists(cestaPorodyPrase))
            {
                using (StreamReader sr = new StreamReader(cestaPorodyPrase))
                {
                    PorodyPrase = (ObservableCollection<Porod>)serializer.Deserialize(sr);
                }
            }
            else
                PorodyPrase = new ObservableCollection<Porod>();

            if (File.Exists(cestaPorodyOvce))
            {
                using (StreamReader sr = new StreamReader(cestaPorodyOvce))
                {
                    PorodyOvce = (ObservableCollection<Porod>)serializer.Deserialize(sr);
                }
            }
            else
                PorodyOvce = new ObservableCollection<Porod>();
        }

        /// <summary>
        /// Uložení všech ID, které identifikují všechny prvky v kolekcích
        /// </summary>
        public void UlozID()
        {
            List<int> ids = new List<int>();
            ids.Add(ID_Finance_Prase);
            ids.Add(ID_Ovce);
            ids.Add(ID_Porod_Prase);
            ids.Add(ID_Sele);
            ids.Add(ID_Veterina_Prase);
            ids.Add(ID_Beran);
            ids.Add(ID_Jehne);
            ids.Add(ID_Finance_Ovce);
            ids.Add(ID_Veterina_Ovce);
            ids.Add(ID_Odcerveni);
            ids.Add(ID_Ockovani);
            ids.Add(ID_Porod_Ovce);

            XmlSerializer serializer = new XmlSerializer(ids.GetType());

            using (StreamWriter sw = new StreamWriter(cestaID))
            {
                serializer.Serialize(sw, ids);
            }
        }

        /// <summary>
        /// Načtení všech IDs
        /// </summary>
        public void NactiID()
        {
            List<int> ids = new List<int>();

            XmlSerializer serializer = new XmlSerializer(ids.GetType());
            if (File.Exists(cestaID))
            {
                using (StreamReader sr = new StreamReader(cestaID))
                {
                    ids = (List<int>)serializer.Deserialize(sr);
                }
            }
            else
                ids = new List<int>();

            ID_Finance_Prase = ids[0];
            ID_Ovce = ids[1];
            ID_Porod_Prase = ids[2];
            ID_Sele = ids[3];
            ID_Veterina_Prase = ids[4];
            ID_Beran = ids[5];
            ID_Jehne = ids[6];
            ID_Finance_Ovce = ids[7];
            ID_Veterina_Ovce = ids[8];
            ID_Odcerveni = ids[9];
            ID_Ockovani = ids[10];
            ID_Porod_Ovce = ids[11];
        }

        /// <summary>
        /// Uložení všech selat, od obou prasnic - kolekce SeznamSelat
        /// </summary>
        public void UlozPrase()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamSelat.GetType());

            using (StreamWriter sw = new StreamWriter(cestaPrase))
            {
                serializer.Serialize(sw, SeznamSelat);
            }
        }

        /// <summary>
        /// Načtení všech selat, od obou prasnic - kolekce SeznamSelat
        /// </summary>
        public void NactiPrase()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamSelat.GetType());
            if (File.Exists(cestaPrase))
            {
                using (StreamReader sr = new StreamReader(cestaPrase))
                {
                    SeznamSelat = (ObservableCollection<Prase>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamSelat = new ObservableCollection<Prase>();
        }

        /// <summary>
        /// Uložení všech veterinárních záznamů Ovce i Prasete - kolekce SeznamVeterinarnichZaznamu_Ovce * Prase
        /// </summary>
        public void UlozVeterina()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamVeterinarnichZaznamu_Prase.GetType());

            using (StreamWriter sw = new StreamWriter(cestaVeterina))
            {
                serializer.Serialize(sw, SeznamVeterinarnichZaznamu_Prase);
            }

            using (StreamWriter sw = new StreamWriter(cestaVeterina_Ovce))
            {
                serializer.Serialize(sw, SeznamVeterinarnichZaznamu_Ovce);
            }
        }

        /// <summary>
        /// Načtení všech veterinárních záznamů Ovce i Prasete - kolekce SeznamVeterinarnichZaznamu_Ovce * Prase
        /// </summary>
        public void NactiVeterina()
        {
            XmlSerializer serializer = new XmlSerializer(SeznamVeterinarnichZaznamu_Prase.GetType());
            if (File.Exists(cestaVeterina))
            {
                using (StreamReader sr = new StreamReader(cestaVeterina))
                {
                    SeznamVeterinarnichZaznamu_Prase = (ObservableCollection<Veterina>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamVeterinarnichZaznamu_Prase = new ObservableCollection<Veterina>();

            if (File.Exists(cestaVeterina_Ovce))
            {
                using (StreamReader sr = new StreamReader(cestaVeterina_Ovce))
                {
                    SeznamVeterinarnichZaznamu_Ovce = (ObservableCollection<Veterina>)serializer.Deserialize(sr);
                }
            }
            else
                SeznamVeterinarnichZaznamu_Ovce = new ObservableCollection<Veterina>();
        }

        #endregion 
    }
}
