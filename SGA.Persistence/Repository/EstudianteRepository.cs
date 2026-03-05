using SGA.Domain.Entitys;
using SGA.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Repository;

namespace SGA.Persistence.Repository
{
    public class EstudianteRepository
        : BaseRepository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Estudiante?> GetByMatriculaAsync(string matricula)
        {
            return await _context.Set<Estudiante>()
                .Include(e => e.Reservas)
                .Include(e => e.Prestamos)
                .Include(e => e.Penalizaciones)
                .FirstOrDefaultAsync(e => e.Matricula == matricula);
        }

        public async Task<Estudiante?> GetEstudianteCompletoAsync(int id)
        {
            return await _context.Set<Estudiante>()
                .Include(e => e.Reservas)
                .Include(e => e.Prestamos)
                .Include(e => e.Penalizaciones)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
