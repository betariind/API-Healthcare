
using Healthcare.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Api.Controllers;

[ApiController]
[Route("doctors")]
public class DoctorsController : ControllerBase
{
    private readonly AvailabilityService _service;
    public DoctorsController(AvailabilityService service) => _service = service;

    [HttpGet("{id}/availability")]
    public async Task<IActionResult> GetAvailability(Guid id, DateTime from, DateTime to, int slot)
        => Ok(await _service.GetAvailability(id, from, to, slot));
}
