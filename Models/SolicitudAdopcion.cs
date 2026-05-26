#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSalvarVidas.Models;

public partial class SolicitudesAdopcion
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UsuarioId { get; set; } = "";

    [Required]
    public int AnimalId { get; set; }

    [Required]
    [StringLength(500)]
    public string Motivo { get; set; } = "";

    // 0 = Pendiente
    // 1 = EnRevision
    // 2 = Aprobada
    // 3 = Rechazada
    public int Estado { get; set; } = 0;

    [StringLength(1000)]
    public string? Observaciones { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public DateTime? FechaEvaluacion { get; set; }

    // =========================
    // FK CORRECTA
    // =========================
    public string? EvaluadoPorId { get; set; }

    // =========================
    // RELACIONES
    // =========================

    [ForeignKey(nameof(UsuarioId))]
    public virtual AspNetUser Usuario { get; set; }

    [ForeignKey(nameof(EvaluadoPorId))]
    public virtual AspNetUser EvaluadoPor { get; set; }

    [ForeignKey(nameof(AnimalId))]
    public virtual Animale Animal { get; set; }
}