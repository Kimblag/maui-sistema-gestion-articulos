using CatalogoApp.Core.Interfaces;

namespace CatalogoApp.UI.ViewModels
{
    public partial class ArticuloViewModel : BaseViewModel
    {
        private readonly IRepositorioArticulo _repo;
        public ArticuloViewModel(IRepositorioArticulo repo)
        {
            _repo = repo;
        }
    }
}
