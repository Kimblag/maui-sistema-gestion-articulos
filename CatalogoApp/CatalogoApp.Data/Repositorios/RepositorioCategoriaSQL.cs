using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

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
            const string query = @"UPDATE Categorias SET Descripcion = @Descripcion WHERE IdCategoria = @IdCategoria";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                    comando.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Agregar(Categoria nuevaCategoria)
        {
            const string query = @"INSERT INTO Categorias (Descripcion) VALUES (@Descripcion)";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.AddWithValue("@Descripcion", nuevaCategoria.Descripcion);
                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idCategoria)
        {
            const string query = @"DELETE FROM Categorias WHERE IdCategoria = @IdCategoria";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.AddWithValue("@IdCategoria", idCategoria);
                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Categoria> Listar()
        {
            const string query = @"SELECT CAT.IdCategoria, CAT.Descripcion FROM Categorias CAT";
            List<Categoria> categorias = new List<Categoria>();

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Connection.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.HasRows)
                        {
                            int idOrdinal = lector.GetOrdinal("IdCategoria");
                            int descOrdinal = lector.GetOrdinal("Descripcion");

                            while (lector.Read())
                            {
                                Categoria categoriaActual = new Categoria
                                {
                                    IdCategoria = lector.GetInt32(idOrdinal),
                                    Descripcion = lector.GetString(descOrdinal)
                                };
                                categorias.Add(categoriaActual);
                            }
                        }
                    }
                }
            }
            return categorias;
        }
    }
}
