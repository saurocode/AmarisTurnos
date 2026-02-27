using Amaris.Domain.Entities;

namespace Amaris.Domain.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllActiveAsync();
        Task<Service?> GetByIdAsync(int id);
    }
}
