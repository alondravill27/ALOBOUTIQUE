using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ALOBOUTIQUE.Models;

namespace ALOBOUTIQUE.ViewModels
{
    public class PosViewModel : INotifyPropertyChanged
    {
        // ══════════════════════════════════════════
        // PROPIEDADES CAJERA / TURNO
        // ══════════════════════════════════════════
        public string CashierName { get; set; } = "Lupita Pérez";
        public string CashierInitials { get; set; } = "LP";
        public string CashierRole { get; set; } = "Cajera · Turno mañana";
        public string ShiftLabel { get; set; } = "Turno: 09:00 – 18:00 · Lupita P.";
        public string StatusConnection { get; set; } = "Conectado";
        public string CashDrawerLabel { get; set; } = "Caja abierta · $5,000";

        // ══════════════════════════════════════════
        // STATS DEL DÍA
        // ══════════════════════════════════════════
        private decimal _todaySales = 14820m;
        private int _todayTransactions = 23;

        public string TodaySalesFormatted => $"${_todaySales:N0}";
        public string TodayTransactions => _todayTransactions.ToString();

        // ══════════════════════════════════════════
        // CLIENTE ACTIVO
        // ══════════════════════════════════════════
        public string CustomerName { get; set; } = "Daniela Morales";
        public string CustomerTier { get; set; } = "VIP Diamante";
        public string CustomerPoints { get; set; } = "⭐ 2,340 pts";
        private decimal CustomerDiscountRate => CustomerTier == "VIP Diamante" ? 0.10m : 0m;

        // ══════════════════════════════════════════
        // ORDEN
        // ══════════════════════════════════════════
        private string _saleMode = "Venta";
        private int _orderNumber = 124;

        public string OrderTitle => $"{_saleMode} #{_orderNumber:D4}";
        public string OrderMeta => $"Hoy · {DateTime.Now:HH:mm} · {CartItems.Count} producto(s)";

        // ══════════════════════════════════════════
        // CATÁLOGO
        // ══════════════════════════════════════════
        private string _searchText = string.Empty;
        private string _activeFilter = "Todos";
        private ProductItem? _selectedProduct;

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public string ProductCountLabel => $"{FilteredProducts.Count} productos disponibles";

        private ObservableCollection<ProductItem> _allProducts = new()
{
           new ProductItem { Id=1, Name="Vestido Floral Primavera", Category="Vestidos",   Price=1290, Icon="\ue3d6", IconBg="#FDF0F5", Sizes="XS · S · M · L",    Badge="Nuevo",    Stock=8 },
           new ProductItem { Id=2, Name="Bolso Structured Rosé",    Category="Accesorios", Price=2150, Icon="\uf1a0", IconBg="#FFF5F8", Sizes="Único",             Badge="",         Stock=4 },
           new ProductItem { Id=3, Name="Mules Satinados Nude",     Category="Zapatos",    Price=980,  Icon="\ue8d4", IconBg="#FDF0F5", Sizes="35 · 36 · 37 · 38", Badge="Últimas",  Stock=3 },
           new ProductItem { Id=4, Name="Set Lino Verano",          Category="Sets",       Price=1850, Icon="\ue3d6", IconBg="#FFF5F8", Sizes="S · M · L",         Badge="",         Stock=6 },
           new ProductItem { Id=5, Name="Collar Perla Maui",        Category="Accesorios", Price=650,  Icon="\ue3ab", IconBg="#FDF0F5", Sizes="Único",             Badge="Sale −20%",Stock=10 },
           new ProductItem { Id=6, Name="Mini Vestido Broderie",    Category="Vestidos",   Price=1490, Icon="\ue3d6", IconBg="#FFF5F8", Sizes="XS · S · M",        Badge="",         Stock=5 },
           new ProductItem { Id=7, Name="Blusa Off-shoulder Rosa",  Category="Blusas",     Price=890,  Icon="\ue3d4", IconBg="#FDF0F5", Sizes="XS · S · M · L",    Badge="Nuevo",    Stock=7 },
           new ProductItem { Id=8, Name="Cinturón Trenzado Camel",  Category="Accesorios", Price=420,  Icon="\ue3ab", IconBg="#FFF5F8", Sizes="Único",             Badge="",         Stock=12 },
};

        public ObservableCollection<ProductItem> FilteredProducts { get; set; } = new();

