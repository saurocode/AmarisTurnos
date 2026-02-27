namespace Amaris.Application.DTOs.Turn
{
    public class UpdateTurnDto
    {
        public int Id { get; set; }
        public string NewStatus { get; set; } = string.Empty;
    }
}
