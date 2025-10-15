using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI.Views.ContentViews;
public partial class ArticulosView : ContentView
{
	public ArticulosView(ArticuloViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		// esto es porque como estamos usando un content view en lugar de 
		// una página, falta el trigger que indique que apareció y
		//  debe ejecutar la llamada de los datos. Esta vista no tiene OnAppearing
		viewModel.CargarArticulosCommand.Execute(null);
	}
}