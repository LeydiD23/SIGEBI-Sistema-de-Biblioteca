using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface IEstudianteService
    {
        Task<IEnumerable<EstudianteDto>> GetAllAsync();
        Task<EstudianteDto> GetByIdAsync(int id);
        Task<EstudianteDto> GetByMatriculaAsync(string matricula);
        Task<EstudianteDto> CreateAsync(CreateEstudianteDto dto);
        Task<EstudianteDto> UpdateAsync(UpdateEstudianteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
