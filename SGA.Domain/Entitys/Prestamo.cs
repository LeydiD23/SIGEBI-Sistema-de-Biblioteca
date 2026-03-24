using SGA.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entitys
{
    public class Prestamo
    {
        public int Id { get; set; }

        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public int Renovaciones { get; set; }
        public EstadoPrestamo Estado { get; set; }

        public int LibroId { get; set; }
        public Libro? Libro { get; set; }

        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public ICollection<Penalizacion>? Penalizaciones { get; set; }
    }
}
