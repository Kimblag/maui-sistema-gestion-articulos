using CatalogoApp.Core.Interfaces;

namespace CatalogoApp.UI.ViewModels
{
    public partial class MarcaViewModel : BaseViewModel
    {
        private readonly IRepositorioMarca _repo;
        public MarcaViewModel(IRepositorioMarca repo)
        {
            _repo = repo;
        }
    }
}
