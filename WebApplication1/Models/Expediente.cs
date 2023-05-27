using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Expediente
    {
        public Expediente()
        {
            Diagnosticos = new HashSet<Diagnostico>();
            Movimientos = new HashSet<Movimiento>();
        }

        public int NumExpediente { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? AntecedentesClinicos { get; set; }
        public string? MedicamentosEscritos { get; set; }
        public string? TipoSangre { get; set; }
        public int? Dui { get; set; }

        public virtual Paciente? DuiNavigation { get; set; }
        public virtual ICollection<Diagnostico> Diagnosticos { get; set; }
        public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
