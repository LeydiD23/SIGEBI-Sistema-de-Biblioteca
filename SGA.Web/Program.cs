using Microsoft.EntityFrameworkCore;
using SGA.Persistence;
using SGA.Domain.Interfaces;
using SGA.Application.Services;
using SGA.Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSingleton<IPasswordHasher, PasswordHasherService>();

builder.Services.AddHttpClient<ILibroApiService, LibroApiService>();
builder.Services.AddHttpClient<ICategoriaApiService, CategoriaApiService>();
builder.Services.AddHttpClient<IPrestamoApiService, PrestamoApiService>();
builder.Services.AddHttpClient<IReservaApiService, ReservaApiService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "http://localhost:5000/api");
        var response = await httpClient.GetAsync("/libros");
        if (response.IsSuccessStatusCode)
            Console.WriteLine(" API conectada");
        else
            Console.WriteLine(" API no disponible");
    }
    catch
    {
        Console.WriteLine(" API no disponible");
    }
}


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.CanConnect())
        Console.WriteLine(" Conexi�n exitosa");
    else
        Console.WriteLine(" No se pudo conectar");
}




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Temporalmente deshabilitado
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
