namespace SGA.Application.DTOs
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int PosicionCola { get; set; }
        public string Estado { get; set; }
        public int LibroId { get; set; }
        public string LibroTitulo { get; set; }
        public int? EstudianteId { get; set; }
        public string? EstudianteNombre { get; set; }
        public int? DocenteId { get; set; }
        public string? DocenteNombre { get; set; }
    }
}
