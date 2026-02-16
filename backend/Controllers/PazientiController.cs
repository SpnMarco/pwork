using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalAppointments.DTOs;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PazientiController : ControllerBase
{
    private readonly IPazienteRepository _pazienteRepository;

    public PazientiController(IPazienteRepository pazienteRepository)
    {
        _pazienteRepository = pazienteRepository;
    }

    /// <summary>
    /// Ottiene la lista di tutti i pazienti
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<IEnumerable<PazienteDto>>> GetPazienti()
    {
        var pazienti = await _pazienteRepository.GetAllAsync();
        var pazientiDto = pazienti.Select(p => MapToPazienteDto(p));
        return Ok(pazientiDto);
    }

    /// <summary>
    /// Ottiene un paziente per ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PazienteDto>> GetPaziente(int id)
    {
        var paziente = await _pazienteRepository.GetByIdAsync(id);

        if (paziente == null)
        {
            return NotFound(new { message = "Paziente non trovato" });
        }

        // Verifica autorizzazione: Admin, Receptionist o il paziente stesso
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;

        if (userRole != "Admin" && userRole != "Receptionist" && userPazienteId != id.ToString())
        {
            return Forbid();
        }

        return Ok(MapToPazienteDto(paziente));
    }

    /// <summary>
    /// Crea un nuovo paziente
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<PazienteDto>> CreatePaziente([FromBody] CreatePazienteDto dto)
    {
        // Verifica se codice fiscale già esiste
        var existingCF = await _pazienteRepository.GetByCodiceFiscaleAsync(dto.CodiceFiscale);
        if (existingCF != null)
        {
            return BadRequest(new { message = "Codice fiscale già esistente" });
        }

        // Verifica se email già esiste
        var existingEmail = await _pazienteRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
        {
            return BadRequest(new { message = "Email già registrata" });
        }

        var paziente = new Paziente
        {
            Nome = dto.Nome,
            Cognome = dto.Cognome,
            CodiceFiscale = dto.CodiceFiscale,
            DataNascita = dto.DataNascita,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Indirizzo = dto.Indirizzo,
            Citta = dto.Citta,
            CAP = dto.CAP
        };

        await _pazienteRepository.AddAsync(paziente);

        return CreatedAtAction(nameof(GetPaziente), new { id = paziente.Id }, MapToPazienteDto(paziente));
    }

    /// <summary>
    /// Aggiorna un paziente esistente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PazienteDto>> UpdatePaziente(int id, [FromBody] UpdatePazienteDto dto)
    {
        var paziente = await _pazienteRepository.GetByIdAsync(id);

        if (paziente == null)
        {
            return NotFound(new { message = "Paziente non trovato" });
        }

        // Verifica autorizzazione
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;

        if (userRole != "Admin" && userRole != "Receptionist" && userPazienteId != id.ToString())
        {
            return Forbid();
        }

        // Verifica se email già esiste per altro paziente
        var existingEmail = await _pazienteRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null && existingEmail.Id != id)
        {
            return BadRequest(new { message = "Email già registrata per un altro paziente" });
        }

        paziente.Nome = dto.Nome;
        paziente.Cognome = dto.Cognome;
        paziente.Email = dto.Email;
        paziente.Telefono = dto.Telefono;
        paziente.Indirizzo = dto.Indirizzo;
        paziente.Citta = dto.Citta;
        paziente.CAP = dto.CAP;

        await _pazienteRepository.UpdateAsync(paziente);

        return Ok(MapToPazienteDto(paziente));
    }

    /// <summary>
    /// Elimina un paziente
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeletePaziente(int id)
    {
        var paziente = await _pazienteRepository.GetByIdAsync(id);

        if (paziente == null)
        {
            return NotFound(new { message = "Paziente non trovato" });
        }

        await _pazienteRepository.DeleteAsync(paziente);

        return NoContent();
    }

    /// <summary>
    /// Ottiene gli appuntamenti di un paziente
    /// </summary>
    [HttpGet("{id}/appuntamenti")]
    public async Task<ActionResult<IEnumerable<AppuntamentoDto>>> GetAppuntamentiPaziente(int id)
    {
        var paziente = await _pazienteRepository.GetPazienteConAppuntamentiAsync(id);

        if (paziente == null)
        {
            return NotFound(new { message = "Paziente non trovato" });
        }

        // Verifica autorizzazione
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userPazienteId = User.FindFirst("PazienteId")?.Value;

        if (userRole != "Admin" && userRole != "Receptionist" && userPazienteId != id.ToString())
        {
            return Forbid();
        }

        var appuntamentiDto = paziente.Appuntamenti.Select(a => new AppuntamentoDto
        {
            Id = a.Id,
            PazienteId = a.PazienteId,
            PazienteNome = paziente.Nome,
            PazienteCognome = paziente.Cognome,
            MedicoId = a.MedicoId,
            MedicoNome = a.Medico.Nome,
            MedicoCognome = a.Medico.Cognome,
            Specializzazione = a.Medico.Specializzazione?.Nome,
            DataOra = a.DataOra,
            DurataMinuti = a.DurataMinuti,
            Stato = a.Stato,
            Note = a.Note,
            MotivoVisita = a.MotivoVisita,
            DataCreazione = a.DataCreazione
        });

        return Ok(appuntamentiDto);
    }

    private static PazienteDto MapToPazienteDto(Paziente paziente)
    {
        return new PazienteDto
        {
            Id = paziente.Id,
            Nome = paziente.Nome,
            Cognome = paziente.Cognome,
            CodiceFiscale = paziente.CodiceFiscale,
            DataNascita = paziente.DataNascita,
            Email = paziente.Email,
            Telefono = paziente.Telefono,
            Indirizzo = paziente.Indirizzo,
            Citta = paziente.Citta,
            CAP = paziente.CAP,
            DataRegistrazione = paziente.DataRegistrazione
        };
    }
}
