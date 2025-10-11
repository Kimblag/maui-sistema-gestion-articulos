using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CatalogoApp.Data.Repositorios
{
    public class RepositorioArticuloSQL : IRepositorioArticulo
    {
        private string _stringConnection;

        public RepositorioArticuloSQL(IConfiguration config)
        {
            string? connection = config.GetConnectionString("CATALOGODB");

            ArgumentNullException.ThrowIfNull(connection);

            _stringConnection = connection;
        }

        public void Actualizar(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public void Agregar(Articulo nuevoArticulo)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int idArticulo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Articulo> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
