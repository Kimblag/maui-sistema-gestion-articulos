using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI.Views.ContentViews;
public partial class ArticulosView : ContentView
{
    private readonly ArticuloViewModel _viewModel;
    private int currentImageIndex = 0;
    public ArticulosView(ArticuloViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel; // Guardamos una referencia al ViewModel
        _viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ArticuloViewModel.ArticuloSeleccionado))
            {
                currentImageIndex = 0;
                UpdateImage();
            }
        };

        // esto es porque como estamos usando un content view en lugar de 
        // una página, falta el trigger que indique que apareció y
        //  debe ejecutar la llamada de los datos. Esta vista no tiene OnAppearing
        viewModel.CargarArticulosCommand.Execute(null);
    }

    private void UpdateImage()
    {
        var images = _viewModel.ArticuloSeleccionado?.Imagenes;
        if (images == null || !images.Any())
        {
            MainImage.Source = "imagen_default.png";
            ImageCounterLabel.Text = "0 / 0";
            return;
        }

        // Mostrar la imagen actual
        MainImage.Source = images[currentImageIndex].UrlImagen;

        // Actualizar el indicador
        ImageCounterLabel.Text = $"{currentImageIndex + 1} / {images.Count}";
    }

    private void PrevImage_Clicked(object sender, EventArgs e)
    {
        var images = _viewModel.ArticuloSeleccionado?.Imagenes;
        if (images == null || !images.Any()) return;

        currentImageIndex = (currentImageIndex - 1 + images.Count) % images.Count;
        UpdateImage();
    }

    private void NextImage_Clicked(object sender, EventArgs e)
    {
        var images = _viewModel.ArticuloSeleccionado?.Imagenes;
        if (images == null || !images.Any()) return;

        currentImageIndex = (currentImageIndex + 1) % images.Count;
        UpdateImage();
    }

}