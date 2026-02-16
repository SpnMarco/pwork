using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;
using MedicalAppointments.DTOs;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Services;

/// <summary>
/// Servizio per la gestione della logica di business degli appuntamenti
/// </summary>
public class AppuntamentoService : IAppuntamentoService
{
    private readonly IAppuntamentoRepository _appuntamentoRepository;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPazienteRepository _pazienteRepository;
    private readonly ApplicationDbContext _context;

    public AppuntamentoService(
        IAppuntamentoRepository appuntamentoRepository,
        IMedicoRepository medicoRepository,
        IPazienteRepository pazienteRepository,
        ApplicationDbContext context)
    {
        _appuntamentoRepository = appuntamentoRepository;
        _medicoRepository = medicoRepository;
        _pazienteRepository = pazienteRepository;
        _context = context;
    }

    /// <summary>
    /// Calcola gli slot disponibili per un medico in una data specifica
    /// </summary>
    public async Task<List<SlotDisponibileDto>> GetSlotsDisponibili(int medicoId, DateTime data)
    {
        var slots = new List<SlotDisponibileDto>();

        // Verifica che il medico esista e sia attivo
        var medico = await _medicoRepository.GetMedicoConDisponibilitaAsync(medicoId);
        if (medico == null || !medico.Attivo)
        {
            return slots;
        }

        // Ottieni il giorno della settimana
        var giornoSettimana = data.DayOfWeek;

        // Trova le disponibilità del medico per quel giorno
        var disponibilita = medico.Disponibilita
            .Where(d => d.GiornoSettimana == giornoSettimana && d.Attivo)
            .ToList();

        if (!disponibilita.Any())
        {
            return slots;
        }

        // Ottieni gli appuntamenti già esistenti per quel medico in quella data
        var appuntamentiEsistenti = await _appuntamentoRepository.GetByMedicoAndDataAsync(medicoId, data);

        // Genera slot ogni 30 minuti per ogni fascia oraria disponibile
        foreach (var disp in disponibilita)
        {
            var oraCorrente = data.Date.Add(disp.OraInizio);
            var oraFine = data.Date.Add(disp.OraFine);

            while (oraCorrente < oraFine)
            {
                // Verifica se lo slot è già occupato
                var isOccupato = appuntamentiEsistenti.Any(a =>
                    a.DataOra <= oraCorrente &&
                    a.DataOra.AddMinutes(a.DurataMinuti) > oraCorrente &&
                    a.Stato != StatoAppuntamento.Cancellato);

                // Verifica se lo slot è nel passato
                var isPast = oraCorrente < DateTime.Now;

                slots.Add(new SlotDisponibileDto
                {
                    DataOra = oraCorrente,
                    Disponibile = !isOccupato && !isPast,
                    Motivo = isOccupato ? "Occupato" : (isPast ? "Passato" : null)
                });

                oraCorrente = oraCorrente.AddMinutes(30);
            }
        }

        return slots.OrderBy(s => s.DataOra).ToList();
    }

    /// <summary>
    /// Verifica se uno slot è disponibile
    /// </summary>
    public async Task<bool> VerificaDisponibilita(int medicoId, DateTime dataOra, int durataMinuti)
    {
        // Verifica che la data sia futura
        if (dataOra < DateTime.Now)
        {
            return false;
        }

        // Verifica che il medico esista e sia attivo
        var medico = await _medicoRepository.GetMedicoConDisponibilitaAsync(medicoId);
        if (medico == null || !medico.Attivo)
        {
            return false;
        }

        // Verifica che il medico abbia disponibilità per quel giorno e orario
        var giornoSettimana = dataOra.DayOfWeek;
        var oraAppuntamento = dataOra.TimeOfDay;

        var haDisponibilita = medico.Disponibilita.Any(d =>
            d.GiornoSettimana == giornoSettimana &&
            d.Attivo &&
            d.OraInizio <= oraAppuntamento &&
            d.OraFine >= oraAppuntamento.Add(TimeSpan.FromMinutes(durataMinuti)));

        if (!haDisponibilita)
        {
            return false;
        }

        // Verifica che non ci siano sovrapposizioni con altri appuntamenti
        var appuntamentiEsistenti = await _appuntamentoRepository.GetByMedicoAndDataAsync(medicoId, dataOra.Date);

        var haConflitto = appuntamentiEsistenti.Any(a =>
        {
            if (a.Stato == StatoAppuntamento.Cancellato)
                return false;

            var fineEsistente = a.DataOra.AddMinutes(a.DurataMinuti);
            var fineNuovo = dataOra.AddMinutes(durataMinuti);

            // Verifica sovrapposizione
            return (dataOra >= a.DataOra && dataOra < fineEsistente) ||
                   (fineNuovo > a.DataOra && fineNuovo <= fineEsistente) ||
                   (dataOra <= a.DataOra && fineNuovo >= fineEsistente);
        });

        return !haConflitto;
    }

