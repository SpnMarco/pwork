using System.ComponentModel.DataAnnotations;

namespace MedicalAppointments.Models;

/// <summary>
/// Rappresenta la disponibilità di un medico per un determinato giorno
/// </summary>
public class DisponibilitaMedico
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MedicoId { get; set; }
    public virtual Medico Medico { get; set; } = null!;

    [Required]
    public DayOfWeek GiornoSettimana { get; set; }

    [Required]
    public TimeSpan OraInizio { get; set; }

    [Required]
    public TimeSpan OraFine { get; set; }

    public bool Attivo { get; set; } = true;
}
