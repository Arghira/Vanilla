namespace Vanilla.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public short TableId { get; set; }
        public TableEntity? Table { get; set; }

        public DateTime StartAt { get; set; }      // păstrăm UTC
        public int DurationMinutes { get; set; } = 60;

        public string CustomerName { get; set; } = "";
        public string? Phone { get; set; }
        public int? PartySize { get; set; }
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }    // default din DB
    }
}
