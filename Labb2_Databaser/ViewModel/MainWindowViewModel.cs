using Labb2_Databaser.Command;
using Labb2_Databaser.DialogWindows;
using Labb2_Databaser.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb2_Databaser.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AdlibrisContext _context;
        public bool IsAllButikerSelected => SelectedButik?.ButikId == 0;
        public bool IsValidButikerSelected => SelectedButik?.ButikId > 0;

        private ObservableCollection<Förlag>? _publishers;
        public ObservableCollection<Förlag>? Publishers
        {
            get => _publishers;
            set
            {
                _publishers = value;
                RaisePropertyChanged();
            }
        }

        private Förlag? _selectedPublisher;
        public Förlag? SelectedPublisher
        {
            get => _selectedPublisher;
            set
            {
                _selectedPublisher = value;
                RaisePropertyChanged();
                RemovePublisherCommand.RaiseCanExecuteChanged();
                UpdatePublishersCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Butiker>? _butiker;
        public ObservableCollection<Butiker>? Butiker
        {
            get { return _butiker; }
            set
            {
                _butiker = value;
                RaisePropertyChanged();
            }
        }

        private string? _butiksnamn;
        public string? Butiksnamn
        {
            get { return _butiksnamn; }
            set
            {
                _butiksnamn = value;
                RaisePropertyChanged();
            }
        }

        private string? _adress;
        public string? Adress
        {
            get { return _adress; }
            set
            {
                _adress = value;
                RaisePropertyChanged();
            }
        }

        private string? _postNummer;
        public string? PostNummer
        {
            get { return _postNummer; }
            set
            {
                _postNummer = value;
                RaisePropertyChanged();
            }
        }

        private string? _ort;
        public string? Ort
        {
            get { return _ort; }
            set
            {
                _ort = value;
                RaisePropertyChanged();
            }
        }

        private Butiker? _selectedButik;
        public Butiker? SelectedButik
        {
            get { return _selectedButik; }
            set
            {
                _selectedButik = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsAllButikerSelected));
                RaisePropertyChanged(nameof(IsValidButikerSelected));
                LoadBooks();
                LoadLagerSaldo();
                LoadAuthors();
                LoadPublishers();

            }
        }


        private ObservableCollection<Böcker>? _books;
        public ObservableCollection<Böcker>? Books
        {
            get { return _books; }
            set
            {
                _books = value;
                RaisePropertyChanged();
            }
        }

        private Böcker? _selectedBook;
        public Böcker? SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                RaisePropertyChanged();
                RemoveBookCommand.RaiseCanExecuteChanged();
                UpdateBooksCommand.RaiseCanExecuteChanged();

                if (SelectedButik?.ButikId == -1)
                {
                    LagerSaldo?.Clear();
                    RaisePropertyChanged();
                    return;
                }

                FilterLagerSaldo();
            }
        }

        private ObservableCollection<Författare>? _authors;
        public ObservableCollection<Författare>? Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                RaisePropertyChanged();
            }
        }

        private Författare? _selectedAuthor;
        public Författare? SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
                RemoveAuthorCommand.RaiseCanExecuteChanged();
                UpdateAuthorsCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<LagerSaldo>? _lagerSaldo;
        public ObservableCollection<LagerSaldo>? LagerSaldo
        {
            get { return _lagerSaldo; }
            set
            {
                _lagerSaldo = value;
                RaisePropertyChanged();
            }
        }

        private LagerSaldo? _selectedLagerSaldo;
        public LagerSaldo? SelectedLagerSaldo
        {
            get { return _selectedLagerSaldo; }
            set
            {
                _selectedLagerSaldo = value;
                RaisePropertyChanged();
                RemoveBookCommand.RaiseCanExecuteChanged();
                UpdateSaldoCommand.RaiseCanExecuteChanged();


            }
        }

        public DelegateCommand AddBookCommand { get; }
        public DelegateCommand RemoveBookCommand { get; }
        public DelegateCommand AddAuthorCommand { get; }
        public DelegateCommand RemoveAuthorCommand { get; }
        public DelegateCommand AddPublisherCommand { get; }
        public DelegateCommand RemovePublisherCommand { get; }
        public DelegateCommand UpdateSaldoCommand { get; }
        public DelegateCommand UpdateBooksCommand { get; }
        public DelegateCommand UpdateAuthorsCommand { get; }
        public DelegateCommand UpdatePublishersCommand { get; }


        public MainWindowViewModel(AdlibrisContext context)
        {
            _context = context;
            Butiker = new ObservableCollection<Butiker>(_context.Butiker.ToList());
            Butiker.Insert(0, new Butiker { ButikId = -1, Butiksnamn = "Välj butik..." });
            Butiker.Insert(0, new Butiker { ButikId = 0, Butiksnamn = "Alla Butiker" });
            Butiker = new ObservableCollection<Butiker>(Butiker.OrderBy(b => b.ButikId));
            LagerSaldo = new ObservableCollection<LagerSaldo>();
            Books = new ObservableCollection<Böcker>(_context.Böcker.ToList());
            Books.Insert(0, new Böcker { FörlagId = -1, Titel = "Välj Bok..." });
            Books.Insert(1, new Böcker { FörlagId = 0, Titel = "Alla Böcker" });
            Authors = new ObservableCollection<Författare>(_context.Författare.ToList());
            Publishers = new ObservableCollection<Förlag>(_context.Förlag.ToList());

            AddBookCommand = new DelegateCommand(_ => AddBook());
            RemoveBookCommand = new DelegateCommand(_ => RemoveBook(), _ => SelectedBook?.FörlagId > 0);
            AddAuthorCommand = new DelegateCommand(_ => AddAuthor());
            UpdateSaldoCommand = new DelegateCommand(_ => UpdateSaldo());
            RemoveAuthorCommand = new DelegateCommand(_ => RemoveAuthor(), _ => SelectedAuthor != null);
            AddPublisherCommand = new DelegateCommand(_ => AddPublisher());
            RemovePublisherCommand = new DelegateCommand(_ => RemovePublisher(), _ => SelectedPublisher != null);
            UpdateBooksCommand = new DelegateCommand(_ => UpdateBooks(), _ => SelectedBook?.FörlagId > 0);
            UpdateAuthorsCommand = new DelegateCommand(_ => UpdateAuthors(), _ => SelectedAuthor != null);
            UpdatePublishersCommand = new DelegateCommand(_ => UpdatePublishers(), _ => SelectedPublisher != null);

            LoadBooks();
            LoadAuthors();
            LoadPublishers();

            SelectedButik = Butiker.FirstOrDefault(b => b.ButikId == -1);
            SelectedBook = Books.FirstOrDefault(b => b.FörlagId == -1);
        }



        public void LoadBooks()
        {
            Books = new ObservableCollection<Böcker>(
                _context.Böcker
                    .Include(b => b.Förlag)
                    .Include(b => b.FörfattareBöcker)
                        .ThenInclude(fb => fb.Författare)
                    .Where(b => b.FörlagId > -1)
                    .ToList()
            );
            Books.Insert(0, new Böcker { FörlagId = -1, Titel = "Välj Bok..." });
            SelectedBook = Books.FirstOrDefault(b => b.FörlagId == -1);
        }

        private void LoadAuthors()
        {
            if (SelectedButik?.ButikId == -1)
            {
                Authors?.Clear();
                return;
            }

            Authors = new ObservableCollection<Författare>(_context.Författare.ToList());
        }
        private void LoadLagerSaldo()
        {
            if (SelectedButik?.ButikId == -1)
            {
                LagerSaldo?.Clear();

                RaisePropertyChanged();
                return;
            }

            var previouslySelectedBook = SelectedBook;

            LagerSaldo?.Clear();
            var saldo = SelectedButik?.ButikId == 0
            ? _context.LagerSaldo
            .Include(ls => ls.IsbnNavigation)
            .ThenInclude(b => b.FörfattareBöcker)
            .ThenInclude(fb => fb.Författare)
            .ToList()
    :        _context.LagerSaldo
            .Where(ls => ls.ButikId == SelectedButik!.ButikId)
            .Include(ls => ls.IsbnNavigation)
            .ThenInclude(b => b.FörfattareBöcker)
            .ThenInclude(fb => fb.Författare)
            .ToList();

            foreach (var item in saldo)
            {
                LagerSaldo?.Add(item);
            }
            RaisePropertyChanged(nameof(LagerSaldo));
            if (previouslySelectedBook != null)
            {
                FilterLagerSaldo();
            }
            SelectedBook = Books?.FirstOrDefault(b => b.FörlagId == -1);
        }

        private void AddBook()
{
    var dialog = new AddBookWindow(_context);
    if (dialog.ShowDialog() == true)
    {
        var newBook = dialog.NewBook;

        if (newBook != null)
        {
            try
            {
                _context.Böcker.Add(newBook);
                _context.SaveChanges();

                MessageBox.Show("Boken har lagts till i sortimentet.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBooks();
                LoadLagerSaldo();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ett fel uppstod vid tillägg av boken: {ex.InnerException?.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
        private void RemoveBook()
        {
            if (SelectedBook == null)
            {
                MessageBox.Show("Välj en bok från listan att ta bort.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort boken: {SelectedBook.Titel} från databasen?", "Bekräfta", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                
                var relatedEntries = _context.FörfattareBöcker
                    .Where(fb => fb.Isbn == SelectedBook.Isbn13)
                    .ToList();

                if (relatedEntries.Any())
                {
                    _context.FörfattareBöcker.RemoveRange(relatedEntries);
                    _context.SaveChanges();
                }

                
                var lagerEntries = _context.LagerSaldo
                    .Where(ls => ls.Isbn == SelectedBook.Isbn13)
                    .ToList();

                if (lagerEntries.Any())
                {
                    _context.LagerSaldo.RemoveRange(lagerEntries);
                    _context.SaveChanges();
                }

                
                _context.Böcker.Remove(SelectedBook);
                _context.SaveChanges();

                MessageBox.Show("Boken har tagits bort från databasen.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBooks();
                LoadLagerSaldo();
            }
        }

        private void AddAuthor()
        {
            var dialog = new AddAuthorWindow();
            if (dialog.ShowDialog() == true)
            {
                var newAuthor = new Författare
                {
                    Förnamn = dialog.Förnamn,
                    Efternamn = dialog.Efternamn,
                    Födelsedatum = dialog.Födelsedatum.HasValue
                        ? DateOnly.FromDateTime(dialog.Födelsedatum.Value)
                        : throw new InvalidOperationException("Födelsedatum måste anges."),
                    Dödsdatum = dialog.Dödsdatum.HasValue
                        ? DateOnly.FromDateTime(dialog.Dödsdatum.Value)
                        : (DateOnly?)null
                };

                _context.Författare.Add(newAuthor);
                _context.SaveChanges();

                MessageBox.Show("Författaren har lagts till.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadAuthors();
            }
        }

        private void RemoveAuthor()
        {
            if (SelectedAuthor == null)
            {
                MessageBox.Show("Välj en författare att ta bort.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort författaren {SelectedAuthor.Förnamn} {SelectedAuthor.Efternamn}?", "Bekräfta", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _context.Författare.Remove(SelectedAuthor);
                _context.SaveChanges();

                MessageBox.Show("Författaren har tagits bort.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadAuthors();
            }
        }

        private void UpdateSaldo()
        {
           
            var dialog = new UpdateSaldoWindow(this);
            if (dialog.ShowDialog() == true)
            {
                var selectedStore = dialog.SelectedStore;
                var selectedBook = dialog.SelectedBook;
                var quantity = dialog.Quantity;

                if (selectedStore != null && selectedBook != null && quantity >= 0)
                {
                    var lagerSaldo = _context.LagerSaldo
                        .FirstOrDefault(ls => ls.ButikId == selectedStore.ButikId && ls.Isbn == selectedBook.Isbn13);

                    if (lagerSaldo != null)
                    {
                        lagerSaldo.Antal = quantity;
                    }
                    else
                    {
                        lagerSaldo = new LagerSaldo
                        {
                            ButikId = selectedStore.ButikId,
                            Isbn = selectedBook.Isbn13,
                            Antal = quantity
                        };
                        _context.LagerSaldo.Add(lagerSaldo);
                    }

                    _context.SaveChanges();
                    LoadLagerSaldo();
                }
            }
        }

        private void AddPublisher()
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

                MessageBox.Show("Förlaget har lagts till.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadPublishers();
            }
        }

        public void RemovePublisher()
        {

            if (SelectedPublisher == null)
            {
                MessageBox.Show("Välj ett förlag att ta bort.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort förlaget {SelectedPublisher.Namn}?", "Bekräfta", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _context.Förlag.Remove(SelectedPublisher);
                _context.SaveChanges();

                MessageBox.Show("Förlaget har tagits bort.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadPublishers();
            }
        }

        private void LoadPublishers()
        {
            if (SelectedButik?.ButikId == -1)
            {
                Publishers?.Clear();
                return;
            }

            Publishers = new ObservableCollection<Förlag>(_context.Förlag.ToList());
        }

        private void FilterLagerSaldo()
        {

            IQueryable<LagerSaldo> query = _context.LagerSaldo.Include(ls => ls.IsbnNavigation);

            if (SelectedBook?.FörlagId > 0)
            {
                query = query.Where(ls => ls.Isbn == SelectedBook.Isbn13);
            }

            if (SelectedButik?.ButikId > 0)
            {
                query = query.Where(ls => ls.ButikId == SelectedButik.ButikId);
            }

            var filteredLagerSaldo = query.ToList();
            LagerSaldo?.Clear();
            foreach (var item in filteredLagerSaldo)
            {
                LagerSaldo?.Add(item);
            }
            RaisePropertyChanged(nameof(LagerSaldo));
        }

        private void UpdateBooks()
        {
            try
            {
                if (SelectedBook != null && SelectedBook.Förlag != null)
                {
                    var newPublisher = new Förlag
                    {
                        Namn = SelectedBook.Förlag.Namn,
                        Adress = SelectedBook.Förlag.Adress,
                        PostNummer = SelectedBook.Förlag.PostNummer,
                        Ort = SelectedBook.Förlag.Ort,
                        TelefonNummer = SelectedBook.Förlag.TelefonNummer
                    };

                    _context.Förlag.Add(newPublisher);
                    _context.SaveChanges();

                    SelectedBook.FörlagId = newPublisher.FörlagId;
                }

                _context.SaveChanges();
                LoadBooks();
                LoadAuthors();
                MessageBox.Show("Bokändringarna har sparats.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateAuthors()
        {
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Författarändringarna har sparats.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePublishers()
        {
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Förlagsändringarna har sparats.", "Framgång", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}