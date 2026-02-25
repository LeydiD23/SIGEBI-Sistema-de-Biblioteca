using SGA.Domain.Enums;
using System;

namespace SGA.Domain.Entitys
{
    public class Penalizacion
    {
        public int Id { get; set; }

        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public int DiasRetraso { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaPago { get; set; }
        public EstadoPenalizacion Estado { get; set; }

        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public int? PrestamoId { get; set; }
        public Prestamo? Prestamo { get; set; }
    }
}
