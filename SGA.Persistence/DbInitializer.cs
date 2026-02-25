using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            
            context.Database.Migrate();


            Console.WriteLine("🔥 SEED EJECUTADO 🔥");

            var categorias = new List<Categoria>
            {
                new Categoria { Nombre = "Programación" },
                new Categoria { Nombre = "Bases de Datos" },
                new Categoria { Nombre = "Redes y Telecomunicaciones" },
                new Categoria { Nombre = "Inteligencia Artificial" },
                new Categoria { Nombre = "Matemáticas" },
                new Categoria { Nombre = "Física" },
                new Categoria { Nombre = "Química" },
                new Categoria { Nombre = "Historia" },
                new Categoria { Nombre = "Literatura" },
                new Categoria { Nombre = "Psicología" },
                new Categoria { Nombre = "Derecho" },
                new Categoria { Nombre = "Administración y Negocios" },
                new Categoria { Nombre = "Medicina" },
                new Categoria { Nombre = "Arquitectura" },
                new Categoria { Nombre = "Ingeniería Civil" }
            };

            context.Categorias.AddRange(categorias);
            context.SaveChanges();

           
            var random = new Random();
            var libros = new List<Libro>();

            for (int i = 1; i <= 500; i++)
            {
                var categoriaAleatoria = categorias[random.Next(categorias.Count)];

                libros.Add(new Libro
                {
                    Titulo = $"Libro {i} - {categoriaAleatoria.Nombre}",
                    Autor = $"Autor {random.Next(1, 200)}",
                    Stock = random.Next(1, 20),
                    CategoriaId = categoriaAleatoria.Id
                });
            }

            context.Libros.AddRange(libros);
            context.SaveChanges();
        }
    }
}
