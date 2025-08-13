using Vanilla.DTO;

namespace Vanilla.Services
{
    public interface IReservationService
    {
        Task<IReadOnlyList<ReservationDto>> GetReservationsAsync(
            DateTime? from, DateTime? to, CancellationToken ct);

        Task<long> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct);

        Task<bool> DeleteReservationAsync(long id, CancellationToken ct);

        // <- metoda cerută de StatusController
        Task<IReadOnlyList<TableStatusDto>> GetStatusForSlotAsync(
            DateTime startAtUtc, CancellationToken ct);
    }
}
