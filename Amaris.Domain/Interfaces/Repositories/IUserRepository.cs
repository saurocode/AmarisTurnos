using Amaris.Domain.Entities;

namespace Amaris.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<Usuario> CreateAsync(Usuario usuario);
    }
}
