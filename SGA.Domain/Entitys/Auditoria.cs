using System;

namespace SGA.Domain.Entitys
{
    public class Auditoria
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public string Detalles { get; set; }
        public DateTime Fecha { get; set; }
    }
}
