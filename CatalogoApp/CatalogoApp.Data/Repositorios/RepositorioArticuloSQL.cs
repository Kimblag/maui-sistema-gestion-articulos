using CatalogoApp.Core.Entidades;
using CatalogoApp.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

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
            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                string nombreSP = "sp_Atualizar_Articulo";
                using (SqlCommand comando = new SqlCommand(nombreSP, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idArticulo", articulo.IdArticulo);
                    comando.Parameters.AddWithValue("@Codigo", articulo.Codigo);
                    comando.Parameters.AddWithValue("@Nombre", articulo.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", articulo.Descripcion);
                    comando.Parameters.AddWithValue("@Precio", articulo.Precio);
                    comando.Parameters.AddWithValue("@IdMarca", articulo.Marca.IdMarca);
                    comando.Parameters.AddWithValue("@IdCategoria", articulo.Categoria.IdCategoria);

                    SqlParameter parametroImagenes = CrearDataTableImagenes(articulo);
                    comando.Parameters.Add(parametroImagenes);

                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        private SqlParameter CrearDataTableImagenes(Articulo articulo)
        {
            // Siempre pasar la lista aunque esté vacía.
            // el sp espera una lista de imágenes en forma de tabla, se debe convertir en una DataTable
            var dtImagenes = new DataTable(); // crear la tabla en memoria
            dtImagenes.Columns.Add("UrlImagen", typeof(string)); // crear la columna

            // agregar las filas
            foreach (Imagen imagen in articulo.Imagenes)
            {
                dtImagenes.Rows.Add(imagen.UrlImagen);
            }

            // construir el parámetro especial. Se indica el tipo como estructurado ya qu eestá
            // representado por una DataTable
            SqlParameter parametroImagenes = new SqlParameter("@ListaImagenes", SqlDbType.Structured)
            {
                TypeName = "dbo.TipoListaImagenes",
                Value = dtImagenes
            };
            return parametroImagenes;
        }

        public void Agregar(Articulo nuevoArticulo)
        {
            const string spAgregar = "sp_Agregar_Articulo";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(spAgregar, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@Codigo", nuevoArticulo.Codigo);
                    comando.Parameters.AddWithValue("@Nombre", nuevoArticulo.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", nuevoArticulo.Descripcion);
                    comando.Parameters.AddWithValue("@Precio", nuevoArticulo.Precio);
                    comando.Parameters.AddWithValue("@IdMarca", nuevoArticulo.Marca.IdMarca);
                    comando.Parameters.AddWithValue("@IdCategoria", nuevoArticulo.Categoria.IdCategoria);

                    SqlParameter parametroImagenes = CrearDataTableImagenes(nuevoArticulo);
                    comando.Parameters.Add(parametroImagenes);

                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idArticulo)
        {
            string sp_eliminar = "sp_Eliminar_Articulo";
            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(sp_eliminar, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@IdArticulo", idArticulo);

                    comando.Connection.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Articulo> Listar()
        {
            List<Articulo> articulos = new List<Articulo>();
            const string query = @"SELECT IdArticulo, Codigo, Nombre, Descripcion, Precio, IdMarca, Marca, IdCategoria, Categoria 
                           FROM vw_Articulos";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.HasRows)
                        {
                            // Obtenemos los ordinales una vez para mayor eficiencia
                            int idOrdinal = lector.GetOrdinal("IdArticulo");
                            int codigoOrdinal = lector.GetOrdinal("Codigo");
                            int nombreOrdinal = lector.GetOrdinal("Nombre");
                            int descripcionOrdinal = lector.GetOrdinal("Descripcion");
                            int precioOrdinal = lector.GetOrdinal("Precio");
                            int idMarcaOrdinal = lector.GetOrdinal("IdMarca");
                            int marcaOrdinal = lector.GetOrdinal("Marca");
                            int idCategoriaOrdinal = lector.GetOrdinal("IdCategoria");
                            int categoriaOrdinal = lector.GetOrdinal("Categoria");

                            while (lector.Read())
                            {
                                articulos.Add(new Articulo
                                {
                                    IdArticulo = lector.GetInt32(idOrdinal),
                                    Codigo = lector.GetString(codigoOrdinal),
                                    Nombre = lector.GetString(nombreOrdinal),
                                    Descripcion = lector.IsDBNull(descripcionOrdinal) ? null : lector.GetString(descripcionOrdinal),
                                    Precio = lector.GetDecimal(precioOrdinal),
                                    Marca = new Marca { IdMarca = lector.GetInt32(idMarcaOrdinal), Descripcion = lector.GetString(marcaOrdinal) },
                                    Categoria = new Categoria { IdCategoria = lector.GetInt32(idCategoriaOrdinal), Descripcion = lector.GetString(categoriaOrdinal) }
                                });
                            }
                        }
                    }
                }
            }
            return articulos;
        }

        public List<Imagen> ObtenerImagenes(int idArticulo)
        {
            List<Imagen> imagenes = new List<Imagen>();
            const string query = "SELECT IdImagen, UrlImagen FROM Imagenes WHERE IdArticulo = @IdArticulo";

            using (SqlConnection conexion = new SqlConnection(_stringConnection))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.CommandType = CommandType.Text;
                    comando.Parameters.AddWithValue("@IdArticulo", idArticulo);
                    comando.Connection.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.HasRows)
                        {
                            int idOrdinal = lector.GetOrdinal("IdImagen");
                            int urlOrdinal = lector.GetOrdinal("UrlImagen");

                            while (lector.Read())
                            {
                                Imagen imagenActual = new Imagen
                                {
                                    IdImagen = lector.GetInt32(idOrdinal),
                                    UrlImagen = lector.GetString(urlOrdinal)
                                };
                                imagenes.Add(imagenActual);
                            }
                        }
                    }
                }
            }
            return imagenes;
        }

    }
}
