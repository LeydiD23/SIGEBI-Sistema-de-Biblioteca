namespace SGA.Application.DTOs
{
    public class CreateLibroDto
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Ubicacion { get; set; }
        public string Editorial { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
    }
}
