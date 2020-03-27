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
    /// Interakční logika pro VeterinaPraseWindow.xaml
    /// </summary>
    public partial class VeterinaPraseWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Prase, které se bude upravovat
        /// </summary>
        private Prase upravovaneSele;
        /// <summary>
        /// Vybraný veterinární záznam
        /// </summary>
        private Veterina veterina;

        /// <summary>
        /// Konstruktor okna
        /// </summary>
        /// <param name="spravce">Správce aplikacee</param>
        /// <param name="upravovane">Prase, které se bude upravovat</param>
        public VeterinaPraseWindow(SpravceZvirat spravce, Prase upravovane)
        {
            InitializeComponent();
            this.spravce = spravce;
            upravovaneSele = upravovane;
            DataContext = spravce;
            spravce.VyberZaznamy(upravovaneSele,null);
        }

        /// <summary>
        /// Uložení formuláře pro přidání / úpravu veterinární záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Nový záznam
            if(veterina == null)
            {
                try
                {
                    spravce.ZadejVeterinarniZaznam(upravovaneSele,null, veterina,ucelTextBox.Text, ukonyTextBox.Text, datumTextBox.Text, lekyTextBox.Text,cenaTextBox.Text,0);
                    spravce.VyberZaznamy(upravovaneSele,null);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            // Úprava stávajícího záznamu
            else
            {
                try
                {
                    spravce.ZadejVeterinarniZaznam(upravovaneSele,null,veterina, ucelTextBox.Text, ukonyTextBox.Text, datumTextBox.Text, lekyTextBox.Text, cenaTextBox.Text,1);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            spravce.UlozID();
        }

        /// <summary>
        /// Obsluha události, změna vybraná položka v seznamu veterinárních záznamů
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VeterinaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            veterina = (Veterina)veterinaListBox.SelectedItem;
        }
    }
}
