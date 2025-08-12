using Microsoft.AspNetCore.Mvc;
using Vanilla.DTO;
using Vanilla.Services;

namespace Vanilla.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController(IReservationService service) : ControllerBase
    {
        // GET /status?startAt=2025-08-12T21:00:00Z
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TableStatusDto>>> Get([FromQuery] DateTime startAt, CancellationToken ct)
        {
            var result = await service.GetStatusForSlotAsync(startAt, ct);
            return Ok(result);
        }
    }
}
