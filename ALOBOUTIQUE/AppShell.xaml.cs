namespace ALOBOUTIQUE
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PosPage), typeof(PosPage));
            Routing.RegisterRoute(nameof(CustomersPage), typeof(CustomersPage));
            // Routing.RegisterRoute("InventoryPage", typeof(InventoryPage));
        }
    }
}
