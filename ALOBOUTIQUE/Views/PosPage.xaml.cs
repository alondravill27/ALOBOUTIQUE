using ALOBOUTIQUE.ViewModels;

namespace ALOBOUTIQUE.Views;

public partial class PosPage : ContentPage
{
    public PosPage(PosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnClientesNavTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CustomersPage));
    }

    private async void OnPosNavTapped(object sender, EventArgs e)
    {
        await DisplayAlert("AloBoutique", "Ya te encuentras en el Punto de Venta activo.", "OK");
    }

    private async void OnHistorialNavTapped(object sender, EventArgs e)
    {
        await DisplayAlert("AloBoutique", "Módulo de Historial de Ventas - Próximamente disponible.", "OK");
    }

    private async void OnApartadosNavTapped(object sender, EventArgs e)
    {
        await DisplayAlert("AloBoutique", "Módulo de Sistema de Apartados - Próximamente disponible.", "OK");
    }

    private async void OnInventarioNavTapped(object sender, EventArgs e)
    {
        await DisplayAlert("AloBoutique", "Módulo de Inventario y Almacén - Próximamente disponible.", "OK");
    }

    private async void OnPekaClubNavTapped(object sender, EventArgs e)
    {
        await DisplayAlert("AloBoutique", "Módulo de Peka Club - Próximamente disponible.", "OK");
    }
}