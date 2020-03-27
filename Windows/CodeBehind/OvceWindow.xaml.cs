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
    /// Interakční logika pro OvceWindow.xaml
    /// </summary>
    public partial class OvceWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravceZvirat;
        /// <summary>
        /// Upravovaná instance OVCE
        /// </summary>
        private Ovce upravovanaOvce;

        /// <summary>
        /// Základní konstruktor - nová ovce
        /// </summary>
        /// <param name="spravce"></param>
        public OvceWindow(SpravceZvirat spravce)
        {
            InitializeComponent();
            spravceZvirat = spravce;
            DataContext = spravceZvirat;
            // Deaktivace tlacitek, pokud se vytvari nova ovce, aby neslo kliknout na vedlejsi tlacitka, ktera nic 
            // pri vytvoreni ovce neobsahuji. Soucasne se trochu zneviditelni pomoci OPACITY
            veterinaButton.IsEnabled = false;
            veterinaButton.Opacity = 0.7;

            porodButton.IsEnabled = false;
            porodButton.Opacity = 0.7;

            ockovaniButton.IsEnabled = false;
            ockovaniButton.Opacity = 0.7;

            odcerveniButton.IsEnabled = false;
            odcerveniButton.Opacity = 0.7;
        }

        /// <summary>
        /// Konstruktor při úpravě stávající OVCe - předání instance v parametrech
        /// </summary>
        /// <param name="spravce"></param>
        /// <param name="ovce"></param>
        public OvceWindow(SpravceZvirat spravce,Ovce ovce)
        {
            InitializeComponent();
            spravceZvirat = spravce;
            upravovanaOvce = ovce;
            DataContext = upravovanaOvce;
            jehneStavajiciListBox.DataContext = spravce.VyberJehnata(upravovanaOvce.EvidencniCislo);
            evidencniCisloOtceTextBox.DataContext = upravovanaOvce.Otec;
            evidencniCisloMatkyTextBox.DataContext = upravovanaOvce.Matka;
        }

        /// <summary>
        /// Uložení všech zadaných hodnot do okna -- Vytvoření nové ovce NEBO úprava stávající
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UlozButton_Click(object sender, RoutedEventArgs e)
        {
            // Nový záznam
            if(upravovanaOvce == null)
            {
                byte stav = 2;
                byte pohlavi = 2;
                bool odpoved = false; ;
                // Jedná se o dospělou OVCI
                if (zenaCheckBox.IsChecked == true)
                {
                    pohlavi = 0;
                    stav = 1;
                }
                // Jedná se o dospělého BERANA
                if (muzCheckBox.IsChecked == true)
                {
                    pohlavi = 1;
                    stav = 1;
                }
                // jedná se o mladou OVCI
                if (jehnickaCheckBox.IsChecked == true)
                {
                    stav = 0;
                    pohlavi = 0;
                }
                // Jedná se o mladého BERANA
                if (beranekCheckBox.IsChecked == true)
                {
                    stav = 0;
                    pohlavi = 1;
                }

                try
                {
                    odpoved = spravceZvirat.ZadejOvci(0, stav, pohlavi, "", evidencniCisloTextBox.Text, "", datumZarazeniTextBox.Text, "", datumNarozeniTextBox.Text, null, "", "", "", "", evidencniCisloMatkyTextBox.Text, evidencniCisloOtceTextBox.Text, "", "", "");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
           
                if(odpoved)
                    Close();
            }
            // Úprava stávající instance ovce
            else
            {
                bool stanSeMatkou = false;
                byte stav_puvodni = upravovanaOvce.Stav; 
                byte stav = 2;
                byte pohlavi = 2;
                bool odpoved = false; ;
                // Jedná se o dospělou OVCI
                if (zenaCheckBox.IsChecked == true)
                {
                    pohlavi = 0;
                    stav = 1;
                }
                // Jedná se o dospělého BERANA
                if (muzCheckBox.IsChecked == true)
                {
                    pohlavi = 1;
                    stav = 1;
                }
                // jedná se o mladou OVCI
                if (jehnickaCheckBox.IsChecked == true)
                {
                    stav = 0;
                    pohlavi = 0;
                }
                // Jedná se o mladého BERANA
                if (beranekCheckBox.IsChecked == true)
                {
                    stav = 0;
                    pohlavi = 1;
                }
                // Převod mladé OVCE na dospělou
                if(stav != stav_puvodni)
                {
                    try
                    {
                        odpoved = spravceZvirat.ZadejOvci(3, stav, pohlavi, "", "", "", "", "", "", upravovanaOvce, "", "", "", "", "", "", "", "", "");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    stanSeMatkou = true;
                }

                if (!stanSeMatkou)
                {
                    try
                    {
                        odpoved = spravceZvirat.ZadejOvci(1, stav, pohlavi, jmenoTextBox.Text, evidencniCisloTextBox.Text, strihaniCislo_ovceTextBox.Text, datumZarazeniTextBox.Text, datumVyrazeniTextBox.Text,
                                                datumNarozeniTextBox.Text, upravovanaOvce, linieTextBox.Text, duvodVyrazeniTextBox.Text, klasifikaceTextBox.Text, popisTextBox.Text,
                                                evidencniCisloMatkyTextBox.Text, evidencniCisloOtceTextBox.Text, mrtvaAktualniTextBox.Text, zivaAktualniTextBox.Text, odchovAktualniTextBox.Text);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                
                if (odpoved)
                    Close();
            }

            
        }

        /// <summary>
        /// Otevření okna odčervení
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdcerveniButton_Click(object sender, RoutedEventArgs e)
        {
            OdcerveniWindow okno = new OdcerveniWindow(spravceZvirat,upravovanaOvce);
            okno.ShowDialog();
        }

        /// <summary>
        /// Otevření okna očkování
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OckovaniButton_Click(object sender, RoutedEventArgs e)
        {
            OckovaniWindow okno = new OckovaniWindow(spravceZvirat, upravovanaOvce);
            okno.ShowDialog();
        }

        /// <summary>
        /// Otevření okna VETERINA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VeterinaButton_Click(object sender, RoutedEventArgs e)
        {
            VeterinaOvceWindow okno = new VeterinaOvceWindow(spravceZvirat, upravovanaOvce);
            okno.ShowDialog();
        }

        /// <summary>
        /// Otevření okna porodu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PorodButton_Click(object sender, RoutedEventArgs e)
        {
            PorodOvceWindow okno = new PorodOvceWindow(spravceZvirat,upravovanaOvce);
            okno.ShowDialog();
        }
    }
}
