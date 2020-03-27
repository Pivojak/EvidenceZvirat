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
    /// Interakční logika pro OdcerveniWindow.xaml
    /// </summary>
    public partial class OdcerveniWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Ovce, které se týkají záznamy o ODČERVENÍ
        /// </summary>
        private Ovce vztaznaOvce;
        /// <summary>
        /// Vybraný záznam o odčervení
        /// </summary>
        private Odcerveni zaznam;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="spravce">Správce aplikace</param>
        /// <param name="vztaznaOvce">Ovce, které se týkají záznamy o ODČERVENÍ</param>
        public OdcerveniWindow(SpravceZvirat spravce, Ovce vztaznaOvce)
        {
            InitializeComponent();
            this.spravce = spravce;
            this.vztaznaOvce = vztaznaOvce;
            odcerveniListBox.DataContext = spravce.VyberOdcerveni(vztaznaOvce.EvidencniCislo);
        }

        /// <summary>
        /// Smazání vybraného záznamu o odčervení
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            if(zaznam != null)
            {
                odpoved = spravce.ZadejOckovaniOdcerveni(1, 2, "", "", "", "", "", "", null, zaznam);
            }
            if (odpoved)
                Close();
        }

        /// <summary>
        /// Uložení zadaných hodnot -- Přidání nového NEBO úprava stávajícího záznamu o ODČERVENÍ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UlozButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            // Nový záznam
            if(zaznam == null)
            {
                try
                {
                    odpoved = spravce.ZadejOckovaniOdcerveni(1, 0, vztaznaOvce.EvidencniCislo, datumTextBox.Text, pripravekTextBox.Text, ucelTextBox.Text,
                                               poznamkaTextBox.Text, cenaTextBox.Text, null, null);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (odpoved)
                    Close();
            }
            // Úprava stávajícího
            else
            {
                try
                {
                    odpoved = spravce.ZadejOckovaniOdcerveni(1,1, vztaznaOvce.EvidencniCislo, datumTextBox.Text, pripravekTextBox.Text, ucelTextBox.Text,
                                               poznamkaTextBox.Text, cenaTextBox.Text, null,zaznam);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (odpoved)
                    Close();
            }
        }

        /// <summary>
        /// Změna vybraného záznamu v LIST boxu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdcerveniListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zaznam = (Odcerveni)odcerveniListBox.SelectedItem;
        }
    }
}
