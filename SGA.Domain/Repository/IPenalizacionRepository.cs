using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        Task<IEnumerable<Penalizacion>> GetPenalizacionesActivasAsync();
        Task<IEnumerable<Penalizacion>> GetByEstudianteIdAsync(int estudianteId);
        Task<IEnumerable<Penalizacion>> GetByDocenteIdAsync(int docenteId);
    }
}
