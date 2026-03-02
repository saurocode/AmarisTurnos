namespace Amaris.Application.DTOs.Turn
{
    public class UpdateTurnDto
    {
        public int Id { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public int IdLocation { get; set; }
        public int ServiceId { get; set; }
    }
}
