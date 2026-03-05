using SGA.Domain.Entitys;
using SGA.Domain.Repository;
using SGA.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence.Repository
{
    public class BibliotecarioRepository
       : BaseRepository<Bibliotecario>, IBibliotecarioRepository
    {
        public BibliotecarioRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Bibliotecario?> GetByEmailAsync(string email)
        {
            return await _context.Set<Bibliotecario>()
                .FirstOrDefaultAsync(b => b.Email == email);
        }

        public async Task<IEnumerable<Bibliotecario>> GetActivosAsync()
        {
            return await _context.Set<Bibliotecario>()
                .Where(b => b.Estado == true)
                .ToListAsync();
        }
    }
}
