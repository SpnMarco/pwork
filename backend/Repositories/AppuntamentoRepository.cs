using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Repositories;

/// <summary>
/// Implementazione del repository per Appuntamento
/// </summary>
public class AppuntamentoRepository : Repository<Appuntamento>, IAppuntamentoRepository
{
    public AppuntamentoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Appuntamento>> GetByPazienteAsync(int pazienteId)
    {
        return await _dbSet
            .Include(a => a.Medico)
            .ThenInclude(m => m.Specializzazione)
            .Where(a => a.PazienteId == pazienteId)
            .OrderByDescending(a => a.DataOra)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appuntamento>> GetByMedicoAsync(int medicoId)
    {
        return await _dbSet
            .Include(a => a.Paziente)
            .Where(a => a.MedicoId == medicoId)
            .OrderBy(a => a.DataOra)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appuntamento>> GetByMedicoAndDataAsync(int medicoId, DateTime data)
    {
        var dataInizio = data.Date;
        var dataFine = data.Date.AddDays(1);

        return await _dbSet
            .Include(a => a.Paziente)
            .Where(a => a.MedicoId == medicoId 
                     && a.DataOra >= dataInizio 
                     && a.DataOra < dataFine
                     && a.Stato != StatoAppuntamento.Cancellato)
            .OrderBy(a => a.DataOra)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appuntamento>> GetByMedicoAndDateRangeAsync(int medicoId, DateTime dataInizio, DateTime dataFine)
    {
        return await _dbSet
            .Include(a => a.Paziente)
            .Where(a => a.MedicoId == medicoId 
                     && a.DataOra >= dataInizio 
                     && a.DataOra <= dataFine)
            .OrderBy(a => a.DataOra)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appuntamento>> GetByStatoAsync(string stato)
    {
        return await _dbSet
            .Include(a => a.Paziente)
            .Include(a => a.Medico)
            .Where(a => a.Stato == stato)
            .OrderBy(a => a.DataOra)
            .ToListAsync();
    }

    public async Task<bool> ExistsAppuntamentoAsync(int medicoId, DateTime dataOra)
    {
        return await _dbSet
            .AnyAsync(a => a.MedicoId == medicoId 
                        && a.DataOra == dataOra 
                        && a.Stato != StatoAppuntamento.Cancellato);
    }

    public async Task<Appuntamento?> GetAppuntamentoCompletoAsync(int id)
    {
        return await _dbSet
            .Include(a => a.Paziente)
            .Include(a => a.Medico)
            .ThenInclude(m => m.Specializzazione)
            .Include(a => a.Referto)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
