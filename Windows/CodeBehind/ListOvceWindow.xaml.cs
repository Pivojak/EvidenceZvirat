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
    /// Interakční logika pro ListOvceWindow.xaml
    /// </summary>
    public partial class ListOvceWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Upravovaná ovce
        /// </summary>
        private Ovce vybranaOvce;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="spravceZvirat"></param>
        public ListOvceWindow(SpravceZvirat spravceZvirat)
        {
            InitializeComponent();
            spravce = spravceZvirat;
            spravce.SeradOvceBeranyJehnata();
            // Nastavení DataContextu pro listBoxy
            ovceListBox.DataContext = spravce;
            beranListBox.DataContext = spravce;
            jehneListBox.DataContext = spravce;
            // Výpis počtu ovcí v jednotlivých kategoriích
            pocetOvciTextBlock.Text = "Pocet ovci:\t" + spravce.SeznamOvci.Count.ToString();
            pocetJehnatTextBlock.Text = "Pocet jehnat:\t" + spravce.SeznamJehnat.Count.ToString();
            pocetBeranuTextBlock.Text = "Pocet beranu:\t" + spravce.SeznamBeranu.Count.ToString();
        }

        /// <summary>
        /// Tlačítko pro odebrání jedné ovce, berana nebo jehněte - kliknutí na ODEBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberButton_Click(object sender, RoutedEventArgs e)
        {
            bool odpoved = false;
            // Uživatel vybral nějaké zvíře
            if(ovceListBox != null && vybranaOvce != null)
            {
                try
                {
                    odpoved = spravce.ZadejOvci(2, vybranaOvce.Stav, vybranaOvce.Pohlavi, "", "", "", "", "", "", vybranaOvce, "", "", "", "", null, null, "", "", "");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            // Aktualizace okna, jejím zavřením a opětovným utevřením
            if (odpoved)
            {
                Close();
                ListOvceWindow okno = new ListOvceWindow(spravce);
                okno.ShowDialog();
            }
        }

        /// <summary>
        /// Změna výběru u ListBoxu ovcí
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OvceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranaOvce = (Ovce)ovceListBox.SelectedItem;
            prvniSloupecStackPanel.DataContext = ovceListBox.SelectedItem;
            druhySloupecStackPanel.DataContext = ovceListBox.SelectedItem;
            tretiSloupecStackPanel.DataContext = ovceListBox.SelectedItem;
        }

        /// <summary>
        /// Změna výběru u ListBoxu beranů
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beranListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranaOvce = (Ovce)beranListBox.SelectedItem;
            prvniSloupecStackPanel.DataContext = beranListBox.SelectedItem;
            druhySloupecStackPanel.DataContext = beranListBox.SelectedItem;
            tretiSloupecStackPanel.DataContext = beranListBox.SelectedItem;
        }

        /// <summary>
        /// Změna výběru u ListBoxu jehňat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jehneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybranaOvce = (Ovce)jehneListBox.SelectedItem;
            prvniSloupecStackPanel.DataContext = jehneListBox.SelectedItem;
            druhySloupecStackPanel.DataContext = jehneListBox.SelectedItem;
            tretiSloupecStackPanel.DataContext = jehneListBox.SelectedItem;
        }

        /// <summary>
        /// Upravení vybrané ovce / berana / jehněte - kliknutí na UPRAV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpravButton_Click(object sender, RoutedEventArgs e)
        {
            // Je něco vybráno
            if(vybranaOvce != null)
            {
                spravce.SpoctiPotomkyOvce(0, vybranaOvce);
                // Vytvoreni okna pro upravu nebo vytvoreni nove ovce, v tomto pripade uprava stavajici
                OvceWindow oknoOvce = new OvceWindow(spravce, vybranaOvce);

                if (vybranaOvce.Pohlavi == 0 && vybranaOvce.Stav == 1)
                    oknoOvce.zenaCheckBox.IsChecked = true;
                else if(vybranaOvce.Pohlavi == 1 && vybranaOvce.Stav == 1)
                    oknoOvce.muzCheckBox.IsChecked = true;

                if (vybranaOvce.Pohlavi == 0 && vybranaOvce.Stav == 0)
                    oknoOvce.jehnickaCheckBox.IsChecked = true;
                else if (vybranaOvce.Pohlavi == 1 && vybranaOvce.Stav == 0)
                    oknoOvce.beranekCheckBox.IsChecked = true;

                oknoOvce.ShowDialog();
            }
        }

        /// <summary>
        /// Uložení dat a uzavření okna - kliknutí na OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            spravce.UlozOvce();
            Close();
        }
    }
}
