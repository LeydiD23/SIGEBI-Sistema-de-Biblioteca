using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Entitys
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<Libro> Libros { get; set; }
    }
}
