using Amaris.Domain.Entities;

namespace Amaris.Domain.Interfaces.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllActivasAsync();
        Task<Location?> GetByIdAsync(int id);
    }
}
