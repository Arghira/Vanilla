namespace Vanilla.Models
{
    public class TableEntity
    {
        public short Id { get; set; }              // 1..8
        public string Name { get; set; } = "";
        public List<Reservation> Reservations { get; set; } = new();
    }
}
