namespace Amaris.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public ICollection<Turn> Turns { get; set; } = new List<Turn>();
    }
}
