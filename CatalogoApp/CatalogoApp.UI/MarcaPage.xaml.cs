using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI;

public partial class MarcaPage : ContentPage
{
    public MarcaPage(MarcaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // enlazamos el contexto del VM
    }


    // Método sobreescrito para cambiar comportamiento cuando se dibuje la página
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MarcaViewModel)?.CargarMarcasCommand.Execute(null);
    }
}