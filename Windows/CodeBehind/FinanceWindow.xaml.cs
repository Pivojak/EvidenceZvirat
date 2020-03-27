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
    /// Interakční logika pro FinanceWindow.xaml
    /// </summary>
    public partial class FinanceWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Vybraný finanční záznam - PŘÍJEM nebo VÝDAJ
        /// </summary>
        private Finance vybranyZaznam;

        private byte zvire;

        /// <summary>
        /// Konstruktor okna
        /// </summary>
        /// <param name="spravce">Správce aplikace</param>
        /// <param name="zv"></param>
        public FinanceWindow(SpravceZvirat spravce, byte zv)
        {
            InitializeComponent();
            this.spravce = spravce;
            zvire = zv;
            DataContext = spravce;

            spravce.SeradFinance();

            // Nastavení kontextu a výpočet parametrů uprostřed okna, jako jsou výdaje a příjmy dle Prasnice apod
            vydajeListBox.DataContext = spravce.VydajePrase;
            prijmyListBox.DataContext = spravce.PrijmyPrase;
            spravce.SpocitejZisk(0);
            vydajeMaruskaTextBlock.DataContext = spravce.SpocitejBilanci(0,0, 0,"",0,0);
            vydajeBaruskaTextBlock.DataContext = spravce.SpocitejBilanci(0,0, 1,"",0,0);
            prijmyMaruskaTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 0,"",0,0);
            prijmyBaruskaTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 1,"",0,0);
            vydajeOstatniTextBlock.DataContext = spravce.SpocitejBilanci(0, 0, 2,"",0,0);
            prijmyOstatniTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 2,"",0,0);
            prijmyVrhTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 3,"", 0,0);
            vydajeVrhTextBlock.DataContext = spravce.SpocitejBilanci(0, 0, 3,"", 0,0);


        }

        /// <summary>
        /// Uložení všech parametrů okna a jeho uzavření - tlačítko ULOŽ FINANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulozFinanceButton_Click(object sender, RoutedEventArgs e)
        {
            spravce.UlozPrijmy(0);
            spravce.UlozVydaje(0);
            spravce.UlozID();
            Close();
        }

        /// <summary>
        /// Odebrání vybraného záznamu Výdaje - pravé tlačítko ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void odeberVyButton_Click(object sender, RoutedEventArgs e)
        {
            if(vybranyZaznam == null)
                MessageBox.Show("Nic jsi nevybral!", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    spravce.OdeberFinancniZaznam(vybranyZaznam, zvire, 0);
                }
                catch
                {
                    MessageBox.Show("Chyba pri odebirani zaznamu", "Chyba" ,MessageBoxButton.OK, MessageBoxImage.Error);
                }
                FinanceWindow okno = new FinanceWindow(spravce, zvire);
                Close();
                okno.ShowDialog();
            }
              
        }

        /// <summary>
        /// Přidání nového záznamu nebo úprava vybraného záznamu Výdaje - pravé tlačítko ULOŽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulozVyButton_Click(object sender, RoutedEventArgs e)
        {
            if(vybranyZaznam == null || vybranyZaznam.TypOperace == 1)
            {
                try
                {
                    spravce.ZadejFinanceZaznam(nazevVyTextBox.Text, popisVyTextBox.Text, cenaVyTextBox.Text,
                                               matkaVyTextBox.Text.ToLower().Trim(), vrhVyTextBox.Text, datumVyTextBox.Text, 0, 0,0,null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                spravce.SpocitejZisk(0);
                vydajeMaruskaTextBlock.DataContext = spravce.SpocitejBilanci(0, 0, 0,"", 0,0);
                vydajeBaruskaTextBlock.DataContext = spravce.SpocitejBilanci(0, 0, 1,"", 0,0);
                vydajeOstatniTextBlock.DataContext = spravce.SpocitejBilanci(0, 0, 2,"", 0,0);
                Close();
                FinanceWindow okno = new FinanceWindow(spravce,zvire);
                okno.ShowDialog();
            }
            else
            {
                spravce.ZadejFinanceZaznam(nazevVyTextBox.Text, popisVyTextBox.Text, cenaVyTextBox.Text,
                                           matkaVyTextBox.Text.ToLower().Trim(), vrhVyTextBox.Text, datumVyTextBox.Text, 0, 0, 1, vybranyZaznam);
                Close();
                FinanceWindow okno = new FinanceWindow(spravce, zvire);
                okno.ShowDialog();
            }
        }

        /// <summary>
        /// Odebrání vybraného záznamu Příjmy - levé tlačítko ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void odeberPrButton_Click(object sender, RoutedEventArgs e)
        {
            if(vybranyZaznam == null)
                MessageBox.Show("Nic jsi nevybral!", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);

            else
            {
                spravce.OdeberFinancniZaznam(vybranyZaznam, zvire, 1);
                FinanceWindow okno = new FinanceWindow(spravce, zvire);
                Close();
                okno.ShowDialog();
            }
        }

        /// <summary>
        /// Přidání nového záznamu nebo úprava vybraného záznamu Příjmy - levé tlačítko ULOŽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulozPrButton_Click(object sender, RoutedEventArgs e)
        {
            if(vybranyZaznam == null || vybranyZaznam.TypOperace == 0)
            {
                try
                {
                    spravce.ZadejFinanceZaznam(nazevPrTextBox.Text, popisPrTextBox.Text, cenaPrTextBox.Text,
                                               matkaPrTextBox.Text.ToLower().Trim(), vrhPrTextBox.Text, datumPrTextBox.Text, 1, 0,0,null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                spravce.SpocitejZisk(0);
                prijmyMaruskaTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 0,"", 0,0);
                prijmyBaruskaTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 1,"", 0,0);
                prijmyOstatniTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 2,"", 0,0);
                Close();
                FinanceWindow okno = new FinanceWindow(spravce, zvire);
                okno.ShowDialog();
            }
            else
            {
                spravce.ZadejFinanceZaznam(nazevPrTextBox.Text, popisPrTextBox.Text, cenaPrTextBox.Text,
                                               matkaPrTextBox.Text.ToLower().Trim(), vrhPrTextBox.Text, datumPrTextBox.Text, 1, 0,1,vybranyZaznam);
                Close();
                FinanceWindow okno = new FinanceWindow(spravce, zvire);
                okno.ShowDialog();
            }
        }

        /// <summary>
        /// Uložení vybrané transakce v sekci PŘÍJMŮ na základě změny výběru v tomto ListBoxu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prijmyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranyZaznam = (Finance)prijmyListBox.SelectedItem;
        }

        /// <summary>
        /// Uložení vybrané transakce v sekci VÝDAJŮ na základě změny výběru v tomto ListBoxu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vydajeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranyZaznam = (Finance)vydajeListBox.SelectedItem;
        }

        /// <summary>
        /// Výpočet příjmů a výdajů pro konkrétní VRH - tlačítko SPOČÍTEJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cisloVrhuTextBox.Text == "")
                MessageBox.Show("Nezadal jsi žádné číslo!", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                prijmyVrhTextBlock.DataContext = spravce.SpocitejBilanci(1, 0, 3,"", byte.Parse(cisloVrhuTextBox.Text),0);
                vydajeVrhTextBlock.DataContext = spravce.SpocitejBilanci(0, 0, 3,"", byte.Parse(cisloVrhuTextBox.Text),0);
            }
        }
    }
}
