using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ALOBOUTIQUE.Models;

namespace ALOBOUTIQUE.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        // ══════════════════════════════════════════
        // DATOS DE PRUEBA
        // ══════════════════════════════════════════
        private ObservableCollection<ProductItem> _allProducts = new()
        {
            new ProductItem { Id=1,  Name="Vestido Floral Primavera",  Category="Vestidos",   Price=1290, Stock=8,  Sizes="XS · S · M · L",    Badge="Nuevo"    },
            new ProductItem { Id=2,  Name="Bolso Structured Rosé",     Category="Accesorios", Price=2150, Stock=4,  Sizes="Único",             Badge=""         },
            new ProductItem { Id=3,  Name="Mules Satinados Nude",      Category="Zapatos",    Price=980,  Stock=2,  Sizes="35 · 36 · 37 · 38", Badge="Últimas"  },
            new ProductItem { Id=4,  Name="Set Lino Verano",           Category="Sets",       Price=1850, Stock=6,  Sizes="S · M · L",         Badge=""         },
            new ProductItem { Id=5,  Name="Collar Perla Maui",         Category="Accesorios", Price=650,  Stock=10, Sizes="Único",             Badge="Sale −20%"},
            new ProductItem { Id=6,  Name="Mini Vestido Broderie",     Category="Vestidos",   Price=1490, Stock=5,  Sizes="XS · S · M",        Badge=""         },
            new ProductItem { Id=7,  Name="Blusa Off-shoulder Rosa",   Category="Blusas",     Price=890,  Stock=7,  Sizes="XS · S · M · L",    Badge="Nuevo"    },
            new ProductItem { Id=8,  Name="Cinturón Trenzado Camel",   Category="Accesorios", Price=420,  Stock=12, Sizes="Único",             Badge=""         },
            new ProductItem { Id=9,  Name="Vestido Satinado Noche",    Category="Vestidos",   Price=2200, Stock=1,  Sizes="XS · S · M",        Badge=""         },
            new ProductItem { Id=10, Name="Sandalias Strappy Gold",    Category="Zapatos",    Price=750,  Stock=0,  Sizes="35 · 36 · 37",      Badge=""         },
            new ProductItem { Id=11, Name="Blusa Floral Crop",         Category="Blusas",     Price=590,  Stock=3,  Sizes="XS · S · M · L",    Badge=""         },
            new ProductItem { Id=12, Name="Aretes Perla Dorada",       Category="Accesorios", Price=280,  Stock=15, Sizes="Único",             Badge=""         },
        };

        // ══════════════════════════════════════════
        // PROPIEDADES LISTA / FILTROS
        // ══════════════════════════════════════════
        private string _searchText = string.Empty;
        private string _activeFilter = "Todos";
        private string _activeStockFilter = "Todos";
        private ProductItem? _selectedProduct;
        private bool _isEditing = false;

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public ObservableCollection<ProductItem> FilteredProducts { get; set; } = new();

        public string ProductCountLabel => $"{FilteredProducts.Count} productos";

        public ProductItem? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                IsEditing = false;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedProduct));
                OnPropertyChanged(nameof(EditName));
                OnPropertyChanged(nameof(EditCategory));
                OnPropertyChanged(nameof(EditPrice));
                OnPropertyChanged(nameof(EditStock));
                OnPropertyChanged(nameof(EditSizes));
                OnPropertyChanged(nameof(StockStatusLabel));
                OnPropertyChanged(nameof(StockStatusColor));
            }
        }

        public bool HasSelectedProduct => _selectedProduct != null;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotEditing)); }
        }
        public bool IsNotEditing => !_isEditing;

        // Campos editables (bind bidireccional)
        public string EditName
        {
            get => _selectedProduct?.Name ?? string.Empty;
            set { if (_selectedProduct != null) { _selectedProduct.Name = value; OnPropertyChanged(); } }
        }
        public string EditCategory
        {
            get => _selectedProduct?.Category ?? string.Empty;
            set { if (_selectedProduct != null) { _selectedProduct.Category = value; OnPropertyChanged(); } }
        }
        public string EditPrice
        {
            get => _selectedProduct?.Price.ToString() ?? "0";
            set { if (_selectedProduct != null && decimal.TryParse(value, out var p)) { _selectedProduct.Price = p; OnPropertyChanged(); } }
        }
        public string EditStock
        {
            get => _selectedProduct?.Stock.ToString() ?? "0";
            set { if (_selectedProduct != null && int.TryParse(value, out var s)) { _selectedProduct.Stock = s; OnPropertyChanged(); } }
        }
        public string EditSizes
        {
            get => _selectedProduct?.Sizes ?? string.Empty;
            set { if (_selectedProduct != null) { _selectedProduct.Sizes = value; OnPropertyChanged(); } }
        }

        public string StockStatusLabel => _selectedProduct?.Stock switch
        {
            0 => "Sin stock",
            <= 3 => "Stock bajo",
            <= 6 => "Stock medio",
            _ => "En stock"
        };

        public string StockStatusColor => _selectedProduct?.Stock switch
        {
            0 => "#C0305A",
            <= 3 => "#8A5A00",
            <= 6 => "#E87AAF",
            _ => "#2D7A4F"
        };

        // ══════════════════════════════════════════
        // ESTADÍSTICAS
        // ══════════════════════════════════════════
        public string TotalProducts => _allProducts.Count.ToString();
        public string TotalStock => _allProducts.Sum(p => p.Stock).ToString();
        public string LowStockCount => _allProducts.Count(p => p.Stock > 0 && p.Stock <= 3).ToString();
        public string OutOfStockCount => _allProducts.Count(p => p.Stock == 0).ToString();
        public string TotalValueFormatted => $"${_allProducts.Sum(p => p.Price * p.Stock):N0}";

        public bool HasLowStockAlert => _allProducts.Any(p => p.Stock <= 3);
        public string LowStockAlertLabel => $"{_allProducts.Count(p => p.Stock <= 3)} producto(s) con stock bajo o agotado";

        // ══════════════════════════════════════════
        // NUEVO PRODUCTO
        // ══════════════════════════════════════════
        private bool _isAddingNew = false;
        public bool IsAddingNew
        {
            get => _isAddingNew;
            set { _isAddingNew = value; OnPropertyChanged(); }
        }

        public string NewName { get; set; } = string.Empty;
        public string NewCategory { get; set; } = "Vestidos";
        public string NewPrice { get; set; } = string.Empty;
        public string NewStock { get; set; } = string.Empty;
        public string NewSizes { get; set; } = string.Empty;

        // ══════════════════════════════════════════
        // COMANDOS
        // ══════════════════════════════════════════
        public ICommand FilterCommand { get; }
        public ICommand StockFilterCommand { get; }
        public ICommand SelectProductCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand NewProductCommand { get; }
        public ICommand SaveNewCommand { get; }
        public ICommand CancelNewCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        // ══════════════════════════════════════════
        // CONSTRUCTOR
        // ══════════════════════════════════════════
        public InventoryViewModel()
        {
            foreach (var p in _allProducts)
                FilteredProducts.Add(p);

            FilterCommand = new Command<string>(filter =>
            {
                _activeFilter = filter;
                ApplyFilter();
            });

            StockFilterCommand = new Command<string>(filter =>
            {
                _activeStockFilter = filter;
                ApplyFilter();
            });

            SelectProductCommand = new Command<ProductItem>(p =>
            {
                SelectedProduct = p;
                IsAddingNew = false;
            });

            EditCommand = new Command(() => IsEditing = true);

            SaveEditCommand = new Command(() =>
            {
                IsEditing = false;
                ApplyFilter();
                RefreshStats();
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(StockStatusLabel));
                OnPropertyChanged(nameof(StockStatusColor));
            });

            CancelEditCommand = new Command(() => IsEditing = false);

            DeleteCommand = new Command(() =>
            {
                if (_selectedProduct != null)
                {
                    _allProducts.Remove(_selectedProduct);
                    SelectedProduct = null;
                    ApplyFilter();
                    RefreshStats();
                }
            });

            NewProductCommand = new Command(() =>
            {
                SelectedProduct = null;
                IsAddingNew = true;
                NewName = string.Empty;
                NewCategory = "Vestidos";
                NewPrice = string.Empty;
                NewStock = string.Empty;
                NewSizes = string.Empty;
                OnPropertyChanged(nameof(NewName));
                OnPropertyChanged(nameof(NewCategory));
                OnPropertyChanged(nameof(NewPrice));
                OnPropertyChanged(nameof(NewStock));
                OnPropertyChanged(nameof(NewSizes));
            });

            SaveNewCommand = new Command(() =>
            {
                if (string.IsNullOrWhiteSpace(NewName)) return;
                var newProduct = new ProductItem
                {
                    Id = _allProducts.Count + 1,
                    Name = NewName,
                    Category = NewCategory,
                    Price = decimal.TryParse(NewPrice, out var p) ? p : 0,
                    Stock = int.TryParse(NewStock, out var s) ? s : 0,
                    Sizes = NewSizes,
                    IconBg = "#FDF0F5"
                };
                _allProducts.Add(newProduct);
                IsAddingNew = false;
                ApplyFilter();
                RefreshStats();
                SelectedProduct = newProduct;
            });

            CancelNewCommand = new Command(() => IsAddingNew = false);

            ClearSelectionCommand = new Command(() =>
            {
                SelectedProduct = null;
                IsAddingNew = false;
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

            if (_activeStockFilter == "Bajo")
                query = query.Where(p => p.Stock > 0 && p.Stock <= 3);
            else if (_activeStockFilter == "Agotado")
                query = query.Where(p => p.Stock == 0);

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(p => p.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var p in query)
                FilteredProducts.Add(p);

            OnPropertyChanged(nameof(ProductCountLabel));
        }

        private void RefreshStats()
        {
            OnPropertyChanged(nameof(TotalProducts));
            OnPropertyChanged(nameof(TotalStock));
            OnPropertyChanged(nameof(LowStockCount));
            OnPropertyChanged(nameof(OutOfStockCount));
            OnPropertyChanged(nameof(TotalValueFormatted));
            OnPropertyChanged(nameof(HasLowStockAlert));
            OnPropertyChanged(nameof(LowStockAlertLabel));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}