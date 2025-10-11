using CatalogoApp.Core.Interfaces;

namespace CatalogoApp.UI.ViewModels
{
    public partial class CategoriaViewModel : BaseViewModel
    {
        private readonly IRepositorioCategoria _repo;
        public CategoriaViewModel(IRepositorioCategoria repo)
        {
            _repo = repo;
        }
    }
}
