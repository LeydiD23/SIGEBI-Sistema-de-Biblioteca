using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence.Repository
{
    public class PrestamoRepository
        : BaseRepository<Prestamo>, IPrestamoRepository
    {
        public PrestamoRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosActivosAsync()
        {
            return await _context.Set<Prestamo>()
                .Where(p => p.Estado == EstadoPrestamo.Activo)
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosVencidosAsync()
        {
            return await _context.Set<Prestamo>()
                .Where(p => p.Estado == EstadoPrestamo.Vencido)
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _context.Set<Prestamo>()
                .Where(p => p.EstudianteId == estudianteId)
                .Include(p => p.Libro)
                .Include(p => p.Penalizaciones)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetByDocenteIdAsync(int docenteId)
        {
            return await _context.Set<Prestamo>()
                .Where(p => p.DocenteId == docenteId)
                .Include(p => p.Libro)
                .Include(p => p.Penalizaciones)
                .ToListAsync();
        }
    }
}
