using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using CatalogoApp.UI.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CatalogoApp.UI.ViewModels
{
    public partial class ArticuloViewModel : BaseViewModel
    {
        private readonly IRepositorioArticulo _repo;
        private readonly IRepositorioMarca _marcaRepo;
        private readonly IRepositorioCategoria _categoriaRepo;

        private List<Articulo> _listaCompletaArticulos; // lista maestra para poder filtrar con LINQ
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

        // articulo seleccionado
        private Articulo? _articuloSeleccionado;
        public Articulo? ArticuloSeleccionado
        {
            get => _articuloSeleccionado;
            set
            {
                if (_articuloSeleccionado == value)
                    return;

                _articuloSeleccionado = value;
                CargarImagenesDelArticuloSeleccionado();
                _indiceImagenActual = 0;
                ActualizarImagenMostrada();
                OnPropertyChanged();
            }
        }

        //props para filtros
        public ObservableCollection<string> ListaDeCampos { get; set; }
        private string _campoSeleccionado;
        public string CampoSeleccionado
        {
            get => _campoSeleccionado;
            set
            {
                _campoSeleccionado = value;
                OnPropertyChanged(); // Notifica a la UI que el campo cambió
                ActualizarCriterios(); // Llama al método que actualiza la lógica
            }
        }


        public ObservableCollection<string> ListaDeCriterios { get; set; }
        public string CriterioSeleccionado { get; set; }

        public string ValorFiltro { get; set; }
        private string _valorFiltroRapido;
        public string ValorFiltroRapido
        {
            get => _valorFiltroRapido;
            set
            {
                _valorFiltroRapido = value;
                OnPropertyChanged(); // notifica que cambió
                FiltroRapido();
            }
        }
        private bool _EstaHabilitadoEntryValor = true;
        public bool EstaHabilitadoEntryValor
        {
            get => _EstaHabilitadoEntryValor;
            set
            {
                _EstaHabilitadoEntryValor = value;
                OnPropertyChanged(); // notifica que cambió
            }
        }

        // para la galería de imágenes
        private bool _estanVisiblesBotones = true;
        public bool EstanVisiblesBotones
        {
            get => _estanVisiblesBotones;
            set
            {
                _estanVisiblesBotones = value;
                OnPropertyChanged(); // notifica que cambió
            }
        }

        private int _indiceImagenActual = 0;
        private Imagen? _imagenMostrada;
        public Imagen? ImagenMostrada
        {
            get => _imagenMostrada;
            set
            {
                _imagenMostrada = value;
                OnPropertyChanged();
            }
        }

        private string _contadorImagenes;
        public string ContadorImagenes
        {
            get => _contadorImagenes;
            set
            {
                _contadorImagenes = value;
                OnPropertyChanged();
            }
        }


        //Comandos
        public ICommand CargarArticulosCommand { get; }

        // comandos filtros
        public ICommand AplicarFiltrosCommand { get; }
        public ICommand LimpiarFiltrosCommand { get; }

        // comandos galería
        public ICommand ImagenSiguienteCommand { get; }
        public ICommand ImagenAnteriorCommand { get; }


        public ArticuloViewModel(IRepositorioArticulo repo, IRepositorioMarca marcaRepo, IRepositorioCategoria categoriaRepo)
        {
            _repo = repo;
            _marcaRepo = marcaRepo;
            _categoriaRepo = categoriaRepo;

            _listaCompletaArticulos = new List<Articulo>();
            _articulos = new ObservableCollection<Articulo>();
            CargarArticulosCommand = new Command(CargarArticulos);

            ListaDeCampos = new ObservableCollection<string> { "Código", "Nombre", "Descripción", "Marca", "Categoría", "Precio" };
            ListaDeCriterios = new ObservableCollection<string>();
            AplicarFiltrosCommand = new Command(AplicarFiltros);
            LimpiarFiltrosCommand = new Command(LimpiarFiltros);

            _campoSeleccionado = string.Empty;
            CriterioSeleccionado = string.Empty;
            ValorFiltro = string.Empty;
            _valorFiltroRapido = string.Empty;

            _contadorImagenes = string.Empty;
            ImagenSiguienteCommand = new Command(ImagenSiguiente);
            ImagenAnteriorCommand = new Command(ImagenAnterior);
        }



        //Métodos

        private void CargarImagenesDelArticuloSeleccionado()
        {
            if (ArticuloSeleccionado == null) return;

            try
            {
                var imagenes = _repo.ObtenerImagenes(ArticuloSeleccionado.IdArticulo).ToList();

    
                // Limpiamos y llenamos la lista de imágenes del artículo seleccionado
                ArticuloSeleccionado.Imagenes.Clear();
                foreach (var img in imagenes)
                {
                    ArticuloSeleccionado.Imagenes.Add(img);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar imágenes: {ex.Message}");
            }
        }


        private void CargarArticulos()
        {
            try
            {
                _listaCompletaArticulos = _repo.Listar().ToList();

                Articulos.Clear(); // limpiar la lista por si tenía datos antiguos
                foreach (Articulo articulo in _listaCompletaArticulos)
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

        private void ValidarCamposFiltros()
        {
            if (!String.IsNullOrWhiteSpace(ValorFiltro)) return;

            if (!string.IsNullOrEmpty(CampoSeleccionado) || !string.IsNullOrEmpty(CriterioSeleccionado)) return;
        }

        private IEnumerable<Articulo> FiltrarPorPrecio(IEnumerable<Articulo> articulosFiltrados)
        {
            if (!decimal.TryParse(ValorFiltro, out decimal valorDecimal))
            {
                // TODO: Mostrar un mensaje informativo en la UI indicando que debe ingresar un valor numérico
                return articulosFiltrados;
            }

            switch (CriterioSeleccionado)
            {
                case "Mayor que":
                    return articulosFiltrados.Where(a => a.Precio > valorDecimal);
                case "Menor que":
                    return articulosFiltrados.Where(a => a.Precio < valorDecimal);
                case "Igual que":
                    return articulosFiltrados.Where(a => a.Precio == valorDecimal);
                default:
                    // Se puede mostrar un mensaje de error informativo.
                    return articulosFiltrados;
            }
        }

        private IEnumerable<Articulo> FiltrarPorCodigo(IEnumerable<Articulo> articulosFiltrados)
        {
            string valorNormalizado = StringHelper.NormalizarTexto(ValorFiltro);
            switch (CriterioSeleccionado)
            {
                case "Contiene":
                    return articulosFiltrados.Where(a => StringHelper.NormalizarTexto(a.Codigo).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                case "Comienza con":
                    return articulosFiltrados.Where(a => StringHelper.NormalizarTexto(a.Codigo).StartsWith(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                case "Termina con":
                    return articulosFiltrados.Where(a => StringHelper.NormalizarTexto(a.Codigo).EndsWith(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                default:
                    // Se puede mostrar un mensaje de error informativo.
                    return articulosFiltrados;
            }
        }
        private IEnumerable<Articulo> FiltrarPorNombre(IEnumerable<Articulo> articulosFiltrados)
        {
            string valorNormalizado = StringHelper.NormalizarTexto(ValorFiltro);
            switch (CriterioSeleccionado)
            {
                case "Contiene":
                    return articulosFiltrados.Where(a => StringHelper.NormalizarTexto(a.Nombre).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                case "Comienza con":
                    return articulosFiltrados.Where(a => StringHelper.NormalizarTexto(a.Nombre).StartsWith(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                case "Termina con":
                    return articulosFiltrados.Where(a => StringHelper.NormalizarTexto(a.Nombre).EndsWith(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                default:
                    // Se puede mostrar un mensaje de error informativo.
                    return articulosFiltrados;
            }
        }

        private IEnumerable<Articulo> FiltrarPorDescripcion(IEnumerable<Articulo> articulosFiltrados)
        {
            string valorNormalizado = StringHelper.NormalizarTexto(ValorFiltro);
            switch (CriterioSeleccionado)
            {
                case "Contiene":
                    return articulosFiltrados.Where(a => !string.IsNullOrEmpty(a.Descripcion) && StringHelper.NormalizarTexto(a.Descripcion).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                case "Comienza con":
                    return articulosFiltrados.Where(a => !string.IsNullOrEmpty(a.Descripcion) && StringHelper.NormalizarTexto(a.Descripcion).StartsWith(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                case "Termina con":
                    return articulosFiltrados.Where(a => !string.IsNullOrEmpty(a.Descripcion) && StringHelper.NormalizarTexto(a.Descripcion).EndsWith(valorNormalizado, StringComparison.OrdinalIgnoreCase));
                default:
                    // Se puede mostrar un mensaje de error informativo.
                    return articulosFiltrados;
            }
        }
        private IEnumerable<Articulo> FiltrarPorMarca(IEnumerable<Articulo> articulosFiltrados)
        {
            return articulosFiltrados.Where(a => !string.IsNullOrEmpty(a.Marca.Descripcion) && a.Marca.Descripcion.Contains(CriterioSeleccionado, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Articulo> FiltrarPorCategoria(IEnumerable<Articulo> articulosFiltrados)
        {
            return articulosFiltrados.Where(a => !string.IsNullOrEmpty(a.Categoria.Descripcion) && a.Categoria.Descripcion.Contains(CriterioSeleccionado, StringComparison.OrdinalIgnoreCase));
        }

        private void AplicarFiltros()
        {
            ValidarCamposFiltros();

            IEnumerable<Articulo> articulosFiltrados = _listaCompletaArticulos;

            switch (CampoSeleccionado)
            {
                case "Código":
                    articulosFiltrados = FiltrarPorCodigo(articulosFiltrados);
                    break;
                case "Nombre":
                    articulosFiltrados = FiltrarPorNombre(articulosFiltrados);
                    break;
                case "Descripción":
                    articulosFiltrados = FiltrarPorDescripcion(articulosFiltrados);
                    break;
                case "Marca":
                    articulosFiltrados = FiltrarPorMarca(articulosFiltrados);
                    break;
                case "Categoría":
                    articulosFiltrados = FiltrarPorCategoria(articulosFiltrados);
                    break;
                case "Precio":
                    articulosFiltrados = FiltrarPorPrecio(articulosFiltrados);
                    break;
                default:
                    break;
            }


            // limpio el observable y lo cargo con los nuevos datos filtrados
            Articulos.Clear();
            foreach (Articulo articulo in articulosFiltrados)
            {
                Articulos.Add(articulo);
            }
        }

        private void LimpiarFiltros()
        {
            CampoSeleccionado = string.Empty;
            CriterioSeleccionado = string.Empty;
            ValorFiltro = string.Empty;
            OnPropertyChanged(nameof(ValorFiltro)); // se notifica que se cambió el campo

            // se muestra de nuevo la lista completa
            Articulos.Clear();
            foreach (var articulo in _listaCompletaArticulos)
            {
                Articulos.Add(articulo);
            }
        }


        private void CargarMarcasYCategorias(string campo)
        {
            if (campo == "Marca")
            {
                var marcas = _marcaRepo.Listar();
                foreach (var marca in marcas)
                {
                    ListaDeCriterios.Add(marca.Descripcion);
                }
            }
            else
            {
                var categorias = _categoriaRepo.Listar();
                foreach (var categoria in categorias)
                {
                    ListaDeCriterios.Add(categoria.Descripcion);
                }
            }
        }

        private void ActualizarCriterios()
        {
            // limpiamos los criteios
            ListaDeCriterios.Clear();
            EstaHabilitadoEntryValor = true; // inicia habilitado

            if (CampoSeleccionado == "Precio")
            {
                ListaDeCriterios.Add("Mayor que");
                ListaDeCriterios.Add("Menor que");
                ListaDeCriterios.Add("Igual que");
            }
            else if (CampoSeleccionado == "Marca" || CampoSeleccionado == "Categoría")
            {
                EstaHabilitadoEntryValor = false;
                CargarMarcasYCategorias(CampoSeleccionado);
            }
            else
            {
                // resto de campos de texto
                ListaDeCriterios.Add("Contiene");
                ListaDeCriterios.Add("Comienza con");
                ListaDeCriterios.Add("Termina con");
            }
            OnPropertyChanged(nameof(EstaHabilitadoEntryValor)); // aquí se notifica del cambio de estado.
        }


        private void FiltroRapido()
        {
            string valorNormalizado = StringHelper.NormalizarTexto(ValorFiltroRapido);

            if (string.IsNullOrWhiteSpace(valorNormalizado))
            {
                Articulos.Clear();
                foreach (var articulo in _listaCompletaArticulos)
                {
                    Articulos.Add(articulo);
                }
                return;
            }

            var articulosFiltrados = _listaCompletaArticulos.Where(a =>
                StringHelper.NormalizarTexto(a.Codigo).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase) ||
                StringHelper.NormalizarTexto(a.Nombre).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase) ||
                (a.Descripcion != null && StringHelper.NormalizarTexto(a.Descripcion).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase)) ||
                StringHelper.NormalizarTexto(a.Marca.Descripcion).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase) ||
                StringHelper.NormalizarTexto(a.Categoria.Descripcion).Contains(valorNormalizado, StringComparison.OrdinalIgnoreCase)
            );

            // actualizar la lista visible
            Articulos.Clear();
            foreach (var articulo in articulosFiltrados)
            {
                Articulos.Add(articulo);
            }
        }

        private void ImagenSiguiente()
        {
            List<Imagen>? imagenes = ArticuloSeleccionado?.Imagenes;
            if (imagenes == null || imagenes.Count == 0) return;
            _indiceImagenActual = (_indiceImagenActual + 1) % imagenes.Count;
            ActualizarImagenMostrada();
        }

        private void ImagenAnterior()
        {
            List<Imagen>? imagenes = ArticuloSeleccionado?.Imagenes;
            if (imagenes == null || imagenes.Count == 0) return;

            _indiceImagenActual = (_indiceImagenActual - 1 + imagenes.Count) % imagenes.Count;
            ActualizarImagenMostrada();
        }


        private void ActualizarImagenMostrada()
        {
            List<Imagen>? imagenes = ArticuloSeleccionado?.Imagenes;
            
            if (imagenes == null || imagenes.Count == 0)
            {
                ImagenMostrada = new Imagen { UrlImagen = "imagen_default.png" };
                ContadorImagenes = "0 / 0";
                EstanVisiblesBotones = false;
                return;
            }

            EstanVisiblesBotones = imagenes.Count > 1;
            ImagenMostrada = imagenes[_indiceImagenActual];
            ContadorImagenes = $"{_indiceImagenActual + 1} / {imagenes.Count}";
        }

    }
}
