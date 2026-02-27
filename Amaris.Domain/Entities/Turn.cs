using Amaris.Domain.Enums;

namespace Amaris.Domain.Entities
{
    public class Turn
    {
        public int Id { get; set; }
        public string Identification { get; set; } = string.Empty;
        public int IdLocation { get; set; }
        public Location Location { get; set; } = null!;
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public DateTime DateExpiration { get; set; }
        public DateTime? DateActivation { get; set; }
        public StatusTurn Status { get; set; } = StatusTurn.Pendiente;
        public string TurnCode { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
