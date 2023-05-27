using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Diagnostico
    {
        public int IdDiagnostico { get; set; }
        public string? Estado { get; set; }
        public string? Enfermedad { get; set; }
        public double? Peso { get; set; }
        public double? Estatura { get; set; }
        public double? Presion { get; set; }
        public DateTime? FechaDiagnostico { get; set; }
        public double? Temperatura { get; set; }
        public string? Detalles { get; set; }
        public string? Recomendaciones { get; set; }
        public int? NumExpediente { get; set; }

        public virtual Expediente? NumExpedienteNavigation { get; set; }
    }
}
