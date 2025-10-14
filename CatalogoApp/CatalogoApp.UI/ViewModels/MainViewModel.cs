using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace CatalogoApp.UI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {

        public ICommand NavegarAArticulosCommand { get; }
        public ICommand NavegarAMarcasCommand { get; }
        public ICommand NavegarACategoriasCommand { get; }


        public MainViewModel()
        {
            NavegarAArticulosCommand = new Command(async () => await NavegarAArticulos());
            NavegarAMarcasCommand = new Command(async () => await NavegarAMarcas());
            NavegarACategoriasCommand = new Command(async () => await NavegarACategorias());
        }

        //Métodos
        #region Métodos
        private async Task NavegarAArticulos()
        {
            await Shell.Current.GoToAsync(nameof(ArticuloPage));
        }
        private async Task NavegarAMarcas()
        {
            await Shell.Current.GoToAsync(nameof(MarcaPage));
        }
        private async Task NavegarACategorias()
        {
            await Shell.Current.GoToAsync(nameof(CategoriaPage));
        }

        #endregion

    }
}
