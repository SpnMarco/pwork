using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta una specializzazione medica
/// </summary>
public class Specializzazione
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descrizione { get; set; }

    // Relazioni
    public virtual ICollection<Medico> Medici { get; set; } = new List<Medico>();
}
