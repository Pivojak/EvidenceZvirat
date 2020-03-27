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
    /// Interakční logika pro OckovaniWindow.xaml
    /// </summary>
    public partial class OckovaniWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Ovce, které se týkají očkovací záznamy
        /// </summary>
        private Ovce vztaznaOvce;
        /// <summary>
        /// Vybraný očkovací záznam
        /// </summary>
        private Ockovani zaznam;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="spravceZv">Správce aplikace</param>
        /// <param name="vztazna">Ovce, které se týkají očkovací záznamy</param>
        public OckovaniWindow(SpravceZvirat spravceZv,Ovce vztazna)
        {
            InitializeComponent();
            spravce = spravceZv;
            vztaznaOvce = vztazna;

            ockovaniListBox.DataContext = spravce.VyberOckovani(vztaznaOvce.EvidencniCislo);
        }

        /// <summary>
        /// Smazání vybraného očkovacího záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            if (zaznam != null)
            {
                odpoved = spravce.ZadejOckovaniOdcerveni(0, 2, "", "", "", "", "", "", zaznam,null);
            }
            if (odpoved)
                Close();
        }

        /// <summary>
        /// Uložení zadaných hodnot - Vytvoření nového záznamu NEBO úprava stávajícího
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UlozButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            if (zaznam == null)
            {
                try
                {
                    odpoved = spravce.ZadejOckovaniOdcerveni(0, 0, vztaznaOvce.EvidencniCislo, datumTextBox.Text, pripravekTextBox.Text, ucelTextBox.Text,
                                               poznamkaTextBox.Text, cenaTextBox.Text, null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (odpoved)
                    Close();
            }
            else
            {
                try
                {
                    odpoved = spravce.ZadejOckovaniOdcerveni(0, 1, vztaznaOvce.EvidencniCislo, datumTextBox.Text, pripravekTextBox.Text, ucelTextBox.Text,
                                               poznamkaTextBox.Text, cenaTextBox.Text, zaznam,null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (odpoved)
                    Close();
            }
        }

        private void OckovaniListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zaznam = (Ockovani)ockovaniListBox.SelectedItem;
        }
    }
}
