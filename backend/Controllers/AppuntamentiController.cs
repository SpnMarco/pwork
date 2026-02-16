using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalAppointments.DTOs;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AppuntamentiController : ControllerBase
{
    private readonly IAppuntamentoRepository _appuntamentoRepository;
    private readonly IAppuntamentoService _appuntamentoService;

    public AppuntamentiController(
        IAppuntamentoRepository appuntamentoRepository,
        IAppuntamentoService appuntamentoService)
    {
        _appuntamentoRepository = appuntamentoRepository;
        _appuntamentoService = appuntamentoService;
    }

    /// <summary>
    /// Ottiene la lista degli appuntamenti (filtrata per ruolo)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppuntamentoDto>>> GetAppuntamenti(
        [FromQuery] int? pazienteId = null,
        [FromQuery] int? medicoId = null,
        [FromQuery] string? stato = null,
        [FromQuery] DateTime? dataInizio = null,
        [FromQuery] DateTime? dataFine = null)
    {
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;
        var userMedicoId = User.FindFirst("MedicoId")?.Value;

        IEnumerable<Appuntamento> appuntamenti;

        // Filtra in base al ruolo
        if (userRole == "Paziente")
        {
            // Il paziente vede solo i propri appuntamenti
            var idPaziente = int.Parse(userPazienteId!);
            appuntamenti = await _appuntamentoRepository.GetByPazienteAsync(idPaziente);
        }
        else if (userRole == "Medico")
        {
            // Il medico vede solo i propri appuntamenti
            var idMedico = int.Parse(userMedicoId!);
            appuntamenti = await _appuntamentoRepository.GetByMedicoAsync(idMedico);
        }
        else
        {
            // Admin e Receptionist vedono tutti o possono filtrare
            if (pazienteId.HasValue)
            {
                appuntamenti = await _appuntamentoRepository.GetByPazienteAsync(pazienteId.Value);
            }
            else if (medicoId.HasValue)
            {
                appuntamenti = await _appuntamentoRepository.GetByMedicoAsync(medicoId.Value);
            }
            else if (dataInizio.HasValue && dataFine.HasValue && medicoId.HasValue)
            {
                appuntamenti = await _appuntamentoRepository.GetByMedicoAndDateRangeAsync(
                    medicoId.Value, dataInizio.Value, dataFine.Value);
            }
            else
            {
                appuntamenti = await _appuntamentoRepository.GetAllAsync();
            }
        }

        // Applica filtro stato se specificato
        if (!string.IsNullOrEmpty(stato))
        {
            appuntamenti = appuntamenti.Where(a => a.Stato == stato);
        }

        var appuntamentiDto = appuntamenti.Select(a => MapToAppuntamentoDto(a));

        return Ok(appuntamentiDto);
    }

    /// <summary>
    /// Ottiene un appuntamento per ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AppuntamentoDto>> GetAppuntamento(int id)
    {
        var appuntamento = await _appuntamentoRepository.GetAppuntamentoCompletoAsync(id);

        if (appuntamento == null)
        {
            return NotFound(new { message = "Appuntamento non trovato" });
        }

        // Verifica autorizzazione
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;
        var userMedicoId = User.FindFirst("MedicoId")?.Value;

        if (userRole == "Paziente" && userPazienteId != appuntamento.PazienteId.ToString())
        {
            return Forbid();
        }

        if (userRole == "Medico" && userMedicoId != appuntamento.MedicoId.ToString())
        {
            return Forbid();
        }

        return Ok(MapToAppuntamentoDto(appuntamento));
    }

    /// <summary>
    /// Crea un nuovo appuntamento
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AppuntamentoDto>> CreateAppuntamento([FromBody] CreateAppuntamentoDto dto)
    {
        try
        {
            // Se è un paziente, può prenotare solo per se stesso
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userPazienteId = User.FindFirst("PazienteId")?.Value;

            if (userRole == "Paziente" && userPazienteId != dto.PazienteId.ToString())
            {
                return Forbid();
            }

            var appuntamento = await _appuntamentoService.CreaAppuntamento(dto);

            return CreatedAtAction(
                nameof(GetAppuntamento),
                new { id = appuntamento.Id },
                MapToAppuntamentoDto(appuntamento));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Modifica un appuntamento esistente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<AppuntamentoDto>> UpdateAppuntamento(
        int id,
        [FromBody] UpdateAppuntamentoDto dto)
    {
        var appuntamento = await _appuntamentoRepository.GetByIdAsync(id);

        if (appuntamento == null)
        {
            return NotFound(new { message = "Appuntamento non trovato" });
        }

        // Verifica autorizzazione
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;

        if (userRole == "Paziente")
        {
            if (userPazienteId != appuntamento.PazienteId.ToString())
            {
                return Forbid();
            }

            // Il paziente può modificare solo se non è confermato
            if (appuntamento.Stato == StatoAppuntamento.Confermato ||
                appuntamento.Stato == StatoAppuntamento.Completato)
            {
                return BadRequest(new { message = "Non puoi modificare un appuntamento confermato o completato" });
            }
        }

        // Verifica disponibilità se cambia la data
        if (dto.DataOra != appuntamento.DataOra)
        {
            var isDisponibile = await _appuntamentoService.VerificaDisponibilita(
                appuntamento.MedicoId,
                dto.DataOra,
                dto.DurataMinuti);

            if (!isDisponibile)
            {
                return BadRequest(new { message = "Lo slot selezionato non è disponibile" });
            }
        }

        appuntamento.DataOra = dto.DataOra;
        appuntamento.DurataMinuti = dto.DurataMinuti;
        appuntamento.Note = dto.Note;
        appuntamento.MotivoVisita = dto.MotivoVisita;
        appuntamento.DataModifica = DateTime.UtcNow;

        await _appuntamentoRepository.UpdateAsync(appuntamento);

        return Ok(MapToAppuntamentoDto(appuntamento));
    }

    /// <summary>
    /// Cancella un appuntamento
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAppuntamento(int id)
    {
        var appuntamento = await _appuntamentoRepository.GetByIdAsync(id);

        if (appuntamento == null)
        {
            return NotFound(new { message = "Appuntamento non trovato" });
        }

        // Verifica autorizzazione
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;

        if (userRole == "Paziente" && userPazienteId != appuntamento.PazienteId.ToString())
        {
            return Forbid();
        }

        try
        {
            await _appuntamentoService.CancellaAppuntamento(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Conferma un appuntamento
    /// </summary>
    [HttpPatch("{id}/conferma")]
    [Authorize(Roles = "Admin,Receptionist,Medico")]
    public async Task<ActionResult> ConfermaAppuntamento(int id)
    {
        try
        {
            var success = await _appuntamentoService.ConfermaAppuntamento(id);

            if (!success)
            {
                return NotFound(new { message = "Appuntamento non trovato" });
            }

            return Ok(new { message = "Appuntamento confermato con successo" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Completa un appuntamento (dopo la visita)
    /// </summary>
    [HttpPatch("{id}/completa")]
    [Authorize(Roles = "Admin,Medico")]
    public async Task<ActionResult> CompletaAppuntamento(int id)
    {
        try
        {
            var success = await _appuntamentoService.CompletaAppuntamento(id);

            if (!success)
            {
                return NotFound(new { message = "Appuntamento non trovato" });
            }

            return Ok(new { message = "Appuntamento completato con successo" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Ottiene gli slot disponibili per un medico in una data specifica
    /// </summary>
    [HttpGet("disponibilita")]
    public async Task<ActionResult<IEnumerable<SlotDisponibileDto>>> GetDisponibilita(
        [FromQuery] int medicoId,
        [FromQuery] DateTime data)
    {
        if (medicoId <= 0)
        {
            return BadRequest(new { message = "MedicoId non valido" });
        }

        var slots = await _appuntamentoService.GetSlotsDisponibili(medicoId, data);

        return Ok(slots);
    }

    /// <summary>
    /// Ottiene gli appuntamenti dell'utente corrente
    /// </summary>
    [HttpGet("miei")]
    public async Task<ActionResult<IEnumerable<AppuntamentoDto>>> GetMieiAppuntamenti()
    {
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;
        var userMedicoId = User.FindFirst("MedicoId")?.Value;

        IEnumerable<Appuntamento> appuntamenti;

        if (userRole == "Paziente" && !string.IsNullOrEmpty(userPazienteId))
        {
            appuntamenti = await _appuntamentoRepository.GetByPazienteAsync(int.Parse(userPazienteId));
        }
        else if (userRole == "Medico" && !string.IsNullOrEmpty(userMedicoId))
        {
            appuntamenti = await _appuntamentoRepository.GetByMedicoAsync(int.Parse(userMedicoId));
        }
        else
        {
            return BadRequest(new { message = "Utente non valido" });
        }

        var appuntamentiDto = appuntamenti.Select(a => MapToAppuntamentoDto(a));

        return Ok(appuntamentiDto);
    }

    private static AppuntamentoDto MapToAppuntamentoDto(Appuntamento appuntamento)
    {
        return new AppuntamentoDto
        {
            Id = appuntamento.Id,
            PazienteId = appuntamento.PazienteId,
            PazienteNome = appuntamento.Paziente?.Nome ?? "",
            PazienteCognome = appuntamento.Paziente?.Cognome ?? "",
            MedicoId = appuntamento.MedicoId,
            MedicoNome = appuntamento.Medico?.Nome ?? "",
            MedicoCognome = appuntamento.Medico?.Cognome ?? "",
            Specializzazione = appuntamento.Medico?.Specializzazione?.Nome,
            DataOra = appuntamento.DataOra,
            DurataMinuti = appuntamento.DurataMinuti,
            Stato = appuntamento.Stato,
            Note = appuntamento.Note,
            MotivoVisita = appuntamento.MotivoVisita,
            DataCreazione = appuntamento.DataCreazione
        };
    }
}
