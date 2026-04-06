using SGA.Application.DTOs;

namespace SGA.Web.Services
{
    public interface ILibroApiService
    {
        Task<List<LibroDto>?> GetAllAsync();
        Task<LibroDto?> GetByIdAsync(int id);
        Task<List<LibroDto>?> SearchAsync(string searchTerm);
    }

    public class LibroApiService : ApiService, ILibroApiService
    {
        public LibroApiService(HttpClient httpClient, IConfiguration configuration) 
            : base(httpClient, configuration) { }

        public async Task<List<LibroDto>?> GetAllAsync()
        {
            return await GetListAsync<LibroDto>("libros");
        }

        public async Task<LibroDto?> GetByIdAsync(int id)
        {
            return await GetAsync<LibroDto>($"libros/{id}");
        }

        public async Task<List<LibroDto>?> SearchAsync(string searchTerm)
        {
            return await GetListAsync<LibroDto>($"libros/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
        }
    }
}
