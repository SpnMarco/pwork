namespace MedicalAppointments.DTOs;

/// <summary>
/// DTO per la risposta con i dati dell'appuntamento
/// </summary>
public class AppuntamentoDto
{
    public int Id { get; set; }
    public int PazienteId { get; set; }
    public string PazienteNome { get; set; } = string.Empty;
    public string PazienteCognome { get; set; } = string.Empty;
    public int MedicoId { get; set; }
    public string MedicoNome { get; set; } = string.Empty;
    public string MedicoCognome { get; set; } = string.Empty;
    public string? Specializzazione { get; set; }
    public DateTime DataOra { get; set; }
    public int DurataMinuti { get; set; }
    public string Stato { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string? MotivoVisita { get; set; }
    public DateTime DataCreazione { get; set; }
}

/// <summary>
/// DTO per la creazione di un nuovo appuntamento
/// </summary>
public class CreateAppuntamentoDto
{
    public int PazienteId { get; set; }
    public int MedicoId { get; set; }
    public DateTime DataOra { get; set; }
    public int DurataMinuti { get; set; } = 30;
    public string? Note { get; set; }
    public string? MotivoVisita { get; set; }
}

/// <summary>
/// DTO per l'aggiornamento di un appuntamento
/// </summary>
public class UpdateAppuntamentoDto
{
    public DateTime DataOra { get; set; }
    public int DurataMinuti { get; set; }
    public string? Note { get; set; }
    public string? MotivoVisita { get; set; }
}

/// <summary>
/// DTO per uno slot orario disponibile
/// </summary>
public class SlotDisponibileDto
{
    public DateTime DataOra { get; set; }
    public bool Disponibile { get; set; }
    public string? Motivo { get; set; }
}

/// <summary>
/// DTO per richiedere gli slot disponibili
/// </summary>
public class RichiestaSlotDto
{
    public int MedicoId { get; set; }
    public DateTime Data { get; set; }
}
