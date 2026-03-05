using Microsoft.EntityFrameworkCore;
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
    public class AuditoriaRepository
        : BaseRepository<Auditoria>, IAuditoriaRepository
    {
        public AuditoriaRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Auditoria>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Set<Auditoria>()
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> GetByEntidadAsync(string entidad)
        {
            return await _context.Set<Auditoria>()
                .Where(a => a.Entidad == entidad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Set<Auditoria>()
                .Where(a => a.Fecha.Date == fecha.Date)
                .ToListAsync();
        }
    }
}
