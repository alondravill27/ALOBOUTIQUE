using System.ComponentModel;

namespace ALOBOUTIQUE.Models
{
    public class ProductItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Icon { get; set; } = "\ue53c";
        public string IconBg { get; set; } = "#FDF0F5";
        public string Sizes { get; set; } = string.Empty;
        public string Badge { get; set; } = string.Empty;
        public bool HasBadge => !string.IsNullOrEmpty(Badge);
        public int Stock { get; set; }

        public string PriceFormatted => $"${Price:N0}";

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}