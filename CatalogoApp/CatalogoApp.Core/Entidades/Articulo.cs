namespace CatalogoApp.Core.Entidades
{
    public class Articulo
    {
        public int Id { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }

        // Asociaciones
        public required Marca Marca { get; set; }
        public required Categoria Categoria { get; set; }
        public List<Imagen> Imagenes { get; set; } = new List<Imagen>();

    }
}
