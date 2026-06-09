using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ALOBOUTIQUE.Models;

namespace ALOBOUTIQUE.ViewModels
{
    public class CustomersViewModel : INotifyPropertyChanged
    {
        // ══════════════════════════════════════════
        // DATOS DE PRUEBA
        // ══════════════════════════════════════════
        private readonly List<Customer> _allCustomers = new()
        {
            new Customer { Id=1, Name="Daniela Morales",   Phone="667-123-4567", Email="daniela@email.com",  Points=12340, MemberSince=new DateTime(2022,3,15) },
            new Customer { Id=2, Name="Sofía Ramírez",     Phone="667-234-5678", Email="sofia@email.com",    Points=6820,  MemberSince=new DateTime(2022,8,20) },
            new Customer { Id=3, Name="Valentina Cruz",    Phone="667-345-6789", Email="vale@email.com",     Points=3150,  MemberSince=new DateTime(2023,1,10) },
            new Customer { Id=4, Name="Isabella Torres",   Phone="667-456-7890", Email="isa@email.com",      Points=1480,  MemberSince=new DateTime(2023,5,5)  },
            new Customer { Id=5, Name="Camila Flores",     Phone="667-567-8901", Email="camila@email.com",   Points=780,   MemberSince=new DateTime(2023,9,18) },
            new Customer { Id=6, Name="Mariana López",     Phone="667-678-9012", Email="mariana@email.com",  Points=320,   MemberSince=new DateTime(2024,2,28) },
            new Customer { Id=7, Name="Fernanda Guzmán",   Phone="667-789-0123", Email="fer@email.com",      Points=5200,  MemberSince=new DateTime(2022,11,3) },
            new Customer { Id=8, Name="Lucía Mendoza",     Phone="667-890-1234", Email="lucia@email.com",    Points=9100,  MemberSince=new DateTime(2021,7,22) },
        };

        private readonly Dictionary<int, List<PurchaseHistory>> _allHistory = new()
        {
            { 1, new List<PurchaseHistory> {
                new() { Id=1, CustomerId=1, Date=DateTime.Now.AddDays(-2),  Total=3564, PointsEarned=356, PaymentMethod="Tarjeta", Items=new(){"Vestido Floral","Bolso Rosé","Collar Perla"} },
                new() { Id=2, CustomerId=1, Date=DateTime.Now.AddDays(-15), Total=1290, PointsEarned=129, PaymentMethod="Efectivo", Items=new(){"Vestido Floral Primavera"} },
                new() { Id=3, CustomerId=1, Date=DateTime.Now.AddDays(-30), Total=2150, PointsEarned=215, PaymentMethod="CoDi",    Items=new(){"Bolso Structured Rosé"} },
            }},
            { 2, new List<PurchaseHistory> {
                new() { Id=4, CustomerId=2, Date=DateTime.Now.AddDays(-5),  Total=1850, PointsEarned=185, PaymentMethod="Tarjeta", Items=new(){"Set Lino Verano"} },
                new() { Id=5, CustomerId=2, Date=DateTime.Now.AddDays(-20), Total=890,  PointsEarned=89,  PaymentMethod="Efectivo", Items=new(){"Blusa Off-shoulder Rosa"} },
            }},
            { 3, new List<PurchaseHistory> {
                new() { Id=6, CustomerId=3, Date=DateTime.Now.AddDays(-8),  Total=980,  PointsEarned=98,  PaymentMethod="PayPal",  Items=new(){"Mules Satinados Nude"} },
            }},
        };

        // ══════════════════════════════════════════
        // PROPIEDADES LISTA
        // ══════════════════════════════════════════
        private string _searchText = string.Empty;
        private string _activeFilter = "Todos";
        private Customer? _selectedCustomer;

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public string CustomerCountLabel => $"{FilteredCustomers.Count} clientes registradas";

