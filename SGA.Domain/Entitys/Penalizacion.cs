using SGA.Domain.Enums;
using System;

namespace SGA.Domain.Entitys
{
    public class Penalizacion
    {
        public const decimal MontoBasePorDia = 5.00m;
        public const decimal MontoMinimo = 10.00m;
        public const decimal MontoMaximo = 500.00m;

        public int Id { get; set; }

        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int DiasRetraso { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaPago { get; set; }
        public EstadoPenalizacion Estado { get; set; }

        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public int? PrestamoId { get; set; }
        public Prestamo? Prestamo { get; set; }

        public bool EstaActiva() => Estado == EstadoPenalizacion.Activa || Estado == EstadoPenalizacion.Pendiente;

        public bool EstaPagada() => Estado == EstadoPenalizacion.Pagada;

        public static decimal CalcularMontoPenalizacion(int diasRetraso, decimal montoPorDia = MontoBasePorDia)
        {
            if (diasRetraso <= 0)
                return 0;

            decimal monto = diasRetraso * montoPorDia;

            if (monto < MontoMinimo)
                monto = MontoMinimo;

            if (monto > MontoMaximo)
                monto = MontoMaximo;

            return Math.Round(monto, 2);
        }

        public void GenerarPenalizacion(int diasRetraso, string motivo = "")
        {
            DiasRetraso = diasRetraso;
            Monto = CalcularMontoPenalizacion(diasRetraso);
            FechaGeneracion = DateTime.Now;
            Estado = EstadoPenalizacion.Pendiente;
            Motivo = string.IsNullOrEmpty(motivo) 
                ? $"Penalización por {diasRetraso} días de retraso" 
                : motivo;
        }

        public (bool exito, string mensaje) MarcarPagada()
        {
            if (Estado == EstadoPenalizacion.Pagada)
                return (false, "Esta penalización ya fue pagada.");

            Estado = EstadoPenalizacion.Pagada;
            FechaPago = DateTime.Now;
            return (true, $"Penalización de {Monto:C} pagada exitosamente.");
        }

        public (bool exito, string mensaje) Activar()
        {
            if (Estado == EstadoPenalizacion.Pagada)
                return (false, "No se puede activar una penalización ya pagada.");

            Estado = EstadoPenalizacion.Activa;
            return (true, "Penalización activada.");
        }

        public int ObtenerDiasActiva()
        {
            if (FechaPago.HasValue)
                return (FechaPago.Value - FechaGeneracion).Days;
            
            if (Estado != EstadoPenalizacion.Pagada)
                return (DateTime.Now - FechaGeneracion).Days;

            return 0;
        }

        public string ObtenerInformacion()
        {
            return $"Monto: {Monto:C} - Días de retraso: {DiasRetraso} - Estado: {Estado}";
        }

        public string ObtenerNombreUsuario()
        {
            if (Estudiante != null)
                return Estudiante.Nombre;
            if (Docente != null)
                return Docente.Nombre;
            return "Desconocido";
        }
    }
}
