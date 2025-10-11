using CatalogoApp.Core.Entidades;

namespace CatalogoApp.Core.Interfaces
{
    public interface IRepositorioArticulo
    {
        IEnumerable<Articulo> Listar();
        void Agregar(Articulo nuevoArticulo);
        void Actualizar(Articulo articulo);
        void Eliminar(int idArticulo);
    }
}
