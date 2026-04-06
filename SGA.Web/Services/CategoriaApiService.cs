using SGA.Application.DTOs;

namespace SGA.Web.Services
{
    public interface ICategoriaApiService
    {
        Task<List<CategoriaDto>?> GetAllAsync();
    }

    public class CategoriaApiService : ApiService, ICategoriaApiService
    {
        public CategoriaApiService(HttpClient httpClient, IConfiguration configuration) 
            : base(httpClient, configuration) { }

        public async Task<List<CategoriaDto>?> GetAllAsync()
        {
            return await GetListAsync<CategoriaDto>("categorias");
        }
    }
}
