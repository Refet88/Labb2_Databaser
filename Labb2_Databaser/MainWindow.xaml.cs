using Labb2_Databaser.Model;
using Labb2_Databaser.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb2_Databaser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AdlibrisContext _context;
        private string _originalValue = " ";
        public MainWindow()
        {
            InitializeComponent();
            _context = new AdlibrisContext();

            
            DataContext = new MainWindowViewModel(_context);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var selectedItem = dataGrid?.SelectedItem as LagerSaldo;
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.OriginalSource is DependencyObject source)
            {
                var button = FindVisualParent<Button>(source);
                if (button != null)
                {
                    
                    return;
                }
            }

            
            DeselectAllDataGrids();
        }

        private void DeselectAllDataGrids()
        {
            DeselectDataGridsInVisualTree(this);
        }

        private void DeselectDataGridsInVisualTree(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is DataGrid dataGrid)
                {
                    dataGrid.SelectedItem = null;
                }
                DeselectDataGridsInVisualTree(child);
            }
        }

        private static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent) return parent;
                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var selectedTab = ((TabControl)e.Source).SelectedItem as TabItem;
                if (selectedTab != null && selectedTab.Header.ToString() == "Böcker")
                {
                    var viewModel = DataContext as MainWindowViewModel;
                    if (viewModel != null && viewModel.SelectedButik?.ButikId == -1)
                    {
                        viewModel.Books?.Clear();
                    }
                    else if (selectedTab.Header.ToString() == "Uppdatera Lagersaldo")
                    {
                        viewModel?.LoadBooks();
                    }
                }
            }
        }

        private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.EditingElement is ComboBox comboBox && e.Column.Header.ToString() == "Förlag")
            {
                
                var originalPublisher = comboBox.SelectedItem as Förlag;

                
                var newPublisher = new Förlag
                {
                    Namn = originalPublisher!.Namn,
                    Adress = originalPublisher!.Adress,
                    PostNummer = originalPublisher!.PostNummer,
                    Ort = originalPublisher!.Ort,
                    TelefonNummer = originalPublisher?.TelefonNummer
                };

                
                comboBox.SelectedItem = newPublisher;
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null) return;

            
            var element = e.OriginalSource as DependencyObject;
            while (element != null && !(element is DataGridCell))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            if (element is DataGridCell cell)
            {
                
                dataGrid.CurrentCell = new DataGridCellInfo(cell);
                dataGrid.BeginEdit();
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var textBox = e.EditingElement as TextBox;
                if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    
                    textBox.Text = _originalValue;
                }
            }
            else if (e.EditAction == DataGridEditAction.Cancel)
            {
                
                var textBox = e.EditingElement as TextBox;
                if (textBox != null)
                {
                    textBox.Text = _originalValue;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context.Dispose();
        }
    }
}