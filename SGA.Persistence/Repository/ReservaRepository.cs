using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence.Repository
{
    public class ReservaRepository
        : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Reserva>> GetReservasActivasAsync()
        {
            return await _context.Set<Reserva>()
                .Where(r => r.Estado == EstadoReserva.Activa)
                .Include(r => r.Libro)
                .Include(r => r.Estudiante)
                .Include(r => r.Docente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _context.Set<Reserva>()
                .Where(r => r.EstudianteId == estudianteId)
                .Include(r => r.Libro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByLibroIdAsync(int libroId)
        {
            return await _context.Set<Reserva>()
                .Where(r => r.LibroId == libroId)
                .Include(r => r.Estudiante)
                .Include(r => r.Docente)
                .ToListAsync();
        }
    }
}
