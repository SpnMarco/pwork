using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta un referto medico
/// </summary>
public class Referto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AppuntamentoId { get; set; }
    public virtual Appuntamento Appuntamento { get; set; } = null!;

    [Required]
    public int PazienteId { get; set; }
    public virtual Paziente Paziente { get; set; } = null!;

    [Required]
    public int MedicoId { get; set; }
    public virtual Medico Medico { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Titolo { get; set; } = string.Empty;

    [Required]
    public string Descrizione { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Diagnosi { get; set; }

    [MaxLength(1000)]
    public string? Terapia { get; set; }

    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

    public DateTime? DataModifica { get; set; }

    [MaxLength(255)]
    public string? FileAllegato { get; set; } // Path al file PDF se presente
}