    /// <summary>
    /// Crea un nuovo appuntamento con validazioni
    /// </summary>
    public async Task<Appuntamento> CreaAppuntamento(CreateAppuntamentoDto dto)
    {
        // Verifica che il paziente esista
        var paziente = await _pazienteRepository.GetByIdAsync(dto.PazienteId);
        if (paziente == null)
        {
            throw new InvalidOperationException("Paziente non trovato");
        }

        // Verifica che il medico esista
        var medico = await _medicoRepository.GetByIdAsync(dto.MedicoId);
        if (medico == null)
        {
            throw new InvalidOperationException("Medico non trovato");
        }

        if (!medico.Attivo)
        {
            throw new InvalidOperationException("Il medico non è attivo");
        }

        // Verifica disponibilità
        var isDisponibile = await VerificaDisponibilita(dto.MedicoId, dto.DataOra, dto.DurataMinuti);
        if (!isDisponibile)
        {
            throw new InvalidOperationException("Lo slot selezionato non è disponibile");
        }

        // Crea l'appuntamento
        var appuntamento = new Appuntamento
        {
            PazienteId = dto.PazienteId,
            MedicoId = dto.MedicoId,
            DataOra = dto.DataOra,
            DurataMinuti = dto.DurataMinuti,
            Stato = StatoAppuntamento.Programmato,
            Note = dto.Note,
            MotivoVisita = dto.MotivoVisita,
            DataCreazione = DateTime.UtcNow
        };

        await _appuntamentoRepository.AddAsync(appuntamento);

        return appuntamento;
    }

    /// <summary>
    /// Conferma un appuntamento
    /// </summary>
    public async Task<bool> ConfermaAppuntamento(int appuntamentoId)
    {
        var appuntamento = await _appuntamentoRepository.GetByIdAsync(appuntamentoId);
        if (appuntamento == null)
        {
            return false;
        }

        if (appuntamento.Stato != StatoAppuntamento.Programmato)
        {
            throw new InvalidOperationException("L'appuntamento non può essere confermato nel suo stato attuale");
        }

        appuntamento.Stato = StatoAppuntamento.Confermato;
        appuntamento.DataModifica = DateTime.UtcNow;

        await _appuntamentoRepository.UpdateAsync(appuntamento);

        return true;
    }

    /// <summary>
    /// Completa un appuntamento (dopo la visita)
    /// </summary>
    public async Task<bool> CompletaAppuntamento(int appuntamentoId)
    {
        var appuntamento = await _appuntamentoRepository.GetByIdAsync(appuntamentoId);
        if (appuntamento == null)
        {
            return false;
        }

        if (appuntamento.Stato == StatoAppuntamento.Cancellato)
        {
            throw new InvalidOperationException("Non è possibile completare un appuntamento cancellato");
        }

        appuntamento.Stato = StatoAppuntamento.Completato;
        appuntamento.DataModifica = DateTime.UtcNow;

        await _appuntamentoRepository.UpdateAsync(appuntamento);

        return true;
    }

    /// <summary>
    /// Cancella un appuntamento
    /// </summary>
    public async Task<bool> CancellaAppuntamento(int appuntamentoId)
    {
        var appuntamento = await _appuntamentoRepository.GetByIdAsync(appuntamentoId);
        if (appuntamento == null)
        {
            return false;
        }

        if (appuntamento.Stato == StatoAppuntamento.Completato)
        {
            throw new InvalidOperationException("Non è possibile cancellare un appuntamento già completato");
        }

        appuntamento.Stato = StatoAppuntamento.Cancellato;
        appuntamento.DataModifica = DateTime.UtcNow;

        await _appuntamentoRepository.UpdateAsync(appuntamento);

        return true;
    }

    /// <summary>
    /// Verifica se un appuntamento può essere modificato
    /// </summary>
    public async Task<bool> PuoModificare(int appuntamentoId, DateTime nuovaData)
    {
        var appuntamento = await _appuntamentoRepository.GetByIdAsync(appuntamentoId);
        if (appuntamento == null)
        {
            return false;
        }

        // Non si possono modificare appuntamenti completati o cancellati
        if (appuntamento.Stato == StatoAppuntamento.Completato ||
            appuntamento.Stato == StatoAppuntamento.Cancellato)
        {
            return false;
        }

        // Verifica che la nuova data sia disponibile
        return await VerificaDisponibilita(appuntamento.MedicoId, nuovaData, appuntamento.DurataMinuti);
    }
}
