using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entitys;
using SGA.Domain.Repository;
using SGA.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Enums;

namespace SGA.Persistence.Repository
{
    public class PenalizacionRepository
       : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        public PenalizacionRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Penalizacion>> GetPenalizacionesActivasAsync()
        {
            return await _context.Set<Penalizacion>()
                .Where(p => p.Estado == EstadoPenalizacion.Activa)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Include(p => p.Prestamo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Penalizacion>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _context.Set<Penalizacion>()
                .Where(p => p.EstudianteId == estudianteId)
                .Include(p => p.Prestamo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Penalizacion>> GetByDocenteIdAsync(int docenteId)
        {
            return await _context.Set<Penalizacion>()
                .Where(p => p.DocenteId == docenteId)
                .Include(p => p.Prestamo)
                .ToListAsync();
        }
    }
}
