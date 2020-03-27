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
    /// Interakční logika pro PraseWindow.xaml
    /// </summary>
    public partial class PraseWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravceZvirat;
        /// <summary>
        /// Prase, které je zobrazeno a případně upraveno
        /// </summary>
        private Prase upravovaneSele;
        
        /// <summary>
        /// Základní konstruktor okna - přidání nového prasete
        /// </summary>
        /// <param name="spravcezvirat">Správce aplikace</param>
        public PraseWindow(SpravceZvirat spravcezvirat)
        {
            InitializeComponent();
            spravceZvirat = spravcezvirat;
            // Nastavení kontextu
            DataContext = upravovaneSele;
            // Kliknutí na veterinární tlačítko - zakázáno
            veterinarButton.IsEnabled = false;
            // Průhlednost tlačítka
            veterinarButton.Opacity = 0.7;
        }

        /// <summary>
        /// Konstruktor pro úpravu stávajícího selete
        /// </summary>
        /// <param name="spravcezvirat">Správce aplikace</param>
        /// <param name="sele">Upravované prase</param>
        public PraseWindow(SpravceZvirat spravcezvirat,Prase sele)
        {
            InitializeComponent();
            spravceZvirat = spravcezvirat;
            upravovaneSele = sele;
            DataContext = upravovaneSele;
        }

        /// <summary>
        /// Metoda pro uložení všech zadaných hodnot - Nové prase NEBO úprava stávajícího
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_praseButton_Click(object sender, RoutedEventArgs e)
        {
            // Nové
            if (upravovaneSele == null)
            {
                try
                {
                    spravceZvirat.ZadejSele(jmeno_praseTextBox.Text, vahaTextBox.Text, popisTextBox.Text, zenaCheckBox.IsChecked, muzCheckBox.IsChecked,
                                            maruskaCheckBox.IsChecked, baruskaCheckBox.IsChecked, narozeni_praseTextBox.Text, upravovaneSele);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            // Úprava stávajícího
            else if(upravovaneSele != null)
            {
                try
                {
                    spravceZvirat.ZadejSele(jmeno_praseTextBox.Text, vahaTextBox.Text, popisTextBox.Text, zenaCheckBox.IsChecked, muzCheckBox.IsChecked,
                                            maruskaCheckBox.IsChecked, baruskaCheckBox.IsChecked, narozeni_praseTextBox.Text, upravovaneSele);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            spravceZvirat.UlozID();  
            Close();
            ListPraseWindow okno = new ListPraseWindow(spravceZvirat);
            okno.ShowDialog();
        }

        /// <summary>
        /// Metoda při kliknutí na tlačítko veteriny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VeterinarButton_Click(object sender, RoutedEventArgs e)
        {
            VeterinaPraseWindow veterinaPrase = new VeterinaPraseWindow(spravceZvirat,upravovaneSele);
            spravceZvirat.VyberZaznamy(upravovaneSele,null);
            veterinaPrase.ShowDialog();
        }

        /// <summary>
        /// Kliknutí na tlačítko GENERUJ - možnost vytvoření více prasat současně
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerujButton_Click(object sender, RoutedEventArgs e)
        {
            GenerujPWindow genOkno = new GenerujPWindow(spravceZvirat);
            Close();
            genOkno.ShowDialog();
        }
    }
}
