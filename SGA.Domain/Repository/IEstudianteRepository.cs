using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IEstudianteRepository : IBaseRepository<Estudiante>
    {
        Task<Estudiante?> GetByMatriculaAsync(string matricula);
        Task<Estudiante?> GetEstudianteCompletoAsync(int id);
    }
}
