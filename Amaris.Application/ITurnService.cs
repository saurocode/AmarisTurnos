using Amaris.Domain.Entities;

namespace Amaris.Application
{
    public interface ITurnService
    {
        Task<Turn> CrearTurnoAsync(string Identification, int locationId);
    }

    // En la implementación usarás un repositorio para contar turnos de hoy
    // if (conteo >= 5) throw new Exception("Límite diario alcanzado");
}
