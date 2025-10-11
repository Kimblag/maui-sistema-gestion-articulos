using CatalogoApp.Core.Entidades;

namespace CatalogoApp.Core.Interfaces
{
    public interface IRepositorioCategoria
    {
        IEnumerable<Categoria> Listar();
        void Agregar(Categoria nuevaCategoria);
        void Actualizar(Categoria categoria);
        void Eliminar(int idCategoria);
    }
}
