using Examen3PM2_AC0036.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
                // Ordena la lista por fecha antes de asignarla a la propiedad ListaNotas
                ListaNotas = new ObservableCollection<Notas>(ListaNotas.OrderBy(n => n.fecha));
                OnPropertyChanged(nameof(ListaNotas));
            });
    }

    private async void btneliminarsitio_Clicked(object sender, EventArgs e)
    {
        // Verifica si se ha seleccionado una nota para eliminar
        if (NotasCollection.SelectedItem != null)
        {
            // Obtiene la nota seleccionada
            Notas notaEliminar = NotasCollection.SelectedItem as Notas;

            // Elimina la nota de la colección y de la base de datos
            ListaNotas.Remove(notaEliminar);
            await client.Child("Notas").Child(notaEliminar.id_nota).DeleteAsync();

            // Asigna nuevamente la colección para forzar la actualización de la interfaz de usuario
            ListaNotas = new ObservableCollection<Notas>(ListaNotas);

            // Notifica a la interfaz de usuario sobre el cambio en la propiedad
            OnPropertyChanged(nameof(ListaNotas));

            // Deselecciona la nota en el CollectionView
            NotasCollection.SelectedItem = null;
        }
        else
        {
            // Muestra un mensaje indicando que no se ha seleccionado ninguna nota
            await DisplayAlert("Alerta", "Seleccione una nota para eliminar", "OK");
        }
    }
}