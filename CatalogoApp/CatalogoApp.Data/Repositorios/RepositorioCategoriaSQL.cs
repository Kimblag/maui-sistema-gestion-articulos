using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CatalogoApp.Data.Repositorios
{
    public class RepositorioCategoriaSQL : IRepositorioCategoria
    {
        private string _stringConnection;

        public RepositorioCategoriaSQL(IConfiguration config)
        {
            string? connection = config.GetConnectionString("CATALOGODB");

            ArgumentNullException.ThrowIfNull(connection);

            _stringConnection = connection;
        }

        public void Actualizar(Categoria categoria)
        {
            throw new NotImplementedException();
        }

        public void Agregar(Categoria nuevaCategoria)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int idCategoria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Categoria> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
