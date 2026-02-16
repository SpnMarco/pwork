using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointments.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Endpoint per verificare che l'API sia attiva
    /// </summary>
    [HttpGet]
    public ActionResult<object> GetHealth()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            service = "Medical Appointments API"
        });
    }
}
