using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Cita
    {
        public int NumCita { get; set; }
        public DateTime? FechaHoraCreacion { get; set; }
        public DateTime? FechaHoraCita { get; set; }
        public string? Motivo { get; set; }
        public string? CreadoPor { get; set; }
        public double? Precio { get; set; }
        public int? Dui { get; set; }

        public virtual Paciente? DuiNavigation { get; set; }
    }
}
