using Microsoft.EntityFrameworkCore;
using Vanilla.Data;
using Vanilla.DTO;
using Vanilla.Models;

namespace Vanilla.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;
    public ReservationService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ReservationDto>> GetReservationsAsync(
        DateTime? from, DateTime? to, CancellationToken ct)
    {
        // toate momentele în UTC
        DateTime? fromUtc = from.HasValue ? DateTime.SpecifyKind(from.Value, DateTimeKind.Utc) : null;
        DateTime? toUtc = to.HasValue ? DateTime.SpecifyKind(to.Value, DateTimeKind.Utc) : null;

        var q = _db.Reservations.AsNoTracking();

        if (fromUtc.HasValue) q = q.Where(r => r.StartAt >= fromUtc.Value);
        if (toUtc.HasValue) q = q.Where(r => r.StartAt < toUtc.Value);

        var items = await q.OrderBy(r => r.StartAt).ToListAsync(ct);

        return items.Select(r => new ReservationDto
        {
            Id = r.Id,
            TableId = r.TableId,
            StartAt = r.StartAt,                 // UTC
            DurationMinutes = r.DurationMinutes,
            CustomerName = r.CustomerName,
            Phone = r.Phone,
            PartySize = r.PartySize,
            Note = r.Note
        }).ToList();
    }

    public async Task<long> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct)
    {
        if (dto.DurationMinutes <= 0) throw new ArgumentOutOfRangeException(nameof(dto.DurationMinutes));

        var startUtc = DateTime.SpecifyKind(dto.StartAt, DateTimeKind.Utc);
        var endUtc = startUtc.AddMinutes(dto.DurationMinutes);

        // suprapunere corectă: [a,b) și [c,d) se suprapun dacă a < d && c < b
        bool overlaps = await _db.Reservations
            .Where(r => r.TableId == dto.TableId)
            .AnyAsync(r =>
                r.StartAt < endUtc &&
                startUtc < r.StartAt.AddMinutes(r.DurationMinutes), ct);

        if (overlaps)
            throw new InvalidOperationException("Intervalul se suprapune peste o rezervare existentă.");

        var entity = new Reservation
        {
            TableId = (short)dto.TableId,
            StartAt = startUtc, // stocăm UTC
            DurationMinutes = dto.DurationMinutes,
            CustomerName = dto.CustomerName,
            Phone = dto.Phone,
            PartySize = dto.PartySize,
            Note = dto.Note
        };

        _db.Reservations.Add(entity);
        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<bool> DeleteReservationAsync(long id, CancellationToken ct)
    {
        var e = await _db.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (e is null) return false;
        _db.Reservations.Remove(e);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<TableStatusDto>> GetStatusForSlotAsync(
    DateTime startAtUtc, CancellationToken ct)
    {
        var slotUtc = DateTime.SpecifyKind(startAtUtc, DateTimeKind.Utc);

        // luăm toate mesele ordonate
        var ids = await _db.Tables
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .Select(t => t.Id)
            .ToListAsync(ct);

        var result = new List<TableStatusDto>(ids.Count);

        foreach (var id in ids)
        {
            // inclusiv la început, exclusiv la final: [Start, End)
            bool reserved = await _db.Reservations
                .AsNoTracking()
                .AnyAsync(r =>
                    r.TableId == id &&
                    r.StartAt <= slotUtc &&                              // <= start
                    slotUtc < r.StartAt.AddMinutes(r.DurationMinutes),  //  < end
                    ct);

            result.Add(new TableStatusDto { TableId = id, IsReserved = reserved });
        }

        return result;
    }
}
