using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalAppointments.DTOs;
using MedicalAppointments.Interfaces;
using MedicalAppointments.Models;

namespace MedicalAppointments.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecializzazioniController : ControllerBase
{
    private readonly IRepository<Specializzazione> _specializzazioneRepository;

    public SpecializzazioniController(IRepository<Specializzazione> specializzazioneRepository)
    {
        _specializzazioneRepository = specializzazioneRepository;
    }

    /// <summary>
    /// Ottiene la lista di tutte le specializzazioni
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpecializzazioneDto>>> GetSpecializzazioni()
    {
        var specializzazioni = await _specializzazioneRepository.GetAllAsync();
        var specializzazioniDto = specializzazioni.Select(s => new SpecializzazioneDto
        {
            Id = s.Id,
            Nome = s.Nome,
            Descrizione = s.Descrizione
        });

        return Ok(specializzazioniDto);
    }

    /// <summary>
    /// Ottiene una specializzazione per ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SpecializzazioneDto>> GetSpecializzazione(int id)
    {
        var specializzazione = await _specializzazioneRepository.GetByIdAsync(id);

        if (specializzazione == null)
        {
            return NotFound(new { message = "Specializzazione non trovata" });
        }

        return Ok(new SpecializzazioneDto
        {
            Id = specializzazione.Id,
            Nome = specializzazione.Nome,
            Descrizione = specializzazione.Descrizione
        });
    }
}
