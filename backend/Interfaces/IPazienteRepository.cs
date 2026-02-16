using MedicalAppointments.Models;

namespace MedicalAppointments.Interfaces;

/// <summary>
/// Repository specifico per Paziente
/// </summary>
public interface IPazienteRepository : IRepository<Paziente>
{
    /// <summary>
    /// Ottiene un paziente per codice fiscale
    /// </summary>
    Task<Paziente?> GetByCodiceFiscaleAsync(string codiceFiscale);

    /// <summary>
    /// Ottiene un paziente per email
    /// </summary>
    Task<Paziente?> GetByEmailAsync(string email);

    /// <summary>
    /// Ottiene pazienti con i loro appuntamenti
    /// </summary>
    Task<IEnumerable<Paziente>> GetPazientiConAppuntamentiAsync();

    /// <summary>
    /// Ottiene un paziente con tutti i suoi appuntamenti
    /// </summary>
    Task<Paziente?> GetPazienteConAppuntamentiAsync(int id);

    /// <summary>
    /// Ottiene un paziente con tutti i suoi referti
    /// </summary>
    Task<Paziente?> GetPazienteConRefertiAsync(int id);
}
