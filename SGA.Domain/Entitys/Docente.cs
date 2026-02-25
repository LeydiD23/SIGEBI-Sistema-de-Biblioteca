using System.Collections.Generic;

namespace SGA.Domain.Entitys
{
    public class Docente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Departamento { get; set; }
        public bool Estado { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }
        public ICollection<Prestamo>? Prestamos { get; set; }
        public ICollection<Penalizacion>? Penalizaciones { get; set; }
    }
}
