#nullable disable
using ApiSalvarVidas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiSalvarVidas.Data;

public partial class PaginaparaSalvarVidasContext
    : IdentityDbContext<AspNetUser, AspNetRole, string,
        IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public PaginaparaSalvarVidasContext(
        DbContextOptions<PaginaparaSalvarVidasContext> options)
        : base(options)
    {
    }

    // =========================
    // TABLAS DEL NEGOCIO
    // =========================

    public virtual DbSet<Animale>
        Animales
    { get; set; }

    public virtual DbSet<AnimalesComunitario>
        AnimalesComunitarios
    { get; set; }

    public virtual DbSet<AnimalesEnAdopcion>
        AnimalesEnAdopcions
    { get; set; }

    public virtual DbSet<AnimalesEnTransito>
        AnimalesEnTransitos
    { get; set; }

    public virtual DbSet<AnimalesPerdidosEncontrado>
        AnimalesPerdidosEncontrados
    { get; set; }

    public virtual DbSet<Familia>
        Familias
    { get; set; }

    public virtual DbSet<FiltrarAnimale>
        FiltrarAnimales
    { get; set; }

    // NUEVA TABLA
    public virtual DbSet<SolicitudesAdopcion>
        SolicitudesAdopcions
    { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CONFIGURACIÓN IDENTITY
        base.OnModelCreating(modelBuilder);

        // =========================
        // SOLICITUDES ADOPCIÓN
        // =========================

        modelBuilder.Entity<SolicitudesAdopcion>(entity =>
        {
            entity.ToTable("SolicitudesAdopcion");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Motivo)
                .HasMaxLength(500);

            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000);

            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("datetime");

            entity.Property(e => e.FechaEvaluacion)
                .HasColumnType("datetime");

            // RELACIÓN USUARIO SOLICITANTE
            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // RELACIÓN EVALUADOR
            entity.HasOne(e => e.EvaluadoPor)
                .WithMany()
                .HasForeignKey(e => e.EvaluadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            // RELACIÓN ANIMAL
            entity.HasOne(e => e.Animal)
                .WithMany()
                .HasForeignKey(e => e.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CONFIGURACIÓN EXTRA
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}