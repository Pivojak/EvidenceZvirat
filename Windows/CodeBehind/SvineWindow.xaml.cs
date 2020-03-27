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
    /// Interakční logika pro SvineWindow.xaml
    /// </summary>
    public partial class SvineWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Prasnice, která se upravuje
        /// </summary>
        private Svine Svine;

        /// <summary>
        /// Základní konstruktor 
        /// </summary>
        /// <param name="spravce">Správce aplikace</param>
        /// <param name="svine">Upravovaná prasnice</param>
        public SvineWindow(SpravceZvirat spravce, Svine svine)
        {
            InitializeComponent();
            this.spravce = spravce;
            Svine = svine;
            DataContext = Svine;
            spravce.SpoctiPotomkyPrase(Svine);
        }

        /// <summary>
        /// Metoda pro otevření okna pro úpravu a zobrazení porodů daného zvířete - tlačítko POROD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PorodButton_Click(object sender, RoutedEventArgs e)
        {
            PorodWindow porodOkno = new PorodWindow(spravce,Svine);
            porodOkno.ShowDialog();
        }

        /// <summary>
        /// Metoda pro otevření okna pro výpis veterinárních záznamů - tlačítko VETERINA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VeterinaButton_Click(object sender, RoutedEventArgs e)
        {
            VeterinaPraseWindow vetOkno = new VeterinaPraseWindow(spravce,Svine);
            spravce.VyberZaznamy(Svine,null);
            vetOkno.ShowDialog();
        }

        /// <summary>
        /// Uložení všech zadaných hodnot do okna a jejich úprava pro danou prasnici - tlačítko OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Svine.Krmivo = krmivoTextBox.Text;
            Svine.Popis = popisTextBox.Text;
            Svine.Vaha = int.Parse(vahaTextBox.Text);
            Svine.DatumNarozeni = DateTime.Parse(narozeniTextBox.Text);
            spravce.UlozID();

            Close();
        }
    }
}
