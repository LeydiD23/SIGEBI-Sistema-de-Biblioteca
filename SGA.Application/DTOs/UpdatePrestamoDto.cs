namespace SGA.Application.DTOs
{
    public class UpdatePrestamoDto
    {
        public int Id { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public int Estado { get; set; }
        public int Renovaciones { get; set; }
        public int LibroId { get; set; }
        public int? EstudianteId { get; set; }
        public int? DocenteId { get; set; }
    }
}
