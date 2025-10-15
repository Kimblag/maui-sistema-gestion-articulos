using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI.Views.ContentViews;
public partial class ArticulosView : ContentView
{
	public ArticulosView(ArticuloViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		// esto es porque como estamos usando un content view en lugar de 
		// una p�gina, falta el trigger que indique que apareci� y
		//  debe ejecutar la llamada de los datos. Esta vista no tiene OnAppearing
		viewModel.CargarArticulosCommand.Execute(null);
	}
}