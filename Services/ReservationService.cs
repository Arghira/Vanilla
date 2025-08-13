using Microsoft.EntityFrameworkCore;
using Vanilla.Data;
using Vanilla.DTO;
using Vanilla.Models;

namespace Vanilla.Services
{
    public class ReservationService(AppDbContext db) : IReservationService
    {
        public async Task<IReadOnlyList<ReservationDto>> GetReservationsAsync(
            DateTime? from, DateTime? to, CancellationToken ct)
        {
            var q = db.Reservations.AsNoTracking().AsQueryable();

            if (from.HasValue)
                q = q.Where(r => r.StartAt >= DateTime.SpecifyKind(from.Value, DateTimeKind.Utc));
            if (to.HasValue)
                q = q.Where(r => r.StartAt < DateTime.SpecifyKind(to.Value, DateTimeKind.Utc));

            return await q
                .OrderBy(r => r.StartAt)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    TableId = r.TableId,
                    StartAt = r.StartAt,
                    DurationMinutes = r.DurationMinutes,
                    CustomerName = r.CustomerName,
                    Phone = r.Phone,
                    PartySize = r.PartySize,
                    Note = r.Note
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<TableStatusDto>> GetStatusForSlotAsync(DateTime startAtUtc, CancellationToken ct)
        {
            var normalized = DateTime.SpecifyKind(startAtUtc, DateTimeKind.Utc);

            var reserved = await db.Reservations
                .AsNoTracking()
                .Where(r => r.StartAt == normalized)
                .Select(r => r.TableId)
                .ToListAsync(ct);

            return Enumerable.Range(1, 11)
                .Select(i => new TableStatusDto
                {
                    TableId = (short)i,
                    IsReserved = reserved.Contains((short)i)
                })
                .ToList();
        }

        public async Task<long> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct)
        {
            if (dto.TableId is < 1 or > 11)
                throw new ArgumentOutOfRangeException(nameof(dto.TableId), "TableId trebuie 1..8");

            var startUtc = DateTime.SpecifyKind(dto.StartAt, DateTimeKind.Utc);

            var entity = new Reservation
            {
                TableId = dto.TableId,
                StartAt = startUtc,
                DurationMinutes = dto.DurationMinutes <= 0 ? 60 : dto.DurationMinutes,
                CustomerName = dto.CustomerName,
                Phone = dto.Phone,
                PartySize = dto.PartySize,
                Note = dto.Note,
                CreatedAt = DateTime.UtcNow
            };

            db.Reservations.Add(entity);
            try
            {
                await db.SaveChangesAsync(ct);
                return entity.Id;
            }
            catch (DbUpdateException ex) when
                (ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true)
            {
                // traducem într-o excepție controlată (poți face un tip custom)
                throw new InvalidOperationException("Masa este deja rezervată pentru acest interval.");
            }
        }

        public async Task<bool> DeleteReservationAsync(long id, CancellationToken ct)
        {
            var res = await db.Reservations.FindAsync([id], ct);
            if (res is null) return false;
            db.Remove(res);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
