using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Paciente
    {
        public Paciente()
        {
            Cita = new HashSet<Cita>();
            Expedientes = new HashSet<Expediente>();
        }

        public int Dui { get; set; }
        public string? NombrePaciente { get; set; }
        public string? ApellidosPaciente { get; set; }
        public string? SexoPaciente { get; set; }
        public int? TelefonoPaciente { get; set; }
        public string? DireccionPaciente { get; set; }
        public string? EstadoCivil { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public virtual ICollection<Cita> Cita { get; set; }
        public virtual ICollection<Expediente> Expedientes { get; set; }
    }
}
