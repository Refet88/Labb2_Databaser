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
    /// Interaction logic for AddPublisherWindow.xaml
    /// </summary>
    public partial class AddPublisherWindow : Window
    {
        public string Namn { get; set; } = string.Empty;
        public string TelefonNummer { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public string PostNummer { get; set; } = string.Empty;
        public string Ort { get; set; } = string.Empty;

        public AddPublisherWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Namn) && !string.IsNullOrWhiteSpace(Adress) && !string.IsNullOrWhiteSpace(PostNummer) && !string.IsNullOrWhiteSpace(Ort))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ange alla obligatoriska fält.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
