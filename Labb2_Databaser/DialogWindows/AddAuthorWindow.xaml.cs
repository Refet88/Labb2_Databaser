using Labb2_Databaser.Model;
using Labb2_Databaser.ViewModel;
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

namespace Labb2_Databaser.DialogWindows
{
    /// <summary>
    /// Interaction logic for AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        public string Förnamn { get; set; } = string.Empty;
        public string Efternamn { get; set; } = string.Empty;
        public DateTime? Födelsedatum { get; set; }
        public DateTime? Dödsdatum { get; set; }

        public AddAuthorWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Förnamn) && !string.IsNullOrWhiteSpace(Efternamn))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ange både förnamn och efternamn.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}