using Examen3PM2_AC0036.Views;

namespace Examen3PM2_AC0036
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AgregarNotaPage), typeof(AgregarNotaPage));
            Routing.RegisterRoute(nameof(ListarNotasPage), typeof(ListarNotasPage));
        }
    }
}