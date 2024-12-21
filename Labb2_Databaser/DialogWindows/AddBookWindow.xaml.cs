using Labb2_Databaser.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Labb2_Databaser.DialogWindows
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private readonly AdlibrisContext _context;
        public Böcker? NewBook { get; private set; }
        public ObservableCollection<Författare> Författare { get; }
        public ObservableCollection<Förlag> Publisher { get; }

        public AddBookWindow(AdlibrisContext context)
        {
            InitializeComponent();
            _context = context;
            Författare = new ObservableCollection<Författare>(_context.Författare.ToList());
            Publisher = new ObservableCollection<Förlag>(_context.Förlag.ToList());
            DataContext = this;
            AuthorComboBox.ItemsSource = Författare;
            PublisherComboBox.ItemsSource = Publisher;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAuthor = AuthorComboBox.SelectedItem as Författare;
                var selectedPublisher = PublisherComboBox.SelectedItem as Förlag;

                if (!decimal.TryParse(PrisTextBox.Text, out var pris))
                {
                    MessageBox.Show("Ange ett giltigt pris.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NewBook = new Böcker
                {
                    Isbn13 = IsbnTextBox.Text,
                    Titel = TitelTextBox.Text,
                    Språk = SprakTextBox.Text,
                    Pris = pris,
                    Utgivningsdatum = DateOnly.FromDateTime(UtgivningsdatumPicker.SelectedDate ?? DateTime.Now),
                    AntalSidor = int.TryParse(AntalSidorTextBox.Text, out var antalSidor) ? antalSidor : 0,
                    FörlagId = selectedPublisher?.FörlagId ?? 0,
                    FörfattareBöcker = new List<FörfattareBöcker>
                {
                    new FörfattareBöcker
                    {
                        FörfattareId = selectedAuthor?.Id ?? 0,
                        Isbn = IsbnTextBox.Text
                    }
                }
                };


                if (string.IsNullOrWhiteSpace(NewBook.Isbn13) || string.IsNullOrWhiteSpace(NewBook.Titel))
                {
                    MessageBox.Show("Ange både ISBN och Titel.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel inträffade: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddAuthorWindow();
            if (dialog.ShowDialog() == true)
            {
                var newAuthor = new Författare
                {
                    Förnamn = dialog.Förnamn,
                    Efternamn = dialog.Efternamn
                };

                _context.Författare.Add(newAuthor);
                _context.SaveChanges();
                Författare.Add(newAuthor);
                AuthorComboBox.ItemsSource = Författare;
            }
        }

        private void AddPublisher_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddPublisherWindow();
            if (dialog.ShowDialog() == true)
            {
                var newPublisher = new Förlag
                {
                    Namn = dialog.Namn,
                    TelefonNummer = dialog.TelefonNummer,
                    Adress = dialog.Adress,
                    PostNummer = dialog.PostNummer,
                    Ort = dialog.Ort
                };

                _context.Förlag.Add(newPublisher);
                _context.SaveChanges();
                Publisher.Add(newPublisher);
                PublisherComboBox.ItemsSource = Publisher;
            }
        }
    }
}