using Microsoft.Extensions.DependencyInjection;
using SGA.Domain.Repository;
using SGA.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SGA.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ILibroRepository, LibroRepository>();
        services.AddScoped<IEstudianteRepository, EstudianteRepository>();
        services.AddScoped<IReservaRepository, ReservaRepository>()
        services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
        services.AddScoped<IPrestamoRepository, PrestamoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
        services.AddScoped<IDocenteRepository, DocenteRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();

        return services;
    }
}

