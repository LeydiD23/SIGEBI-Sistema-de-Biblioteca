namespace SGA.Application.DTOs
{
    public class CreatePenalizacionDto
    {
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public int DiasRetraso { get; set; }
        public int? EstudianteId { get; set; }
        public int? DocenteId { get; set; }
        public int? PrestamoId { get; set; }
    }
}
