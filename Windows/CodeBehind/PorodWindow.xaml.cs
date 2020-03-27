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
    /// Interakční logika pro PorodWindow.xaml
    /// </summary>
    public partial class PorodWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Matka - prasnice, pro kterou se evidují porody
        /// </summary>
        private Svine svine;
        /// <summary>
        /// Upravovaný záznam o porodu
        /// </summary>
        private Porod zaznam;

        /// <summary>
        /// Konstruktor pro upravení porodů prasnice 
        /// </summary>
        /// <param name="spravceZv">Správce aplikace</param>
        /// <param name="svine">Vztažná prasnice</param>
        public PorodWindow(SpravceZvirat spravceZv, Svine svine)
        {
            InitializeComponent();
            spravce = spravceZv;
            this.svine = svine;

            spravce.SeradPorodyPrase();
            
            porodListBox.DataContext = spravce.VyberPorodyPrase(svine);

        }

        /// <summary>
        /// Uložení záznamů - přidání nového NEBO úprava stávajícího
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            byte sv;
            bool brezostPomocna;
            bool ok = false;
            if (svine.Jmeno == "Maruska")
                sv = 0;
            else
                sv = 1;
            if (kontrolaBrezostiCheckBox.IsChecked == true)
                brezostPomocna = true;
            else
                brezostPomocna = false;
            // Nový záznam
            if(zaznam == null)
            {
                try
                {
                   ok = spravce.ZadejPorod(0, sv, 0,"", null, "", "", "", "",false, testBrezostiTextBox.Text, zapusteniTextBox.Text,"", "");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            // Úprava stávajícího
            else
            {
                try
                {
                  ok = spravce.ZadejPorod(0, sv, 1,"", zaznam, zivaTextBox.Text, mrtvaTextBox.Text,
                                       odchovanaTextBox.Text, prubehPoroduTextBox.Text, brezostPomocna,
                                       testBrezostiTextBox.Text, zapusteniTextBox.Text,"", narozeniTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (ok)
            {
                spravce.SpoctiPotomkyPrase(svine);
                Close();
            }
                

        }

        /// <summary>
        /// Změna vybraného porodu v LISTboxu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void porodListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zaznam = (Porod)porodListBox.SelectedItem;
        }
    }
}
