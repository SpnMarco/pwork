using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalAppointments.DTOs;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MediciController : ControllerBase
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly IRepository<DisponibilitaMedico> _disponibilitaRepository;

    public MediciController(
        IMedicoRepository medicoRepository,
        IRepository<DisponibilitaMedico> disponibilitaRepository)
    {
        _medicoRepository = medicoRepository;
        _disponibilitaRepository = disponibilitaRepository;
    }

    /// <summary>
    /// Ottiene la lista di tutti i medici attivi
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicoDto>>> GetMedici()
    {
        var medici = await _medicoRepository.GetMediciAttiviAsync();
        var mediciDto = medici.Select(m => MapToMedicoDto(m));
        return Ok(mediciDto);
    }

    /// <summary>
    /// Ottiene un medico per ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicoDto>> GetMedico(int id)
    {
        var medico = await _medicoRepository.GetByIdAsync(id);

        if (medico == null)
        {
            return NotFound(new { message = "Medico non trovato" });
        }

        return Ok(MapToMedicoDto(medico));
    }

    /// <summary>
    /// Ottiene medici per specializzazione
    /// </summary>
    [HttpGet("specializzazione/{specializzazioneId}")]
    public async Task<ActionResult<IEnumerable<MedicoDto>>> GetMediciBySpecializzazione(int specializzazioneId)
    {
        var medici = await _medicoRepository.GetBySpecializzazioneAsync(specializzazioneId);
        var mediciDto = medici.Select(m => MapToMedicoDto(m));
        return Ok(mediciDto);
    }

    /// <summary>
    /// Crea un nuovo medico
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MedicoDto>> CreateMedico([FromBody] CreateMedicoDto dto)
    {
        // Verifica se email già esiste
        var existingEmail = await _medicoRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
        {
            return BadRequest(new { message = "Email già registrata" });
        }

        // Verifica se numero albo già esiste
        var existingAlbo = await _medicoRepository.GetByNumeroAlboAsync(dto.NumeroAlbo);
        if (existingAlbo != null)
        {
            return BadRequest(new { message = "Numero albo già esistente" });
        }

        var medico = new Medico
        {
            Nome = dto.Nome,
            Cognome = dto.Cognome,
            Email = dto.Email,
            Telefono = dto.Telefono,
            NumeroAlbo = dto.NumeroAlbo,
            Biografia = dto.Biografia,
            SpecializzazioneId = dto.SpecializzazioneId
        };

        await _medicoRepository.AddAsync(medico);

        return CreatedAtAction(nameof(GetMedico), new { id = medico.Id }, MapToMedicoDto(medico));
    }

    /// <summary>
    /// Aggiorna un medico esistente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MedicoDto>> UpdateMedico(int id, [FromBody] UpdateMedicoDto dto)
    {
        var medico = await _medicoRepository.GetByIdAsync(id);

        if (medico == null)
        {
            return NotFound(new { message = "Medico non trovato" });
        }

        // Verifica se email già esiste per altro medico
        var existingEmail = await _medicoRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null && existingEmail.Id != id)
        {
            return BadRequest(new { message = "Email già registrata per un altro medico" });
        }

        medico.Nome = dto.Nome;
        medico.Cognome = dto.Cognome;
        medico.Email = dto.Email;
        medico.Telefono = dto.Telefono;
        medico.Biografia = dto.Biografia;
        medico.SpecializzazioneId = dto.SpecializzazioneId;
        medico.Attivo = dto.Attivo;

        await _medicoRepository.UpdateAsync(medico);

        return Ok(MapToMedicoDto(medico));
    }

    /// <summary>
    /// Disattiva un medico
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteMedico(int id)
    {
        var medico = await _medicoRepository.GetByIdAsync(id);

        if (medico == null)
        {
            return NotFound(new { message = "Medico non trovato" });
        }

        // Soft delete: disattiva invece di eliminare
        medico.Attivo = false;
        await _medicoRepository.UpdateAsync(medico);

        return NoContent();
    }

    /// <summary>
    /// Ottiene le disponibilità di un medico
    /// </summary>
    [HttpGet("{id}/disponibilita")]
    public async Task<ActionResult<IEnumerable<DisponibilitaDto>>> GetDisponibilita(int id)
    {
        var medico = await _medicoRepository.GetMedicoConDisponibilitaAsync(id);

        if (medico == null)
        {
            return NotFound(new { message = "Medico non trovato" });
        }

        var disponibilitaDto = medico.Disponibilita
            .Where(d => d.Attivo)
            .Select(d => new DisponibilitaDto
            {
                Id = d.Id,
                MedicoId = d.MedicoId,
                GiornoSettimana = d.GiornoSettimana,
                OraInizio = d.OraInizio,
                OraFine = d.OraFine,
                Attivo = d.Attivo
            });

        return Ok(disponibilitaDto);
    }

    /// <summary>
    /// Imposta le disponibilità di un medico
    /// </summary>
    [HttpPost("{id}/disponibilita")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DisponibilitaDto>> CreateDisponibilita(int id, [FromBody] CreateDisponibilitaDto dto)
    {
        var medico = await _medicoRepository.GetByIdAsync(id);

        if (medico == null)
        {
            return NotFound(new { message = "Medico non trovato" });
        }

        if (dto.OraInizio >= dto.OraFine)
        {
            return BadRequest(new { message = "L'ora di inizio deve essere precedente all'ora di fine" });
        }

        var disponibilita = new DisponibilitaMedico
        {
            MedicoId = id,
            GiornoSettimana = dto.GiornoSettimana,
            OraInizio = dto.OraInizio,
            OraFine = dto.OraFine,
            Attivo = true
        };

        await _disponibilitaRepository.AddAsync(disponibilita);

        return Ok(new DisponibilitaDto
        {
            Id = disponibilita.Id,
            MedicoId = disponibilita.MedicoId,
            GiornoSettimana = disponibilita.GiornoSettimana,
            OraInizio = disponibilita.OraInizio,
            OraFine = disponibilita.OraFine,
            Attivo = disponibilita.Attivo
        });
    }

    private static MedicoDto MapToMedicoDto(Medico medico)
    {
        return new MedicoDto
        {
            Id = medico.Id,
            Nome = medico.Nome,
            Cognome = medico.Cognome,
            Email = medico.Email,
            Telefono = medico.Telefono,
            NumeroAlbo = medico.NumeroAlbo,
            Biografia = medico.Biografia,
            SpecializzazioneId = medico.SpecializzazioneId,
            Specializzazione = medico.Specializzazione?.Nome,
            Attivo = medico.Attivo
        };
    }
}
