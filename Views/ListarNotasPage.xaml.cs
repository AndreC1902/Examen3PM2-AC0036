using Examen3PM2_AC0036.Models;
using Firebase.Database;
using System.Collections.ObjectModel;

namespace Examen3PM2_AC0036.Views;

public partial class ListarNotasPage : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://examenpm2ac-default-rtdb.firebaseio.com/");
    public ObservableCollection<Notas> ListaNotas { get; set; } = new ObservableCollection<Notas>();

    public ListarNotasPage()
	{
		InitializeComponent();
        CargarNotas();
        BindingContext = this;
    }

    public void CargarNotas()
    {
        ListaNotas.Clear();

        client.Child("Notas")
            .AsObservable<Notas>()
            .Subscribe(Notas =>
            {
                if (Notas != null && Notas.Object != null)
                {
                    ListaNotas.Add(Notas.Object);
                }
            });
    }

    private async void NotasCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Notas nota = e.CurrentSelection.FirstOrDefault() as Notas;
        var parametro = new Dictionary<string, object>
        {
            ["Detalle"] = nota
        };
        //await Shell.Current.GoToAsync(nameof(DetallesNotaPage), parametro);
    }
}