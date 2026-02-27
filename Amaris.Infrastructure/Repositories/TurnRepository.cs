using Amaris.Domain.Entities;
using Amaris.Domain.Enums;
using Amaris.Domain.Interfaces.Repositories;
using Amaris.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amaris.Infrastructure.Repositories
{
    public class TurnRepository : ITurnRepository
    {
        private readonly ApplicationDbContext _context;

        public TurnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Turn?> GetByIdAsync(int id) =>
            await _context.Turn
            .Include(t => t.Location)
            .Include(t => t.Service)
            .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<Turn>> GetAllAsync() =>
            await _context.Turn
            .Include(t => t.Location)
            .Include(t => t.Service)
            .OrderByDescending(t => t.DateCreation).ToListAsync();

        public async Task<IEnumerable<Turn>> GetByCedulaAsync(string cedula) =>
            await _context.Turn.Include(t => t.Location)
                .Where(t => t.Identification == cedula)
                .OrderByDescending(t => t.DateCreation).ToListAsync();

        public async Task<int> CountTurnosHoyByCedulaAsync(string cedula)
        {
            var hoy = DateTime.UtcNow.Date;
            return await _context.Turn
                .CountAsync(t => t.Identification == cedula && t.DateCreation.Date == hoy);
        }

        public async Task<Turn> CreateAsync(Turn Turn)
        {
            _context.Turn.Add(Turn);
            await _context.SaveChangesAsync();

            await _context.Entry(Turn).Reference(t => t.Location).LoadAsync();
            await _context.Entry(Turn).Reference(t => t.Service).LoadAsync();

            return Turn;
        }

        public async Task<Turn> UpdateAsync(Turn Turn)
        {
            _context.Turn.Update(Turn);
            await _context.SaveChangesAsync();
            return Turn;
        }

        public async Task<IEnumerable<Turn>> GetTurnosExpiradosAsync() =>
            await _context.Turn
                .Where(t => t.Status == StatusTurn.Pendiente && t.DateExpiration < DateTime.UtcNow)
                .ToListAsync();
    }
}
