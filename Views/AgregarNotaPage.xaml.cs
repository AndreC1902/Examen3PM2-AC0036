using Examen3PM2_AC0036.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Plugin.Maui.Audio;

namespace Examen3PM2_AC0036.Views;

public partial class AgregarNotaPage : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://examenpm2ac-default-rtdb.firebaseio.com/");
    FirebaseStorage storage = new FirebaseStorage("examenpm2ac.appspot.com");

    private string urlFoto { get; set; }
    private string urlAudio { get; set; }
    readonly IAudioManager _audioManager;
    readonly IAudioRecorder _audioRecorder;

    public AgregarNotaPage(IAudioManager audioManager)
	{
		InitializeComponent();
        BindingContext = this;
        _audioManager = audioManager;
        _audioRecorder = audioManager.CreateRecorder();
    }

    private async void OnTomarFotoClicked(object sender, EventArgs e)
    {
        var foto = await MediaPicker.CapturePhotoAsync();
        if (foto != null)
        {
            var stream = await foto.OpenReadAsync();
            urlFoto = await new FirebaseStorage("examenpm2ac.appspot.com")
                                                .Child("Fotos")
                                                .Child(DateTime.Now.ToString("ddMMyyyyhhmmss") + foto.FileName)
                                                .PutAsync(stream);

            fotoCap.Source = urlFoto;
        }
    }

    private async void OnStartRecordingClicked(object sender, EventArgs e)
    {
        if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
        {
            return;
        }
        if (!_audioRecorder.IsRecording)
        {
            await _audioRecorder.StartAsync();
        }
        else
        {
            var audioRecorded = await _audioRecorder.StopAsync();
            var player = AudioManager.Current.CreatePlayer(audioRecorded.GetAudioStream());
            player.Play();

            var audioStream = audioRecorded.GetAudioStream();
            urlAudio = await storage
                .Child("Audio")
                .Child(DateTime.Now.ToString("ddMMyyyyhhmmss") + ".wav")
                .PutAsync(audioStream);
        }
    }

    private async void GuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtDescripcion.Text))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
        }

        var nota = new Notas
        {
            descripcion = txtDescripcion.Text,
            fecha = datePicker.Date,
            photo_record = urlFoto,
            audio_record = urlAudio
        };

        var firebaseObject = await client.Child("Notas").PostAsync(nota);
        string nuevoId = firebaseObject.Key;

        nota.id_nota = nuevoId;

        await client.Child("Notas").Child(nuevoId).PutAsync(nota);
        ListarNotasPage listapage = new ListarNotasPage();
        listapage.CargarNotas();

        await Shell.Current.GoToAsync("..");
        await DisplayAlert("Éxito", "Los datos se han subido correctamente.", "OK");
    }
}