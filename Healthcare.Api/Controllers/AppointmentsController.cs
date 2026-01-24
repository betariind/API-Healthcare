
using Healthcare.Api.DTOs;
using Healthcare.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Api.Controllers;

[ApiController]
[Route("appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _service;
    public AppointmentsController(AppointmentService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentRequest req)
    {
        try { await _service.CreateAppointmentAsync(req); return Ok(); }
        catch {
            return Conflict("Doctor is not available at the selected time");
        }
    }

    [HttpDelete("appointments/{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var deleted = await _service.CancelAppointment(id);

        if (!deleted)
            return NotFound();

        return NoContent(); // 204
    }

}
