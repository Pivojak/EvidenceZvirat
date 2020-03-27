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
    /// Interakční logika pro ListPraseWindow.xaml
    /// </summary>
    public partial class ListPraseWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;
        /// <summary>
        /// Vybrané prase z listBoxu pod jednou z PRASNIC
        /// </summary>
        private Prase vybraneSele;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="spravceZv">Správce aplikace</param>
        public ListPraseWindow(SpravceZvirat spravceZv)
        {

            spravce = spravceZv;
            InitializeComponent();
            DataContext = spravce;
            // Nastavení dataContextu pro jednotlivé ListBoxy
            maruskaListBox.DataContext = spravce.SeleMaruska;
            baruskaListBox.DataContext = spravce.SeleBaruska;
            // První Prasnice nemá evidovaná žádná seleta
            if (spravce.SeleBaruska.Count == 0)
            {
                odeberBaruska.IsEnabled = false;
                odeberBaruska.Opacity = 0.7;
                upravBaruska.IsEnabled = false;
                upravBaruska.Opacity = 0.7;
            }
            // Druhá prasnice nemá evidována žádný selata
            else if (spravce.SeleMaruska.Count == 0)
            {
                odeberMaruska.IsEnabled = false;
                odeberMaruska.Opacity = 0.7;
                upravMaruska.IsEnabled = false;
                upravMaruska.Opacity = 0.7;
            }
        }

        /// <summary>
        /// Změna vybraného selete u první z PRASNIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaruskaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybraneSele = (Prase)maruskaListBox.SelectedItem;
        }

        /// <summary>
        /// Změna vybraného selete u druhé z PRASNIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaruskaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vybraneSele = (Prase)baruskaListBox.SelectedItem;
        }

        /// <summary>
        /// Úprava vybraného selete u první z PRASNIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpravMaruska_Click(object sender, RoutedEventArgs e)
        {
            if (vybraneSele != null)
            {
                Close();
                PraseWindow oknoPrase = new PraseWindow(spravce, vybraneSele);
                oknoPrase.poradoveCislo_praseTextBox.Focusable = false;

                if (vybraneSele.Matka.Jmeno == "Maruska")
                    oknoPrase.maruskaCheckBox.IsChecked = true;

                if (vybraneSele.Matka.Jmeno == "Baruska")
                    oknoPrase.baruskaCheckBox.IsChecked = true;

                if (vybraneSele.Pohlavi == "Žena")
                    oknoPrase.zenaCheckBox.IsChecked = true;
                else if (vybraneSele.Pohlavi == "Muž")
                    oknoPrase.muzCheckBox.IsChecked = true;

                oknoPrase.ShowDialog();
            }
        }

        /// <summary>
        /// Odebrání selete u první z PRASNIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberMaruska_Click(object sender, RoutedEventArgs e)
        {
            if(vybraneSele != null)
            {
                spravce.OdeberPrase(vybraneSele);
                Close();
                ListPraseWindow okno = new ListPraseWindow(spravce);
                okno.ShowDialog();
            }
            
        }

        /// <summary>
        /// Úprava vybraného selete u druhé z PRASNIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpravBaruska_Click(object sender, RoutedEventArgs e)
        {
            if (vybraneSele != null)
            {
                Close();
                PraseWindow oknoPrase = new PraseWindow(spravce, vybraneSele);
                oknoPrase.poradoveCislo_praseTextBox.Focusable = false;

                if (vybraneSele.Matka.Jmeno == "Maruska")
                    oknoPrase.maruskaCheckBox.IsChecked = true;

                if (vybraneSele.Matka.Jmeno == "Baruska")
                    oknoPrase.baruskaCheckBox.IsChecked = true;

                if (vybraneSele.Pohlavi == "Žena")
                    oknoPrase.zenaCheckBox.IsChecked = true;
                else if (vybraneSele.Pohlavi == "Muž")
                    oknoPrase.muzCheckBox.IsChecked = true;

                oknoPrase.ShowDialog();
            }
        }

        /// <summary>
        /// Odebrání vybraného selete u druhé z PRASNIC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OdeberBaruska_Click(object sender, RoutedEventArgs e)
        {
            if(vybraneSele != null)
            {
                spravce.OdeberPrase(vybraneSele);
                ListPraseWindow okno = new ListPraseWindow(spravce);
                okno.ShowDialog();
            }
            
        }

        /// <summary>
        /// Zobrazení okna první PRASNICE se všemi informacemi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaruskaButton_Click(object sender, RoutedEventArgs e)
        {
            SvineWindow oknoSvine = new SvineWindow(spravce, spravce.Maruska);
            oknoSvine.ShowDialog();
        }

        /// <summary>
        /// Zobrazení okna druhé PRASNICE se všemi informacemi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaruskaButton_Click(object sender, RoutedEventArgs e)
        {
            SvineWindow oknoSvine = new SvineWindow(spravce, spravce.Baruska);
            oknoSvine.ShowDialog();
        }
    }
}
