using SGA.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entitys
{
    public class Prestamo
    {
        public const int MaximoRenovaciones = 2;
        public const int DiasVencimiento = 7;

        public int Id { get; set; }

        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public int Renovaciones { get; set; }
        public EstadoPrestamo Estado { get; set; }

        public int LibroId { get; set; }
        public Libro? Libro { get; set; }

        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public ICollection<Penalizacion>? Penalizaciones { get; set; }

        public bool EstaActivo() => Estado == EstadoPrestamo.Activo;

        public bool EstaVencido()
        {
            if (Estado != EstadoPrestamo.Activo)
                return false;
            return DateTime.Now > FechaLimite;
        }

        public bool PuedeRenovarse()
        {
            if (Estado != EstadoPrestamo.Activo)
                return false;

            if (Renovaciones >= MaximoRenovaciones)
                return false;

            if (EstaVencido())
                return false;

            return true;
        }

        public (bool exito, string mensaje) Renovar(int diasAdicionales = DiasVencimiento)
        {
            if (!PuedeRenovarse())
            {
                if (Renovaciones >= MaximoRenovaciones)
                    return (false, $"No se puede renovar. Se alcanzó el límite de {MaximoRenovaciones} renovaciones.");
                
                if (EstaVencido())
                    return (false, "No se puede renovar un préstamo vencido. Primero debe devolver el libro.");
                
                return (false, "No se puede renovar este préstamo.");
            }

            Renovaciones++;
            FechaLimite = FechaLimite.AddDays(diasAdicionales);
            return (true, $"Préstamo renovado exitosamente. Nueva fecha límite: {FechaLimite:dd/MM/yyyy}");
        }

        public (bool exito, string mensaje) Devolver()
        {
            if (Estado == EstadoPrestamo.Devuelto)
                return (false, "Este préstamo ya fue devuelto.");

            Estado = EstadoPrestamo.Devuelto;
            FechaDevolucionReal = DateTime.Now;

            if (Libro != null)
            {
                Libro.Devolver();
            }

            return (true, $"Libro devuelto exitosamente el {FechaDevolucionReal:dd/MM/yyyy}");
        }

        public int CalcularDiasRetraso()
        {
            if (Estado == EstadoPrestamo.Devuelto && FechaDevolucionReal.HasValue)
            {
                if (FechaDevolucionReal.Value > FechaLimite)
                    return (FechaDevolucionReal.Value - FechaLimite).Days;
                return 0;
            }

            if (EstaVencido())
                return (DateTime.Now - FechaLimite).Days;

            return 0;
        }

        public int ObtenerDiasRestantes()
        {
            if (Estado != EstadoPrestamo.Activo)
                return 0;

            if (FechaLimite < DateTime.Now)
                return 0;

            return (FechaLimite - DateTime.Now).Days;
        }

        public TimeSpan ObtenerTiempoRestante()
        {
            if (Estado != EstadoPrestamo.Activo)
                return TimeSpan.Zero;

            if (FechaLimite < DateTime.Now)
                return TimeSpan.Zero;

            return FechaLimite - DateTime.Now;
        }

        public void MarcarVencido()
        {
            if (Estado == EstadoPrestamo.Activo && EstaVencido())
            {
                Estado = EstadoPrestamo.Vencido;
            }
        }

        public decimal CalcularPenalizacionEstimada(decimal montoPorDia = 5.00m)
        {
            int diasRetraso = CalcularDiasRetraso();
            if (diasRetraso <= 0)
                return 0;

            return diasRetraso * montoPorDia;
        }

        public bool TienePenalizacionesActivas()
        {
            return Penalizaciones?.Any(p => p.Estado == EstadoPenalizacion.Activa || p.Estado == EstadoPenalizacion.Pendiente) ?? false;
        }

        public string ObtenerEstadoActual()
        {
            if (Estado == EstadoPrestamo.Devuelto)
                return $"Devuelto el {FechaDevolucionReal:dd/MM/yyyy}";

            if (Estado == EstadoPrestamo.Vencido)
                return $"Vencido hace {CalcularDiasRetraso()} días";

            if (Estado == EstadoPrestamo.Activo)
            {
                int diasRestantes = ObtenerDiasRestantes();
                if (diasRestantes > 0)
                    return $"Activo - {diasRestantes} días restantes";
                if (diasRestantes == 0)
                    return "Vence hoy";
                return $"Vencido hace {Math.Abs(diasRestantes)} días";
            }

            return Estado.ToString();
        }

        public void Crear(DateTime fechaLimite)
        {
            FechaPrestamo = DateTime.Now;
            FechaLimite = fechaLimite;
            Estado = EstadoPrestamo.Activo;
            Renovaciones = 0;
        }
    }
}
