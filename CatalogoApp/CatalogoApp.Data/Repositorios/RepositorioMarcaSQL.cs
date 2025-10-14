using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using Microsoft.Data.SqlClient;
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
            const string query = @"UPDATE Marcas SET Descripcion = @Descripcion WHERE IdMarca = @IdMarca";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.AddWithValue("@IdMarca", marca.IdMarca);
                    comando.Parameters.AddWithValue("@Descripcion", marca.Descripcion);
                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Agregar(Marca nuevaMarca)
        {
            const string query = @"INSERT INTO Marcas (Descripcion) VALUES (@Descripcion)";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.AddWithValue("@Descripcion", nuevaMarca.Descripcion);
                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idMarca)
        {
            const string query = @"DELETE FROM Marcas WHERE IdMarca = @IdMarca";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.AddWithValue("@IdMarca", idMarca);
                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Marca> Listar()
        {
            const string query = @"SELECT MA.IdMarca, MA.Descripcion FROM Marcas MA";
            List<Marca> marcas = new List<Marca>();

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
                            int idOrdinal = lector.GetOrdinal("IdMarca");
                            int descOrdinal = lector.GetOrdinal("Descripcion");

                            while (lector.Read())
                            {
                                Marca marcaActual = new Marca
                                {
                                    IdMarca = lector.GetInt32(idOrdinal),
                                    Descripcion = lector.GetString(descOrdinal)
                                };
                                marcas.Add(marcaActual);
                            }
                        }
                    }
                }
            }
            return marcas;
        }
    }
}
