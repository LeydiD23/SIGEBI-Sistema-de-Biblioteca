namespace SGA.Application.DTOs
{
    public class PenalizacionDto
    {
        public int Id { get; set; }
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public int DiasRetraso { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Estado { get; set; }
        public int? EstudianteId { get; set; }
        public string? EstudianteNombre { get; set; }
        public int? DocenteId { get; set; }
        public string? DocenteNombre { get; set; }
        public int? PrestamoId { get; set; }
    }
}
