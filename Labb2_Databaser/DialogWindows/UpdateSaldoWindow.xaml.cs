using Labb2_Databaser.Model;
using Labb2_Databaser.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for UpdateSaldoWindow.xaml
    /// </summary>
    public partial class UpdateSaldoWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public Butiker? SelectedStore { get; private set; }
        public Böcker? SelectedBook { get; private set; }
        public int Quantity { get; private set; }

        public UpdateSaldoWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

        }

        private void StoreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedButik = StoreComboBox.SelectedItem as Butiker;

        }

        private void BookComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedBook = BookComboBox.SelectedItem as Böcker;


        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedStore = StoreComboBox.SelectedItem as Butiker;
            SelectedBook = BookComboBox.SelectedItem as Böcker;

            if (SelectedStore?.ButikId == -1 && SelectedBook?.FörlagId == -1 && string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Välj en butik, en bok och ange ett antal.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedStore?.ButikId == -1)
            {
                MessageBox.Show("Välj en butik.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedBook?.FörlagId == -1)
            {
                MessageBox.Show("Välj en bok.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                MessageBox.Show("Ange ett giltigt antal.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Quantity = quantity;

            if (Quantity < 0)
            {
                MessageBox.Show("Antalet kan inte vara negativt.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedBook = _viewModel?.Books?.FirstOrDefault(b => b.FörlagId == -1);
            DialogResult = false;
            Close();
        }
    }
}