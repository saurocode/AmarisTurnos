using Amaris.Domain.Entities;
using Amaris.Domain.Interfaces.Repositories;
using Amaris.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Amaris.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) => _context = context;

        public async Task<Usuario?> GetByUsernameAsync(string username) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
