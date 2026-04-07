using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface IDocenteService
    {
        Task<IEnumerable<DocenteDto>> GetAllAsync();
        Task<DocenteDto> GetByIdAsync(int id);
        Task<DocenteDto> GetByCedulaAsync(string cedula);
        Task<DocenteDto> CreateAsync(CreateDocenteDto dto);
        Task<DocenteDto> UpdateAsync(UpdateDocenteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
