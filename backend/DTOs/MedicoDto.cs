namespace MedicalAppointments.DTOs;

/// <summary>
/// DTO per la risposta con i dati del medico
/// </summary>
public class MedicoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string NumeroAlbo { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public int? SpecializzazioneId { get; set; }
    public string? Specializzazione { get; set; }
    public bool Attivo { get; set; }
}

/// <summary>
/// DTO per la creazione di un nuovo medico
/// </summary>
public class CreateMedicoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string NumeroAlbo { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public int? SpecializzazioneId { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di un medico
/// </summary>
public class UpdateMedicoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public int? SpecializzazioneId { get; set; }
    public bool Attivo { get; set; }
}

/// <summary>
/// DTO per la disponibilità del medico
/// </summary>
public class DisponibilitaDto
{
    public int Id { get; set; }
    public int MedicoId { get; set; }
    public DayOfWeek GiornoSettimana { get; set; }
    public TimeSpan OraInizio { get; set; }
    public TimeSpan OraFine { get; set; }
    public bool Attivo { get; set; }
}

/// <summary>
/// DTO per creare una disponibilità
/// </summary>
public class CreateDisponibilitaDto
{
    public DayOfWeek GiornoSettimana { get; set; }
    public TimeSpan OraInizio { get; set; }
    public TimeSpan OraFine { get; set; }
}
