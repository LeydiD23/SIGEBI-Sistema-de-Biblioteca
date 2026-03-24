using Microsoft.Extensions.DependencyInjection;
using SGA.Domain.Repository;
using SGA.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SGA.Persistence;
using SGA.Application.Interfaces;
using SGA.Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ILibroRepository, LibroRepository>();

        services.AddScoped<ILibroService, LibroService>();
        services.AddScoped<IEstudianteService, EstudianteService>();
        services.AddScoped<IDocenteService, DocenteService>();
        services.AddScoped<IPrestamoService, PrestamoService>();
        services.AddScoped<IReservaService, ReservaService>();
        services.AddScoped<IPenalizacionService, PenalizacionService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IBibliotecarioService, BibliotecarioService>();

        return services;
    }
}

