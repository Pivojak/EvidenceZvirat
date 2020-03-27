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
    /// Interakční logika pro PorodOvceWindow.xaml
    /// </summary>
    public partial class PorodOvceWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Vybraný porodní záznam
        /// </summary>
        private Porod zaznam;
        /// <summary>
        /// Ovce, které se dané záznamy týkají
        /// </summary>
        private Ovce vztaznaOvce;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="spravceZv">Správce aplikace</param>
        /// <param name="vztazna">Ovce, které se záznamy týkají</param>
        public PorodOvceWindow(SpravceZvirat spravceZv,Ovce vztazna)
        {
            InitializeComponent();
            spravce = spravceZv;
            vztaznaOvce = vztazna;
            porodListBox.DataContext = spravce.VyberPorodyOvce(vztaznaOvce.EvidencniCislo);
        }

        /// <summary>
        /// Kliknutí na tlačítko ULOŽ - přidání nového záznamu NEBO úprava stávajícího
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            // Nový záznam
            if(zaznam == null)
            {
                try
                {
                    odpoved = spravce.ZadejPorod(1, 2, 0, vztaznaOvce.EvidencniCislo, null, "", "", "", "",
                                   false, "", mesicZapusteniTextBox.Text, mesicNarozeniTextBox.Text, "");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }
            // Úprava stávajícího záznamu
            else
            {
                try
                {
                    odpoved = spravce.ZadejPorod(1, 2, 1, vztaznaOvce.EvidencniCislo, zaznam, zivaTextBox.Text, mrtvaTextBox.Text, odchovanaTextBox.Text, prubehPoroduTextBox.Text,
                                   false, "", mesicZapusteniTextBox.Text, mesicNarozeniTextBox.Text, datumNarozeniJehnatTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }

            if (odpoved)
                Close();
                
        }

        /// <summary>
        /// Odebrání porodního záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            odpoved = spravce.ZadejPorod(1, 2, 2, "", zaznam, "", "", "", "", false, "", "", "","");
            if (odpoved)
            {
                Close();
                PorodOvceWindow okno = new PorodOvceWindow(spravce, vztaznaOvce);
                okno.ShowDialog();
            }
        }

        /// <summary>
        /// Změna vybraného porodního záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void porodListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zaznam = (Porod)porodListBox.SelectedItem;
            DataContext = zaznam;
        }
    }
}
