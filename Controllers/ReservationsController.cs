using Microsoft.AspNetCore.Mvc;
using Vanilla.DTO;
using Vanilla.Services;

namespace Vanilla.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationsController(IReservationService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReservationDto>>> List(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            CancellationToken ct)
        {
            var data = await service.GetReservationsAsync(from, to, ct);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateReservationDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try
            {
                var id = await service.CreateReservationAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex)
            {
                // conflict (dublură)
                return Conflict(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ReservationDto>> GetById(long id, CancellationToken ct)
        {
            // Simplu: folosim List + FirstOrDefault (sau faci un nou method în service)
            var data = await service.GetReservationsAsync(null, null, ct);
            var item = data.FirstOrDefault(r => r.Id == id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var ok = await service.DeleteReservationAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
