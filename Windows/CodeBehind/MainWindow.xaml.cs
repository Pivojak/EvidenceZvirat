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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EvidenceZvirat
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Správce aplikace - jeho vytvoření
        /// </summary>
        private SpravceZvirat spravce = new SpravceZvirat();

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                // Načtení všech kolekcí
                spravce.NactiOvce();
                spravce.NactiVeterina();
                spravce.NactiPrase();
                // Prasata
                spravce.NactiPrijmy(0);
                spravce.NactiVydaje(0);
                // Ovce
                spravce.NactiPrijmy(1);
                spravce.NactiVydaje(1);
                spravce.NactiID();
                spravce.NactiPorody();
                spravce.NactiOckovani();
                spravce.NactiOdcerveni();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            spravce.Trid();
            DataContext = spravce;
        }

        /// <summary>
        /// Přidání nového prasete - pravé tlačítko PŘIDEJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pridej_praseButton_Click(object sender, RoutedEventArgs e)
        {
            PraseWindow praseWindow = new PraseWindow(spravce);
            praseWindow.ShowDialog();
        }

        /// <summary>
        /// Přidej novou ovci - levé tlačítko PŘIDEJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pridej_ovceButton_Click(object sender, RoutedEventArgs e)
        {
            OvceWindow ovceWindow = new OvceWindow(spravce);
            ovceWindow.ShowDialog();
        }

        /// <summary>
        /// Odebrání ovce ze seznamu - leve tlačítko ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Odeber_ovceButton_Click(object sender, RoutedEventArgs e)
        {
            ListOvceWindow seznamOvce = new ListOvceWindow(spravce);
            seznamOvce.ShowDialog();
        }

        /// <summary>
        /// Odebrání prasete ze seznamu - pravé tlačítko ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Odeber_praseButton_Click(object sender, RoutedEventArgs e)
        {
            ListPraseWindow seznamPrase = new ListPraseWindow(spravce);
            seznamPrase.ShowDialog();
        }

        /// <summary>
        /// Seznam ovcí - levé tlačítko SEZNAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Seznam_ovceButton_Click(object sender, RoutedEventArgs e)
        {
            ListOvceWindow seznamOvce = new ListOvceWindow(spravce);
            seznamOvce.ShowDialog();
        }

        /// <summary>
        /// Seznam prasat - pravé tlačítko SEZNAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Seznam_praseButton_Click(object sender, RoutedEventArgs e)
        {
            ListPraseWindow listPrase = new ListPraseWindow(spravce);
            listPrase.ShowDialog();
        }

        /// <summary>
        /// Finance pro sekci prasat - pravé tlačítko FINANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void financePraseButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow log = new LoginWindow(spravce);
            log.ShowDialog();
        }

        /// <summary>
        /// Finance pro sekci ovcí - levé tlačítko FINANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void financeOvceButton_Click(object sender, RoutedEventArgs e)
        {
            FinanceOvceWindow okno = new FinanceOvceWindow(spravce);
            okno.ShowDialog();
        }

        /// <summary>
        /// Uložení všech dat projektu - tlačítko ULOŽIT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulozitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                spravce.UlozOvce();
                spravce.UlozVeterina();
                spravce.UlozPrase();
                spravce.UlozID();
                spravce.UlozPorody();
                spravce.UlozOckovani();
                spravce.UlozOdcerveni();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}
