using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
        Task<IEnumerable<Prestamo>> GetPrestamosActivosAsync();
        Task<IEnumerable<Prestamo>> GetByEstudianteIdAsync(int estudianteId);
        Task<IEnumerable<Prestamo>> GetByDocenteIdAsync(int docenteId);
        Task<IEnumerable<Prestamo>> GetPrestamosVencidosAsync();
    }
}
