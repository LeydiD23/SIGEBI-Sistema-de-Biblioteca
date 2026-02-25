using System;

namespace SGA.Domain.Entitys
{
    public class Notificacion
    {
        public int Id { get; set; }
        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
    }
}
