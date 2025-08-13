using System.ComponentModel.DataAnnotations;

namespace Vanilla.DTO
{
    public class CreateReservationDto
    {
        [Range(1, 11, ErrorMessage = "TableId must be between 1 and 11.")]
        public short TableId { get; set; }

        [Required]
        public DateTime StartAt { get; set; } // trimite UTC (ISO 8601)

        [Range(1, int.MaxValue)]
        public int DurationMinutes { get; set; } = 60;

        [Required, MinLength(2)]
        public string CustomerName { get; set; } = "";

        public string? Phone { get; set; }
        [Range(1, 20)]
        public int? PartySize { get; set; }
        public string? Note { get; set; }
    }
}
