using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CatalogoApp.UI.ViewModels
{
    public partial class MarcaViewModel : BaseViewModel
    {
        private readonly IRepositorioMarca _repo;

        private ObservableCollection<Marca> _marcas;
        public ObservableCollection<Marca> Marcas
        {
            get => _marcas;
            set
            {
                _marcas = value;
                OnPropertyChanged(); // Notifica a la UI que la propiedad ha cambiado
            }
        }
        public ICommand CargarMarcasCommand { get; }


        public MarcaViewModel(IRepositorioMarca repo)
        {
            _repo = repo;
            _marcas = new ObservableCollection<Marca>();
            CargarMarcasCommand = new Command(CargarMarcas);
        }

        private void CargarMarcas()
        {
            try
            {
                IEnumerable<Marca> marcas = _repo.Listar();

                Marcas.Clear(); // limpiar la lista por si tenía datos antiguos

                foreach (Marca marca in marcas)
                {
                    Marcas.Add(marca);
                }

            }
            catch (Exception ex)
            {
                //TODO: PEndiente agregar mensajes popup de error

                System.Diagnostics.Debug.WriteLine($"Error al cargar marcas: {ex.Message}");
            }
        }

    }
}
