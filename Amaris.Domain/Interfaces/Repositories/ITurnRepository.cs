using Amaris.Domain.Entities;

namespace Amaris.Domain.Interfaces.Repositories
{
    public interface ITurnRepository
    {
        Task<Turn?> GetByIdAsync(int id);
        Task<IEnumerable<Turn>> GetAllAsync();
        Task<IEnumerable<Turn>> GetByCedulaAsync(string cedula);
        Task<int> CountTurnTodayByCedulaAsync(string cedula);
        Task<Turn> CreateAsync(Turn turno);
        Task<Turn> UpdateAsync(Turn turno);
        Task<IEnumerable<Turn>> GetExpiredTurnAsync();
        Task<IEnumerable<Turn>> GetByIdentificationAsync(string identification);
    }
}
