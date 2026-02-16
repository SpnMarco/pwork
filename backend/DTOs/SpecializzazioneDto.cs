namespace MedicalAppointments.DTOs;

/// <summary>
/// DTO per la specializzazione
/// </summary>
public class SpecializzazioneDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
}
