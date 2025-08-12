namespace Vanilla.DTO
{
    public class ReservationDto
    {
        public long Id { get; set; }
        public short TableId { get; set; }
        public DateTime StartAt { get; set; }
        public int DurationMinutes { get; set; }
        public string CustomerName { get; set; } = "";
        public string? Phone { get; set; }
        public int? PartySize { get; set; }
        public string? Note { get; set; }
    }
}
