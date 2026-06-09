using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ALOBOUTIQUE.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity = 1;

        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Variant { get; set; } = string.Empty;
        public string Icon { get; set; } = "\ue53c";
        public decimal UnitPrice { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalFormatted));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public decimal Total => UnitPrice * Quantity;
        public string TotalFormatted => $"${Total:N0}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}