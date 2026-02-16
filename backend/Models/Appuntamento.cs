using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta un appuntamento tra paziente e medico
/// </summary>
public class Appuntamento
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PazienteId { get; set; }
    public virtual Paziente Paziente { get; set; } = null!;

    [Required]
    public int MedicoId { get; set; }
    public virtual Medico Medico { get; set; } = null!;

    [Required]
    public DateTime DataOra { get; set; }

    [Required]
    public int DurataMinuti { get; set; } = 30;

    [Required]
    [MaxLength(50)]
    public string Stato { get; set; } = "Programmato"; // Programmato, Confermato, Completato, Cancellato

    [MaxLength(500)]
    public string? Note { get; set; }

    [MaxLength(100)]
    public string? MotivoVisita { get; set; }

    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

    public DateTime? DataModifica { get; set; }

    // Relazione con referto (opzionale)
    public virtual Referto? Referto { get; set; }
}

/// <summary>
/// Enum per gli stati dell'appuntamento
/// </summary>
public static class StatoAppuntamento
{
    public const string Programmato = "Programmato";
    public const string Confermato = "Confermato";
    public const string Completato = "Completato";
    public const string Cancellato = "Cancellato";
}
