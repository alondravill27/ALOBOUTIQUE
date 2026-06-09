namespace ALOBOUTIQUE.Views;

public partial class PosPage : ContentPage
{
	public PosPage()
	{
		InitializeComponent();

	}

    private async void OnClientesNavTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//CustomersPage");
    }
}
////