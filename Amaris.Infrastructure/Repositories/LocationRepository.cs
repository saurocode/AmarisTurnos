using Amaris.Domain.Entities;
using Amaris.Domain.Interfaces.Repositories;
using Amaris.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Amaris.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        public LocationRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Location>> GetAllActiveAsync() =>
            await _context.Locations.Where(s => s.Active).ToListAsync();

        public async Task<Location?> GetByIdAsync(int id) =>
            await _context.Locations.FindAsync(id);
    }
}