        public ObservableCollection<Customer> FilteredCustomers { get; set; } = new();

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedCustomer));
                OnPropertyChanged(nameof(SelectedCustomerHistory));
                OnPropertyChanged(nameof(SelectedTotalSpent));
                OnPropertyChanged(nameof(SelectedTotalSpentFormatted));
                OnPropertyChanged(nameof(SelectedVisits));
                OnPropertyChanged(nameof(PointsToNextTier));
                OnPropertyChanged(nameof(PointsToNextTierLabel));
                OnPropertyChanged(nameof(TierProgressPercent));
            }
        }

        public bool HasSelectedCustomer => _selectedCustomer != null;

        // ══════════════════════════════════════════
        // DETALLE CLIENTE SELECCIONADA
        // ══════════════════════════════════════════
        public List<PurchaseHistory> SelectedCustomerHistory =>
            _selectedCustomer != null && _allHistory.TryGetValue(_selectedCustomer.Id, out var h)
                ? h.OrderByDescending(x => x.Date).ToList()
                : new List<PurchaseHistory>();

        public decimal SelectedTotalSpent =>
            SelectedCustomerHistory.Sum(h => h.Total);

        public string SelectedTotalSpentFormatted => $"${SelectedTotalSpent:N0}";

        public int SelectedVisits => SelectedCustomerHistory.Count;

        public int PointsToNextTier => _selectedCustomer?.Tier switch
        {
            "Estándar" => 1000 - (_selectedCustomer?.Points ?? 0),
            "Plata" => 5000 - (_selectedCustomer?.Points ?? 0),
            "Oro" => 10000 - (_selectedCustomer?.Points ?? 0),
            _ => 0
        };

        public string PointsToNextTierLabel => _selectedCustomer?.Tier switch
        {
            "Diamante" => "¡Nivel máximo alcanzado! 💎",
            _ => $"Faltan {PointsToNextTier:N0} pts para {NextTierName}"
        };

        private string NextTierName => _selectedCustomer?.Tier switch
        {
            "Estándar" => "Plata",
            "Plata" => "Oro",
            "Oro" => "Diamante",
            _ => ""
        };

        public double TierProgressPercent => _selectedCustomer?.Tier switch
        {
            "Estándar" => Math.Min((_selectedCustomer.Points / 1000.0) * 100, 100),
            "Plata" => Math.Min(((_selectedCustomer.Points - 1000) / 4000.0) * 100, 100),
            "Oro" => Math.Min(((_selectedCustomer.Points - 5000) / 5000.0) * 100, 100),
            "Diamante" => 100,
            _ => 0
        };

        // ══════════════════════════════════════════
        // STATS GENERALES
        // ══════════════════════════════════════════
        public string TotalCustomers => _allCustomers.Count.ToString();
        public string DiamondCount => _allCustomers.Count(c => c.Tier == "Diamante").ToString();
        public string GoldCount => _allCustomers.Count(c => c.Tier == "Oro").ToString();
        public string SilverCount => _allCustomers.Count(c => c.Tier == "Plata").ToString();

        // ══════════════════════════════════════════
        // COMANDOS
        // ══════════════════════════════════════════
        public ICommand FilterCommand { get; }
        public ICommand SelectCustomerCommand { get; }
        public ICommand NewCustomerCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        // ══════════════════════════════════════════
        // CONSTRUCTOR
        // ══════════════════════════════════════════
        public CustomersViewModel()
        {
            foreach (var c in _allCustomers)
                FilteredCustomers.Add(c);

            FilterCommand = new Command<string>(filter =>
            {
                _activeFilter = filter;
                ApplyFilter();
            });

            SelectCustomerCommand = new Command<Customer>(customer =>
            {
                SelectedCustomer = customer;
            });

            NewCustomerCommand = new Command(() =>
            {
                // TODO: Navegar a pantalla de nueva cliente
            });

            ClearSelectionCommand = new Command(() =>
            {
                SelectedCustomer = null;
            });
        }

        // ══════════════════════════════════════════
        // MÉTODOS PRIVADOS
        // ══════════════════════════════════════════
        private void ApplyFilter()
        {
            FilteredCustomers.Clear();
            var query = _allCustomers.AsEnumerable();

            if (_activeFilter != "Todos")
                query = query.Where(c => c.Tier == _activeFilter);

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(c =>
                    c.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(_searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var c in query)
                FilteredCustomers.Add(c);

            OnPropertyChanged(nameof(CustomerCountLabel));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
