using MedicalAppointments.Models;

namespace MedicalAppointments.Interfaces;

/// <summary>
/// Repository specifico per Appuntamento
/// </summary>
public interface IAppuntamentoRepository : IRepository<Appuntamento>
{
    /// <summary>
    /// Ottiene appuntamenti per paziente
    /// </summary>
    Task<IEnumerable<Appuntamento>> GetByPazienteAsync(int pazienteId);

    /// <summary>
    /// Ottiene appuntamenti per medico
    /// </summary>
    Task<IEnumerable<Appuntamento>> GetByMedicoAsync(int medicoId);

    /// <summary>
    /// Ottiene appuntamenti per medico in una data specifica
    /// </summary>
    Task<IEnumerable<Appuntamento>> GetByMedicoAndDataAsync(int medicoId, DateTime data);

    /// <summary>
    /// Ottiene appuntamenti per medico in un range di date
    /// </summary>
    Task<IEnumerable<Appuntamento>> GetByMedicoAndDateRangeAsync(int medicoId, DateTime dataInizio, DateTime dataFine);

    /// <summary>
    /// Ottiene appuntamenti per stato
    /// </summary>
    Task<IEnumerable<Appuntamento>> GetByStatoAsync(string stato);

    /// <summary>
    /// Verifica se esiste un appuntamento per medico in un orario specifico
    /// </summary>
    Task<bool> ExistsAppuntamentoAsync(int medicoId, DateTime dataOra);

    /// <summary>
    /// Ottiene un appuntamento con tutte le relazioni
    /// </summary>
    Task<Appuntamento?> GetAppuntamentoCompletoAsync(int id);
}
