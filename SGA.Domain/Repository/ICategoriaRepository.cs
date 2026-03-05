using SGA.Domain.Entitys;
using SGA.Domain.Repository;

public interface ICategoriaRepository : IBaseRepository<Categoria>
{
    Task<Categoria?> GetByNombreAsync(string nombre);
    Task<IEnumerable<Categoria>> GetCategoriasConLibrosAsync();
}