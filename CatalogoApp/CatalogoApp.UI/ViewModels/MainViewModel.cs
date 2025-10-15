using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;

using System.Windows.Input;
using Microsoft.Maui.Controls;
using CatalogoApp.UI.Views.ContentViews;
using CatalogoApp.UI.Helpers;

namespace CatalogoApp.UI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly ContentView _contentArea;
        public ICommand VerArticulosCommand { get; }
        public ICommand VerMarcasCommand { get; }
        public ICommand VerCategoriasCommand { get; }
        public MainViewModel(ContentView contenArea)
        {
            _contentArea = contenArea;

            VerArticulosCommand = new Command(() => VerArticulos());
            VerMarcasCommand = new Command(() => VerMarcas());
            VerCategoriasCommand = new Command(() => VerCategorias());

            VerArticulos(); // inicia siempre en esta vista
        }

        public void VerArticulos()
        {
            _contentArea.Content = ServiceHelper.GetService<ArticulosView>();
        }

        public void VerMarcas()
        {
            _contentArea.Content = ServiceHelper.GetService<MarcasView>();
        }

        public void VerCategorias()
        {
            _contentArea.Content = ServiceHelper.GetService<CategoriasView>();
        }

    }
}
