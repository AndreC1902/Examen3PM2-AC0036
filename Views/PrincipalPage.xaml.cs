namespace Examen3PM2_AC0036.Views;

public partial class PrincipalPage : ContentPage
{
	public PrincipalPage()
	{
		InitializeComponent();
	}

    private async void OnAgregarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AgregarNotaPage));
    }

    private async void OnListarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ListarNotasPage));
    }
}