using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto> GetByIdAsync(int id);
        Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto);
        Task<CategoriaDto> UpdateAsync(UpdateCategoriaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
