using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Repositories;

/// <summary>
/// Implementazione del repository per Medico
/// </summary>
public class MedicoRepository : Repository<Medico>, IMedicoRepository
{
    public MedicoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Medico?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(m => m.Specializzazione)
            .FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<Medico?> GetByNumeroAlboAsync(string numeroAlbo)
    {
        return await _dbSet
            .Include(m => m.Specializzazione)
            .FirstOrDefaultAsync(m => m.NumeroAlbo == numeroAlbo);
    }

    public async Task<IEnumerable<Medico>> GetBySpecializzazioneAsync(int specializzazioneId)
    {
        return await _dbSet
            .Include(m => m.Specializzazione)
            .Where(m => m.SpecializzazioneId == specializzazioneId && m.Attivo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Medico>> GetMediciAttiviAsync()
    {
        return await _dbSet
            .Include(m => m.Specializzazione)
            .Where(m => m.Attivo)
            .ToListAsync();
    }

    public async Task<Medico?> GetMedicoConDisponibilitaAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Specializzazione)
            .Include(m => m.Disponibilita)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Medico?> GetMedicoConAppuntamentiAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Specializzazione)
            .Include(m => m.Appuntamenti)
            .ThenInclude(a => a.Paziente)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}
