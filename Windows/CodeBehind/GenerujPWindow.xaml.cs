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
    /// Interakční logika pro GenerujPWindow.xaml
    /// </summary>
    public partial class GenerujPWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;

        /// <summary>
        /// Základní konstruktor 
        /// </summary>
        /// <param name="spr">Správce aplikace</param>
        public GenerujPWindow(SpravceZvirat spr)
        {
            InitializeComponent();
            spravce = spr;
        }

        /// <summary>
        /// Uložení zadaných hodnot - vytvoření zadaného počtu SELAT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Svine matka;
            if (maruskaCheckBox.IsChecked == true)
                matka = spravce.Maruska;
            else if (baruskaCheckBox.IsChecked == true)
                matka = spravce.Baruska;
            else
                matka = null;

            try
            {
                spravce.GenerujSelata(int.Parse(pocetTextBox.Text), DateTime.Parse(datumTextBox.Text), matka);
                Close();
                spravce.UlozPrase();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            
        }
    }
}
