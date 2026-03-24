namespace SGA.Application.DTOs
{
    public class LibroDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Ubicacion { get; set; }
        public string Editorial { get; set; }
        public int Stock { get; set; }
        public int StockDisponible { get; set; }
        public string Estado { get; set; }
        public DateTime FechaAdquisicion { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
    }
}
