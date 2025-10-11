using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CatalogoApp.Data.Repositorios
{
    public class RepositorioMarcaSQL : IRepositorioMarca
    {
        private string _stringConnection;

        public RepositorioMarcaSQL(IConfiguration config)
        {
            string? connection = config.GetConnectionString("CATALOGODB");

            ArgumentNullException.ThrowIfNull(connection);

            _stringConnection = connection;
        }

        public void Actualizar(Marca marca)
        {
            throw new NotImplementedException();
        }

        public void Agregar(Marca nuevaMarca)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int idMarca)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Marca> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
