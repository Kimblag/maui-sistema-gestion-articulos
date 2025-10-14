using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI;

public partial class MarcaPage : ContentPage
{
    public MarcaPage(MarcaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // enlazamos el contexto del VM
    }


    // M�todo sobreescrito para cambiar comportamiento cuando se dibuje la p�gina
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MarcaViewModel)?.CargarMarcasCommand.Execute(null);
    }
}