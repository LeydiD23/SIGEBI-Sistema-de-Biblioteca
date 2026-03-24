namespace SGA.Application.DTOs
{
    public class UpdateBibliotecarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Cargo { get; set; }
        public bool Estado { get; set; }
    }
}
