using MedicalAppointments.Models;

namespace MedicalAppointments.Interfaces;

/// <summary>
/// Repository specifico per Medico
/// </summary>
public interface IMedicoRepository : IRepository<Medico>
{
    /// <summary>
    /// Ottiene un medico per email
    /// </summary>
    Task<Medico?> GetByEmailAsync(string email);

    /// <summary>
    /// Ottiene un medico per numero albo
    /// </summary>
    Task<Medico?> GetByNumeroAlboAsync(string numeroAlbo);

    /// <summary>
    /// Ottiene medici per specializzazione
    /// </summary>
    Task<IEnumerable<Medico>> GetBySpecializzazioneAsync(int specializzazioneId);

    /// <summary>
    /// Ottiene medici attivi
    /// </summary>
    Task<IEnumerable<Medico>> GetMediciAttiviAsync();

    /// <summary>
    /// Ottiene un medico con le sue disponibilità
    /// </summary>
    Task<Medico?> GetMedicoConDisponibilitaAsync(int id);

    /// <summary>
    /// Ottiene un medico con i suoi appuntamenti
    /// </summary>
    Task<Medico?> GetMedicoConAppuntamentiAsync(int id);
}
