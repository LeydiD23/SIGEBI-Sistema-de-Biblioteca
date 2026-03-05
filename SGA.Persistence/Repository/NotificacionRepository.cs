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
    public class NotificacionRepository
       : BaseRepository<Notificacion>, INotificacionRepository
    {
        public NotificacionRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Notificacion>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _context.Set<Notificacion>()
                .Where(n => n.EstudianteId == estudianteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetByDocenteIdAsync(int docenteId)
        {
            return await _context.Set<Notificacion>()
                .Where(n => n.DocenteId == docenteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetNoLeidasAsync()
        {
            return await _context.Set<Notificacion>()
                .Where(n => n.Leida == false)
                .ToListAsync();
        }

        public async Task MarcarComoLeidaAsync(int id)
        {
            var notificacion = await _context.Set<Notificacion>().FindAsync(id);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                _context.Update(notificacion);
            }
        }
    }
}
