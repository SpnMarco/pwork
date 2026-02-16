using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta un paziente nel sistema
/// </summary>
public class Paziente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Cognome { get; set; } = string.Empty;

    [Required]
    [MaxLength(16)]
    public string CodiceFiscale { get; set; } = string.Empty;

    [Required]
    public DateTime DataNascita { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Indirizzo { get; set; }

    [MaxLength(100)]
    public string? Citta { get; set; }

    [MaxLength(10)]
    public string? CAP { get; set; }

    public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;

    // Relazioni
    public virtual ICollection<Appuntamento> Appuntamenti { get; set; } = new List<Appuntamento>();
    public virtual ICollection<Referto> Referti { get; set; } = new List<Referto>();
}
