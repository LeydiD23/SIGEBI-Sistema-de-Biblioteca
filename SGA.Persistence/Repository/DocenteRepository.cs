using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Entitys;
using SGA.Domain.Repository;

namespace SGA.Persistence.Repository
{
    public class DocenteRepository
        : BaseRepository<Docente>, IDocenteRepository
    {
        public DocenteRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Docente?> GetByCedulaAsync(string cedula)
        {
            return await _context.Set<Docente>()
                .FirstOrDefaultAsync(d => d.Cedula == cedula);
        }

        public async Task<Docente?> GetByEmailAsync(string email)
        {
            return await _context.Set<Docente>()
                .FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<IEnumerable<Docente>> GetDocentesActivosAsync()
        {
            return await _context.Set<Docente>()
                .Where(d => d.Estado == true)
                .ToListAsync();
        }
    }
}
