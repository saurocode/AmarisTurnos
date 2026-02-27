using Amaris.Domain.Entities;
using Amaris.Domain.Interfaces.Repositories;
using Amaris.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Amaris.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;
    public ServiceRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Service>> GetAllActiveAsync() =>
        await _context.Services.Where(s => s.Active).ToListAsync();

    public async Task<Service?> GetByIdAsync(int id) =>
        await _context.Services.FindAsync(id);
}