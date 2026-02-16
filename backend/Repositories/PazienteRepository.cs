using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Repositories;

/// <summary>
/// Implementazione del repository per Paziente
/// </summary>
public class PazienteRepository : Repository<Paziente>, IPazienteRepository
{
    public PazienteRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Paziente?> GetByCodiceFiscaleAsync(string codiceFiscale)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.CodiceFiscale == codiceFiscale);
    }

    public async Task<Paziente?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task<IEnumerable<Paziente>> GetPazientiConAppuntamentiAsync()
    {
        return await _dbSet
            .Include(p => p.Appuntamenti)
            .ThenInclude(a => a.Medico)
            .ToListAsync();
    }

    public async Task<Paziente?> GetPazienteConAppuntamentiAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Appuntamenti)
            .ThenInclude(a => a.Medico)
            .ThenInclude(m => m.Specializzazione)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Paziente?> GetPazienteConRefertiAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Referti)
            .ThenInclude(r => r.Medico)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
