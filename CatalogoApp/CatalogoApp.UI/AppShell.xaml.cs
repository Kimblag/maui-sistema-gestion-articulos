using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArticuloPage), typeof(ArticuloPage));
            Routing.RegisterRoute(nameof(MarcaPage), typeof(MarcaPage));
            Routing.RegisterRoute(nameof(CategoriaPage), typeof(CategoriaPage));
        }
    }
}
