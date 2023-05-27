using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Models
{
    public partial class SistemaClinicoContext : DbContext
    {
        public SistemaClinicoContext()
        {
        }

        public SistemaClinicoContext(DbContextOptions<SistemaClinicoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cita> Citas { get; set; } = null!;
        public virtual DbSet<Diagnostico> Diagnosticos { get; set; } = null!;
        public virtual DbSet<Expediente> Expedientes { get; set; } = null!;
        public virtual DbSet<Movimiento> Movimientos { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               // optionsBuilder.UseSqlServer("Data Source=3.228.164.208;Initial Catalog=SistemaClinico;User ID=sa;Password=SapiAdmin2020");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.NumCita)
                    .HasName("PK__Citas__15B91097EE1CA4FC");

                entity.HasIndex(e => e.FechaHoraCita, "UQ__Citas__1A692E1D84B2D86A")
                    .IsUnique();

                entity.Property(e => e.NumCita).HasColumnName("num_cita");

                entity.Property(e => e.CreadoPor).HasMaxLength(100);

                entity.Property(e => e.Dui).HasColumnName("dui");

                entity.Property(e => e.FechaHoraCita)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_HoraCita");

                entity.Property(e => e.FechaHoraCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_HoraCreacion");

                entity.Property(e => e.Motivo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.DuiNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Dui)
                    .HasConstraintName("FK__Citas__dui__412EB0B6");
            });

            modelBuilder.Entity<Diagnostico>(entity =>
            {
                entity.HasKey(e => e.IdDiagnostico)
                    .HasName("PK__Diagnost__E38ACD1D044E37C8");

                entity.ToTable("Diagnostico");

                entity.Property(e => e.IdDiagnostico).HasColumnName("Id_Diagnostico");

                entity.Property(e => e.Detalles)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("detalles");

                entity.Property(e => e.Enfermedad).HasMaxLength(250);

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Estatura).HasColumnName("estatura");

                entity.Property(e => e.FechaDiagnostico)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaDiagnostico");

                entity.Property(e => e.NumExpediente).HasColumnName("Num_Expediente");

                entity.Property(e => e.Presion).HasColumnName("presion");

                entity.Property(e => e.Recomendaciones)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("recomendaciones");

                entity.Property(e => e.Temperatura).HasColumnName("temperatura");

                entity.HasOne(d => d.NumExpedienteNavigation)
                    .WithMany(p => p.Diagnosticos)
                    .HasForeignKey(d => d.NumExpediente)
                    .HasConstraintName("FK__Diagnosti__Num_E__3D5E1FD2");
            });

            modelBuilder.Entity<Expediente>(entity =>
            {
                entity.HasKey(e => e.NumExpediente)
                    .HasName("PK__Expedien__6B6DC88AFDF0F25C");

                entity.ToTable("Expediente");

                entity.Property(e => e.NumExpediente).HasColumnName("Num_Expediente");

                entity.Property(e => e.AntecedentesClinicos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Antecedentes_Clinicos");

                entity.Property(e => e.Dui).HasColumnName("dui");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.MedicamentosEscritos)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Medicamentos_Escritos");

                entity.Property(e => e.TipoSangre)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("tipo_sangre");

                entity.HasOne(d => d.DuiNavigation)
                    .WithMany(p => p.Expedientes)
                    .HasForeignKey(d => d.Dui)
                    .HasConstraintName("FK__Expediente__dui__3A81B327");
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(e => e.CodMovimiento)
                    .HasName("PK__Movimien__03BD3CFD8934AF6C");

                entity.ToTable("Movimiento");

                entity.Property(e => e.CodMovimiento).HasColumnName("Cod_Movimiento");

                entity.Property(e => e.Detalle)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FechaMovimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Movimiento");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("Id_Usuario");

                entity.Property(e => e.NumExpediente).HasColumnName("Num_Expediente");

                entity.Property(e => e.TipoMovimiento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Tipo_Movimiento");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Movimient__Id_Us__440B1D61");

                entity.HasOne(d => d.NumExpedienteNavigation)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.NumExpediente)
                    .HasConstraintName("FK__Movimient__Num_E__44FF419A");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.Dui)
                    .HasName("PK__Paciente__C0317D9029ED2032");

                entity.ToTable("Paciente");

                entity.Property(e => e.Dui).ValueGeneratedNever();

                entity.Property(e => e.ApellidosPaciente)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Apellidos_Paciente");

                entity.Property(e => e.DireccionPaciente)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Direccion_Paciente");

                entity.Property(e => e.EstadoCivil).HasMaxLength(20);

                entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");

                entity.Property(e => e.NombrePaciente)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Paciente");

                entity.Property(e => e.SexoPaciente)
                    .HasMaxLength(1)
                    .HasColumnName("Sexo_Paciente");

                entity.Property(e => e.TelefonoPaciente).HasColumnName("Telefono_Paciente");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__63C76BE227CB042B");

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("Id_Usuario");

                entity.Property(e => e.ApellidoUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Apellido_Usuario");

                entity.Property(e => e.CargoUsuario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Cargo_Usuario");

                entity.Property(e => e.ContraseñaUsuasio)
                    .HasMaxLength(10)
                    .HasColumnName("Contraseña_Usuasio");

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Usuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
