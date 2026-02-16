using System.Linq.Expressions;

namespace MedicalAppointments.Interfaces;

/// <summary>
/// Interfaccia generica per il Repository Pattern
/// </summary>
/// <typeparam name="T">Tipo dell'entità</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Ottiene un'entità per ID
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Ottiene tutte le entità
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Trova entità che soddisfano un predicato
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Aggiunge una nuova entità
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Aggiunge multiple entità
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Aggiorna un'entità esistente
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Elimina un'entità
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Elimina multiple entità
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Verifica se un'entità esiste per ID
    /// </summary>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Conta le entità che soddisfano un predicato
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
}
