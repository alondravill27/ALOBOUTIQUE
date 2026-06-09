namespace ALOBOUTIQUE.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Points { get; set; }
        public DateTime MemberSince { get; set; } = DateTime.Now;

        public string Initials => Name.Length >= 2
            ? $"{Name.Split(' ')[0][0]}{(Name.Split(' ').Length > 1 ? Name.Split(' ')[1][0] : Name[1])}"
            : Name[0].ToString();

        public string Tier => Points switch
        {
            >= 10000 => "Diamante",
            >= 5000 => "Oro",
            >= 1000 => "Plata",
            _ => "Estándar"
        };

        public string TierIcon => Tier switch
        {
            "Diamante" => "💎",
            "Oro" => "🥇",
            "Plata" => "🥈",
            _ => "⭐"
        };

        public decimal DiscountRate => Tier switch
        {
            "Diamante" => 0.10m,
            "Oro" => 0.08m,
            "Plata" => 0.05m,
            _ => 0m
        };

        public string DiscountLabel => DiscountRate > 0
            ? $"{DiscountRate * 100:0}% descuento"
            : "Sin descuento";

        public string PointsFormatted => $"{Points:N0} pts";
        public string MemberSinceFormatted => $"Desde {MemberSince:MMMM yyyy}";
    }

    public class PurchaseHistory
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int PointsEarned { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public List<string> Items { get; set; } = new();

        public string DateFormatted => Date.ToString("dd MMM yyyy · HH:mm");
        public string TotalFormatted => $"${Total:N0}";
        public string PointsFormatted => $"+{PointsEarned} pts";
        public string ItemsSummary => Items.Count > 0 ? string.Join(", ", Items) : "Sin detalle";
    }
}