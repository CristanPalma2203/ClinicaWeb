using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Movimiento
    {
        public int CodMovimiento { get; set; }
        public string? TipoMovimiento { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public string? Detalle { get; set; }
        public string? IdUsuario { get; set; }
        public int? NumExpediente { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual Expediente? NumExpedienteNavigation { get; set; }
    }
}
