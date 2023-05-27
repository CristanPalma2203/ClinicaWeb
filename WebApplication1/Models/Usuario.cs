using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Movimientos = new HashSet<Movimiento>();
        }

        public string IdUsuario { get; set; } = null!;
        public string? NombreUsuario { get; set; }
        public string? ApellidoUsuario { get; set; }
        public string? CargoUsuario { get; set; }
        public string? ContraseñaUsuasio { get; set; }

        public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