        public ProductItem? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                if (value != null) AddProductToCart(value);
            }
        }

        // ══════════════════════════════════════════
        // CARRITO
        // ══════════════════════════════════════════
        public ObservableCollection<CartItem> CartItems { get; set; } = new();

        private decimal Subtotal => CartItems.Sum(i => i.Total);
        private decimal DiscountAmount => Subtotal * CustomerDiscountRate;
        private decimal Total => Subtotal - DiscountAmount;

        public string SubtotalFormatted => $"${Subtotal:N0}";
        public string DiscountLabel => $"Descuento {CustomerTier} −{CustomerDiscountRate * 100:0}%";
        public string DiscountFormatted => $"−${DiscountAmount:N0}";
        public string TotalFormatted => $"${Total:N0}";
        public bool HasDiscount => CustomerDiscountRate > 0 && CartItems.Count > 0;

        // Puntos Peka Club (1 pto por cada $10)
        private int PointsToEarn => (int)(Total / 10);
        public bool HasPointsToEarn => PointsToEarn > 0;
        public string PointsEarnedMessage => $"{CustomerName} ganará +{PointsToEarn} puntos con esta compra";

        // ══════════════════════════════════════════
        // PAGO
        // ══════════════════════════════════════════
        private string _paymentMethod = "Tarjeta";

        public string PaymentMethod
        {
            get => _paymentMethod;
            set { _paymentMethod = value; OnPropertyChanged(); OnPropertyChanged(nameof(PaymentBgCard)); OnPropertyChanged(nameof(PaymentBorderCard)); OnPropertyChanged(nameof(ChargeButtonLabel)); }
        }

        public string PaymentBgCard => _paymentMethod == "Tarjeta" ? "#FDF0F5" : "#FFFFFF";
        public string PaymentBorderCard => _paymentMethod == "Tarjeta" ? "#E87AAF" : "#F0D8E6";
        public string ChargeButtonLabel => CartItems.Count > 0
            ? $"Cobrar {TotalFormatted} · {_paymentMethod}"
            : "Cobrar";
        public bool CanCharge => CartItems.Count > 0;

        // ══════════════════════════════════════════
        // COMANDOS
        // ══════════════════════════════════════════
        public ICommand FilterCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand IncreaseQtyCommand { get; }
        public ICommand DecreaseQtyCommand { get; }
        public ICommand ClearCartCommand { get; }
        public ICommand SetPaymentMethodCommand { get; }
        public ICommand SetModeCommand { get; }
        public ICommand ChargeCommand { get; }
        public ICommand ConvertToLayawayCommand { get; }
        public ICommand NewProductCommand { get; }

        // ══════════════════════════════════════════
        // CONSTRUCTOR
        // ══════════════════════════════════════════
        public PosViewModel()
        {
            // Cargar todos los productos al inicio
            foreach (var p in _allProducts)
                FilteredProducts.Add(p);

            FilterCommand = new Command<string>(filter =>
            {
                _activeFilter = filter;
                ApplyFilter();
            });

            AddToCartCommand = new Command<ProductItem>(product =>
            {
                if (product != null) AddProductToCart(product);
            });

            RemoveFromCartCommand = new Command<CartItem>(item =>
            {
                if (item != null && CartItems.Contains(item))
                {
                    CartItems.Remove(item);
                    RefreshTotals();
                }
            });

            IncreaseQtyCommand = new Command<CartItem>(item =>
            {
                if (item != null) { item.Quantity++; RefreshTotals(); }
            });

            DecreaseQtyCommand = new Command<CartItem>(item =>
            {
                if (item == null) return;
                if (item.Quantity > 1) { item.Quantity--; RefreshTotals(); }
                else { CartItems.Remove(item); RefreshTotals(); }
            });

            ClearCartCommand = new Command(() =>
            {
                CartItems.Clear();
                RefreshTotals();
            });

            SetPaymentMethodCommand = new Command<string>(method =>
            {
                if (!string.IsNullOrEmpty(method)) PaymentMethod = method;
            });

            SetModeCommand = new Command<string>(mode =>
            {
                if (!string.IsNullOrEmpty(mode))
                {
                    _saleMode = mode;
                    OnPropertyChanged(nameof(OrderTitle));
                }
            });

            ChargeCommand = new Command(
                execute: () =>
                {
                    // TODO: Implementar lógica de cobro real
                    // Por ahora limpiamos el carrito como simulación
                    _todaySales += Total;
                    _todayTransactions++;
                    CartItems.Clear();
                    _orderNumber++;
                    RefreshTotals();
                    OnPropertyChanged(nameof(TodaySalesFormatted));
                    OnPropertyChanged(nameof(TodayTransactions));
                    OnPropertyChanged(nameof(OrderTitle));
                },
                canExecute: () => CanCharge
            );

            ConvertToLayawayCommand = new Command(() =>
            {
                // TODO: Implementar apartado
                _saleMode = "Apartado";
                OnPropertyChanged(nameof(OrderTitle));
            });

            NewProductCommand = new Command(() =>
            {
                // TODO: Navegar a pantalla de nuevo producto
            });
        }

        // ══════════════════════════════════════════
        // MÉTODOS PRIVADOS
        // ══════════════════════════════════════════
        private void ApplyFilter()
        {
            FilteredProducts.Clear();
            var query = _allProducts.AsEnumerable();

            if (_activeFilter != "Todos")
                query = query.Where(p => p.Category == _activeFilter);

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(p => p.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var p in query)
                FilteredProducts.Add(p);

            OnPropertyChanged(nameof(ProductCountLabel));
        }

        private void AddProductToCart(ProductItem product)
        {
            var existing = CartItems.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Variant = product.Sizes.Split('·')[0].Trim(),
                    Icon = product.Icon,
                    UnitPrice = product.Price
                });
            }
            _selectedProduct = null;
            OnPropertyChanged(nameof(SelectedProduct));
            RefreshTotals();
        }

        private void RefreshTotals()
        {
            OnPropertyChanged(nameof(SubtotalFormatted));
            OnPropertyChanged(nameof(DiscountFormatted));
            OnPropertyChanged(nameof(DiscountLabel));
            OnPropertyChanged(nameof(TotalFormatted));
            OnPropertyChanged(nameof(HasDiscount));
            OnPropertyChanged(nameof(HasPointsToEarn));
            OnPropertyChanged(nameof(PointsEarnedMessage));
            OnPropertyChanged(nameof(ChargeButtonLabel));
            OnPropertyChanged(nameof(CanCharge));
            OnPropertyChanged(nameof(OrderMeta));
            ((Command)ChargeCommand).ChangeCanExecute();
        }

        // ══════════════════════════════════════════
        // INOTIFYPROPERTYCHANGED
        // ══════════════════════════════════════════
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}