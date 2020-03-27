using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interakční logika pro VeterinaOvceWindow.xaml
    /// </summary>
    public partial class VeterinaOvceWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Upravovaná instance OVCE
        /// </summary>
        private Ovce ovce;
        /// <summary>
        /// Veterinární záznamy pro danou ovci
        /// </summary>
        private ObservableCollection<Veterina> vybraneZaznamy;
        /// <summary>
        /// Vybraný veterinární záznam
        /// </summary>
        private Veterina vybranyZaznam;
        
        /// <summary>
        /// Konstruktor pro zobrazeni veterinarnich zaznamu OVCE
        /// </summary>
        /// <param name="spravceZv">Spravce</param>
        /// <param name="vztaznaOvce">Ovce, ktere se tykaji dane zaznamy</param>
        public VeterinaOvceWindow(SpravceZvirat spravceZv, Ovce vztaznaOvce)
        {
            InitializeComponent();
            spravce = spravceZv;
            ovce = vztaznaOvce;
            vybraneZaznamy = spravce.VyberZaznamy(null,ovce);
            veterinaListBox.DataContext = vybraneZaznamy;
        }

        /// <summary>
        /// Uložení formuláře pro přidání / úpravu veterinární záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Přidání nového záznamu
            if(vybranyZaznam == null)
            {
                try
                {
                    spravce.ZadejVeterinarniZaznam(null, ovce, vybranyZaznam, ucelTextBox.Text, ukonyTextBox.Text,
                                                   datumTextBox.Text, lekyTextBox.Text, cenaTextBox.Text, 0);
                    vybraneZaznamy = spravce.VyberZaznamy(null, ovce);
                    Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            // Úprava záznamu
            else
            {
                try
                {
                    spravce.ZadejVeterinarniZaznam(null, ovce, vybranyZaznam, ucelTextBox.Text, ukonyTextBox.Text,
                                                   datumTextBox.Text, lekyTextBox.Text, cenaTextBox.Text, 1);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            spravce.UlozID();
            spravce.UlozVeterina();
        }

        /// <summary>
        /// Obsluha události, změna vybraná položka v seznamu veterinárních záznamů
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VeterinaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranyZaznam = (Veterina)veterinaListBox.SelectedItem;
        }


    }
}
