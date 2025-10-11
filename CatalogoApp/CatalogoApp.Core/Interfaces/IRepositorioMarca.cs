using CatalogoApp.Core.Entidades;

namespace CatalogoApp.Core.Interfaces
{
    public interface IRepositorioMarca
    {
        IEnumerable<Marca> Listar();
        void Agregar(Marca nuevaMarca);
        void Actualizar(Marca marca);
        void Eliminar(int idMarca);
    }
}
