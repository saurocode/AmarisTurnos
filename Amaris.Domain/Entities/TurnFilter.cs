namespace Amaris.Domain.Entities
{
    public class TurnFilter
    {
        public string? Identification { get; set; }
        public string? Status { get; set; }
        public int? LocationId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
