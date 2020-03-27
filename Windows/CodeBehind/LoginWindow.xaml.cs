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
    /// Interakční logika pro LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Správce aplikace
        /// </summary>
        private SpravceZvirat spravce;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="sp"></param>
        public LoginWindow(SpravceZvirat sp)
        {
            InitializeComponent();
            spravce = sp;
        }

        /// <summary>
        /// Uložení zadaných hodnot a porovnání zda jsou v pořádku - případně přihlášení do sekce FINANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginTextBox.Text == "tom" && hesloTextBox.Text == "123456")
            {
                Close();
                FinanceWindow financeOkno = new FinanceWindow(spravce, 0);
                financeOkno.ShowDialog();
                
            }
            else
                MessageBox.Show("Špatně zadané informace, zkus to znovu!", "POZOR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
