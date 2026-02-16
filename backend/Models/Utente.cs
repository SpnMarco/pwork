using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta un utente del sistema per l'autenticazione
/// </summary>
public class Utente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Ruolo { get; set; } = RuoloUtente.Paziente; // Admin, Medico, Paziente, Receptionist

    public int? PazienteId { get; set; }
    public virtual Paziente? Paziente { get; set; }

    public int? MedicoId { get; set; }
    public virtual Medico? Medico { get; set; }

    public bool Attivo { get; set; } = true;

    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

    public DateTime? UltimoAccesso { get; set; }
}

/// <summary>
/// Enum per i ruoli utente
/// </summary>
public static class RuoloUtente
{
    public const string Admin = "Admin";
    public const string Medico = "Medico";
    public const string Paziente = "Paziente";
    public const string Receptionist = "Receptionist";
}
