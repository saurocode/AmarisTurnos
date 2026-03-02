using Amaris.Domain.Entities;
using Amaris.Domain.Enums;

namespace UnitTests.Helpers
{
    public static class MockHelper
    {
        public static Turn CreateTurn(int id = 1, string identification = "1234567890",
        StatusTurn status = StatusTurn.Pendiente) => new()
        {
            Id = id,
            Identification = identification,
            IdLocation = 1,
            ServiceId = 1,
            TurnCode = $"T-20260301-ABC{id:D3}",
            DateCreation = DateTime.UtcNow,
            DateExpiration = DateTime.UtcNow.AddMinutes(15),
            Status = status,
            Location = new Location { Id = 1, Name = "Sucursal Centro", City = "Bogotá" },
            Service = new Service { Id = 1, Name = "Ventanilla / Caja" }
        };

        public static Location CreateLocation(int id = 1) => new()
        {
            Id = id,
            Name = "Sucursal Centro",
            Address = "Calle 10 #5-20",
            City = "Bogotá",
            Active = true
        };

        public static Service CreateService(int id = 1) => new()
        {
            Id = id,
            Name = "Ventanilla / Caja",
            Description = "Operaciones de caja",
            Active = true
        };

    }
}
