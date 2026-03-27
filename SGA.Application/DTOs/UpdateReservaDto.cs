namespace SGA.Application.DTOs
{
    public class UpdateReservaDto
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int PosicionCola { get; set; }
        public int Estado { get; set; }
        public int LibroId { get; set; }
        public int? EstudianteId { get; set; }
        public int? DocenteId { get; set; }
    }
}
