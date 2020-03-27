using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidenceZvirat
{
        /// <summary>
        /// Třída reprezentující jeden veterinářský záznam
        /// </summary>
        public class Veterina
        {
            /// <summary>
            /// Datum návštevy veterináře
            /// </summary>
            public DateTime DatumNavstevy { get; set; }
            /// <summary>
            /// Účel veterinárního úkou - očkování, porod apod.
            /// </summary>
            public string Ucel { get; set; } 
            /// <summary>
            ///  Pořadí záznamu
            /// </summary>
            public int CisloNavstevy { get; set;}
            /// <summary>
            /// Náklady na daný veterinární zákrok
            /// </summary>
            public int Cena { get; set; }
            /// <summary>
            /// Léky podáné zvířeti
            /// </summary>
            public string PodaneLecivo { get; set; }
            /// <summary>
            /// Vyjmenování provedených úkonů
            /// </summary>
            public string ProvedeneUkony { get; set; }
            /// <summary>
            /// Prase pro které byl záznam vypsán
            /// </summary>
            public Prase VztaznePrase { get; set; }
            /// <summary>
            /// Ovce pro kterou byl záznam vypsán
            /// </summary>
            public Ovce VztaznaOvce { get; set; }

            /// <summary>
            /// Základní konstruktor - NeKompletní
            /// </summary>
            /// <param name="ucel">Účel veterinárního úkou - očkování, porod apod.</param>
            /// <param name="provadeneUkony">Vyjmenování provedených úkonů</param>
            /// <param name="datumNavstevy">Datum</param>
            /// <param name="sele">Prase, kterého se záznam týká</param>
            public Veterina(string ucel, string provadeneUkony, DateTime datumNavstevy, Prase sele)
            {
                VztaznePrase = sele;
                Ucel = ucel;
                ProvedeneUkony = provadeneUkony;
                DatumNavstevy = datumNavstevy;
            }

            /// <summary>
            /// Kompletní konstuktor
            /// </summary>
            /// <param name="ucel">Účel veterinárního úkonu - očkování, porod apod.</param>
            /// <param name="provadeneUkony">Úkony prováděné zvířeti</param>
            /// <param name="datumNavstevy">Datum záznamu</param>
            /// <param name="podaneLecivo">Léčivo podané zvířeti</param>
            /// <param name="sele">Prase, kterého se úkon týkal</param>
            /// <param name="ovce">Ovce, které se úkon týkal</param>
            /// <param name="cislo">Pořadové číslo záznamu</param>
            /// <param name="cena">Náklady v CZK</param>
            public Veterina(string ucel, string provadeneUkony, DateTime datumNavstevy, string podaneLecivo, Prase sele, Ovce ovce, int cislo, int cena)
            {
                VztaznePrase = sele;
                Ucel = ucel;
                ProvedeneUkony = provadeneUkony;
                DatumNavstevy = datumNavstevy;
                PodaneLecivo = podaneLecivo;
                CisloNavstevy = cislo;
                Cena = cena;
                VztaznaOvce = ovce;
            }

            /// <summary>
            /// Prázdný konstruktor pro XmlSerialiazer
            /// </summary>
            public Veterina()
            {

            }   
        
            /// <summary>
            /// Přetížená metoda pro výpis při zobrazení v ListBoxu využíváno
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Ucel;
            }
        }
}