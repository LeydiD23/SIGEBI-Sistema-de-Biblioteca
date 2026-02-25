using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entitys;
using SGA.Domain.Enums;

namespace SGA.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Bibliotecario> Bibliotecarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<ParametroSistema> ParametrosSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Libro
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Libro>()
                .Property(l => l.Estado)
                .HasConversion<int>();

            // Configuración de Reserva
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Estudiante)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Docente)
                .WithMany(d => d.Reservas)
                .HasForeignKey(r => r.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Libro)
                .WithMany(l => l.Reservas)
                .HasForeignKey(r => r.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Estado)
                .HasConversion<int>();

            // Configuración de Prestamo
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Estudiante)
                .WithMany(e => e.Prestamos)
                .HasForeignKey(p => p.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Docente)
                .WithMany(d => d.Prestamos)
                .HasForeignKey(p => p.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .Property(p => p.Estado)
                .HasConversion<int>();

            // Configuración de Penalizacion
            modelBuilder.Entity<Penalizacion>()
                .HasOne(p => p.Estudiante)
                .WithMany(e => e.Penalizaciones)
                .HasForeignKey(p => p.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Penalizacion>()
                .HasOne(p => p.Docente)
                .WithMany(d => d.Penalizaciones)
                .HasForeignKey(p => p.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Penalizacion>()
                .HasOne(p => p.Prestamo)
                .WithMany(p => p.Penalizaciones)
                .HasForeignKey(p => p.PrestamoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Penalizacion>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Penalizacion>()
                .Property(p => p.Estado)
                .HasConversion<int>();

            // Configuración de Notificacion
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Estudiante)
                .WithMany()
                .HasForeignKey(n => n.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Docente)
                .WithMany()
                .HasForeignKey(n => n.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
