using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetReservasActivasAsync();
        Task<IEnumerable<Reserva>> GetByEstudianteIdAsync(int estudianteId);
        Task<IEnumerable<Reserva>> GetByLibroIdAsync(int libroId);
    }
}
