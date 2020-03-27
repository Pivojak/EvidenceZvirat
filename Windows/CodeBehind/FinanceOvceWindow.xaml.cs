using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvidenceZvirat
{
    /// <summary>
    /// Interakční logika pro FinanceOvceWindow.xaml
    /// </summary>
    public partial class FinanceOvceWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Upravovaný finanční záznam
        /// </summary>
        private Finance vybranyZaznam;

        private byte zvire = 1;

        /// <summary>
        /// Výpočet parametrů jako výdaje, příjmy dle kokrétní ovce, generace ovcí, roku či že se jedná o vedlejší položky - např. Oplocení
        /// </summary>
        /// <param name="evCislo">Evidenční číslo konkrétní ovce</param>
        /// <param name="rok">Rok, za který budou příjmy a výdaje počítány</param>
        /// <param name="generace">Generace ovcí, pro kterou budou výdaje a příjmy počítány</param>
        private void PrepocitejBilance(string evCislo,int rok, byte generace)
        {
            vydajeOvceTextBlock.DataContext = spravce.SpocitejBilanci(0, 1,4,evCislo,0,0);
            vydajeRokTextBlock.DataContext = spravce.SpocitejBilanci(0, 1,5,"",0,rok);
            vydajeOstatniTextBlock.DataContext = spravce.SpocitejBilanci(0, 1,2,"",0,0);
            vydajeGeneraceTextBlock.DataContext = spravce.SpocitejBilanci(0, 1,3,"",generace,0);

            prijmyOvceTextBlock.DataContext = spravce.SpocitejBilanci(1, 1, 4, evCislo, 0, 0);
            prijmyRokTextBlock.DataContext = spravce.SpocitejBilanci(1, 1, 5, "", 0, rok);
            prijmyOstatniTextBlock.DataContext = spravce.SpocitejBilanci(1, 1, 2, "", 0, 0);
            prijmyGeneraceaTextBlock.DataContext = spravce.SpocitejBilanci(1, 1, 3, "", generace, 0);
        }

        /// <summary>
        /// Základní konstruktor okna 
        /// </summary>
        /// <param name="spravceZv">Správce aplikace</param>
        public FinanceOvceWindow(SpravceZvirat spravceZv)
        {
            InitializeComponent();
            spravce = spravceZv;
            spravce.SpocitejZisk(1);
            DataContext = spravce;
            vydajeListBox.DataContext = spravce.VydajeOvce;
            prijmyListBox.DataContext = spravce.PrijmyOvce;
            rokTextBox.Text = "2019";
            PrepocitejBilance("", DateTime.Now.Year, 0);
            
        }
        
        /// <summary> 
        /// Uložení změn u PŘÍJMŮ - levé tlačítko ULOŽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UlozPrButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            // Není vybraný žádný záznam NEBO je vybrán, avšak se jedná o VÝDAJ
            if (vybranyZaznam == null || vybranyZaznam.TypOperace == 0)
            {
                try
                {
                    odpoved = spravce.ZadejFinanceZaznam(nazevPrTextBox.Text, popisPrTextBox.Text, cenaPrTextBox.Text,
                                               ovcePrTextBox.Text.ToLower().Trim(), generacePrTextBox.Text, datumPrTextBox.Text,1,1,0,null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                

                
            }
            // Upravení stávajícího příjmu
            else
            {
                 odpoved = spravce.ZadejFinanceZaznam(nazevPrTextBox.Text, popisPrTextBox.Text, cenaPrTextBox.Text,
                                               ovcePrTextBox.Text.ToLower().Trim(), generacePrTextBox.Text, datumPrTextBox.Text,1, 1, 1, vybranyZaznam);
            }
            if (odpoved)
            {
                spravce.SpocitejZisk(1);
                PrepocitejBilance("", DateTime.Now.Year, 0);
                datumPrTextBox.Clear();
                generacePrTextBox.Clear();
                ovcePrTextBox.Clear();
                nazevPrTextBox.Clear();
                cenaPrTextBox.Clear();
                popisPrTextBox.Clear();
            }

        }
        
        /// <summary>
        /// Uložení změn VÝDAJŮ - pravé tlačtíko ULOŽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UlozVyButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;

            // Není vybraný žádný záznam NEBO je vybrán, avšak se jedná o PŘÍJEM
            if (vybranyZaznam == null || vybranyZaznam.TypOperace == 1)
            {
                try
                {
                    odpoved = spravce.ZadejFinanceZaznam(nazevVyTextBox.Text, popisVyTextBox.Text, cenaVyTextBox.Text,
                                               ovceVyTextBox.Text.ToLower().Trim(), generaceVyTextBox.Text, datumVyTextBox.Text,0,1,0,null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }



            }
            // Úprava výdaje
            else
            {
                odpoved = spravce.ZadejFinanceZaznam(nazevVyTextBox.Text, popisVyTextBox.Text, cenaVyTextBox.Text,
                                               ovceVyTextBox.Text.ToLower().Trim(), generaceVyTextBox.Text, datumVyTextBox.Text,0,1,1,vybranyZaznam);
            }
            if (odpoved)
            {
                spravce.SpocitejZisk(1);
                PrepocitejBilance("", DateTime.Now.Year, 0);
                datumVyTextBox.Clear();
                generaceVyTextBox.Clear();
                ovceVyTextBox.Clear();
                nazevVyTextBox.Clear();
                cenaVyTextBox.Clear();
                popisVyTextBox.Clear();
            }
        }

        /// <summary>
        /// Odebrání transakce na straně Příjmů - levé tlačítko ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberPrButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            // Ošetření, že uživatel vybral nějakou transakci
            if (vybranyZaznam == null)
                MessageBox.Show("Nic jsi nevybral!", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    odpoved = spravce.OdeberFinancniZaznam(vybranyZaznam, zvire,1);
                }
                catch
                {
                    MessageBox.Show("Chyba pri odebirani zaznamu", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (odpoved)
            {
                spravce.SpocitejZisk(1);
                PrepocitejBilance("", DateTime.Now.Year, 0);
            }


        }

        /// <summary>
        /// Odebrání transakce na straně VÝDAJŮ - pravé tlačítko ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberVyButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            // Ošetření, že uživatel vybral nějakou transakci
            if (vybranyZaznam == null)
                MessageBox.Show("Nic jsi nevybral!", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    odpoved = spravce.OdeberFinancniZaznam(vybranyZaznam, zvire,0);
                }
                catch
                {
                    MessageBox.Show("Chyba pri odebirani zaznamu", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (odpoved)
            {
                spravce.SpocitejZisk(1);
                PrepocitejBilance("", DateTime.Now.Year, 0);
            }
        }

        /// <summary>
        /// Ulozeni celkovych financi pri zavirani okna (PŘÍJMY, VÝDAJE) - tlačítko ULOŽ FINANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UlozFinanceButton_Click(object sender, RoutedEventArgs e)
        {
            spravce.UlozVydaje(1);
            spravce.UlozPrijmy(1);
            spravce.UlozID();
            Close();

        }

        /// <summary>
        /// Uložení vybraného záznamu při změně výběru v ListBoxu pro příjem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrijmyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranyZaznam = (Finance)prijmyListBox.SelectedItem;
        }

        /// <summary>
        /// Uložení vybraného záznamu při změně výběru v ListBoxu pro výdaj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VydajeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranyZaznam = (Finance)vydajeListBox.SelectedItem;
        }

        /// <summary>
        /// Výpočet příjmů a výdajů pro konkrétní ovci - tlačítko SPOČTI na řádku ovce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpoctiKonkretniOvciButton_Click(object sender, RoutedEventArgs e)
        {
            if(ovceTextBox.Text == "")
                    MessageBox.Show("Nezadal jsi žádnou ovci.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                PrepocitejBilance(ovceTextBox.Text, DateTime.Now.Year, 0);
        }

        /// <summary>
        /// Výpočet příjmů a výdajů pro daný rok - tlačítko SPOČTI na řádku rok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpoctiRokButton_Click(object sender, RoutedEventArgs e)
        {
            int rok_pomocna;
            bool odpoved = false;
            odpoved = int.TryParse(rokTextBox.Text, out rok_pomocna);
            if(!odpoved)
                MessageBox.Show("Nezadal jsi žádný rok. ", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            if (odpoved)
                PrepocitejBilance("", rok_pomocna, 0);
        }

        /// <summary>
        /// Výpočet příjmů a výdajů pro konkrétní ovci - tlačítko SPOČTI na řádku generace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpoctiGeneraceButton_Click(object sender, RoutedEventArgs e)
        {
            byte generace_pomocna;
            bool odpoved = false;
            odpoved = byte.TryParse(cisloGeneraceTextBox.Text, out generace_pomocna);
            if (!odpoved)
                MessageBox.Show("Nezadal žádné číslo generace.", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            if (odpoved)
                PrepocitejBilance("", DateTime.Now.Year,generace_pomocna);
        }
    }
}
