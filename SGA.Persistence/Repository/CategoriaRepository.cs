using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence.Repository
{
    public class CategoriaRepository
         : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Categoria?> GetByNombreAsync(string nombre)
        {
            return await _context.Set<Categoria>()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasConLibrosAsync()
        {
            return await _context.Set<Categoria>()
                .Include(c => c.Libros)
                .ToListAsync();
        }
    }
}
