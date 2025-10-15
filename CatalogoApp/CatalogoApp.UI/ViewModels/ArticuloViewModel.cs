using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CatalogoApp.UI.ViewModels
{
    public partial class ArticuloViewModel : BaseViewModel
    {
        private readonly IRepositorioArticulo _repo;

        private ObservableCollection<Articulo> _articulos;
        public ObservableCollection<Articulo> Articulos
        {
            get => _articulos;
            set
            {
                _articulos = value;
                OnPropertyChanged();
            }
        }

        public ICommand CargarArticulosCommand { get; }
        public ArticuloViewModel(IRepositorioArticulo repo)
        {
            _repo = repo;
            _articulos = new ObservableCollection<Articulo>();
            CargarArticulosCommand = new Command(CargarArticulos);
        }

        private void CargarArticulos() {

            try
            {
                IEnumerable<Articulo> articulos = _repo.Listar();
                Articulos.Clear(); // limpiar la lista por si tenía datos antiguos
                foreach (Articulo articulo in articulos)
                {
                    Articulos.Add(articulo);
                }
            }
            catch (Exception ex)
            {
                //TODO: PEndiente agregar mensajes popup de error
                System.Diagnostics.Debug.WriteLine($"Error al cargar artículos: {ex.Message}");
            }
        }
    }
}
