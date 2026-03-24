namespace SGA.Application.DTOs
{
    public class PrestamoDto
    {
        public int Id { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public string Estado { get; set; }
        public int Renovaciones { get; set; }
        public int LibroId { get; set; }
        public string LibroTitulo { get; set; }
        public int? EstudianteId { get; set; }
        public string? EstudianteNombre { get; set; }
        public int? DocenteId { get; set; }
        public string? DocenteNombre { get; set; }
    }
}
