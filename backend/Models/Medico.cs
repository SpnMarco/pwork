using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta un medico nel sistema
/// </summary>
public class Medico
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
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string NumeroAlbo { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Biografia { get; set; }

    public int? SpecializzazioneId { get; set; }
    public virtual Specializzazione? Specializzazione { get; set; }

    public bool Attivo { get; set; } = true;

    public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;

    // Relazioni
    public virtual ICollection<Appuntamento> Appuntamenti { get; set; } = new List<Appuntamento>();
    public virtual ICollection<Referto> Referti { get; set; } = new List<Referto>();
    public virtual ICollection<DisponibilitaMedico> Disponibilita { get; set; } = new List<DisponibilitaMedico>();
}
