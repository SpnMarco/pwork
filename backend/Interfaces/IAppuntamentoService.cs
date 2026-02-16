using MedicalAppointments.DTOs;
using MedicalAppointments.Models;

namespace MedicalAppointments.Interfaces;

/// <summary>
/// Interfaccia per il servizio di gestione appuntamenti
/// </summary>
public interface IAppuntamentoService
{
    /// <summary>
    /// Calcola gli slot disponibili per un medico in una data specifica
    /// </summary>
    Task<List<SlotDisponibileDto>> GetSlotsDisponibili(int medicoId, DateTime data);

    /// <summary>
    /// Verifica se uno slot è disponibile
    /// </summary>
    Task<bool> VerificaDisponibilita(int medicoId, DateTime dataOra, int durataMinuti);

    /// <summary>
    /// Crea un nuovo appuntamento con validazioni
    /// </summary>
    Task<Appuntamento> CreaAppuntamento(CreateAppuntamentoDto dto);

    /// <summary>
    /// Conferma un appuntamento
    /// </summary>
    Task<bool> ConfermaAppuntamento(int appuntamentoId);

    /// <summary>
    /// Completa un appuntamento (dopo la visita)
    /// </summary>
    Task<bool> CompletaAppuntamento(int appuntamentoId);

    /// <summary>
    /// Cancella un appuntamento
    /// </summary>
    Task<bool> CancellaAppuntamento(int appuntamentoId);

    /// <summary>
    /// Verifica se un appuntamento può essere modificato
    /// </summary>
    Task<bool> PuoModificare(int appuntamentoId, DateTime nuovaData);
}
