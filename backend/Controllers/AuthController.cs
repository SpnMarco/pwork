using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MedicalAppointments.Data;
using MedicalAppointments.Models;

namespace MedicalAppointments.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// Login utente
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var utente = await _context.Utenti
            .Include(u => u.Paziente)
            .Include(u => u.Medico)
            .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Username);

        if (utente == null || !VerifyPassword(request.Password, utente.PasswordHash))
        {
            return Unauthorized(new { message = "Credenziali non valide" });
        }

        if (!utente.Attivo)
        {
            return Unauthorized(new { message = "Account disattivato" });
        }

        // Aggiorna ultimo accesso
        utente.UltimoAccesso = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(utente);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = utente.Username,
            Email = utente.Email,
            Ruolo = utente.Ruolo,
            PazienteId = utente.PazienteId,
            MedicoId = utente.MedicoId
        });
    }

    /// <summary>
    /// Registrazione nuovo utente paziente
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        // Verifica se username o email esistono già
        if (await _context.Utenti.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest(new { message = "Username già esistente" });
        }

        if (await _context.Utenti.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest(new { message = "Email già registrata" });
        }

        // Crea il paziente
        var paziente = new Paziente
        {
            Nome = request.Nome,
            Cognome = request.Cognome,
            CodiceFiscale = request.CodiceFiscale,
            DataNascita = request.DataNascita,
            Email = request.Email,
            Telefono = request.Telefono
        };

        _context.Pazienti.Add(paziente);
        await _context.SaveChangesAsync();

        // Crea l'utente
        var utente = new Utente
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            Ruolo = RuoloUtente.Paziente,
            PazienteId = paziente.Id
        };

        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(utente);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = utente.Username,
            Email = utente.Email,
            Ruolo = utente.Ruolo,
            PazienteId = utente.PazienteId
        });
    }

    private string GenerateJwtToken(Utente utente)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, utente.Id.ToString()),
            new Claim(ClaimTypes.Name, utente.Username),
            new Claim(ClaimTypes.Email, utente.Email),
            new Claim(ClaimTypes.Role, utente.Ruolo),
            new Claim("PazienteId", utente.PazienteId?.ToString() ?? ""),
            new Claim("MedicoId", utente.MedicoId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}

// DTOs
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string CodiceFiscale { get; set; } = string.Empty;
    public DateTime DataNascita { get; set; }
    public string Telefono { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Ruolo { get; set; } = string.Empty;
    public int? PazienteId { get; set; }
    public int? MedicoId { get; set; }
}
