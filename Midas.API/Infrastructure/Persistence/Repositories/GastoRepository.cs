using Microsoft.EntityFrameworkCore;
using Midas.Infrastructure.Persistence.Entities;

namespace Midas.Infrastructure.Persistence.Repositories
{
    public class GastoRepository : IGastoRepository
    {
        private readonly MidasContext _context;

        public GastoRepository(MidasContext context)
        {
            _context = context;
        }

        public async Task<Gasto> AddAsync(Gasto gasto)
        {
            await _context.Gastos.AddAsync(gasto);
            await _context.SaveChangesAsync();
            return gasto;
        }

        public async Task<Gasto?> GetByIdAsync(int id)
        {
            return await _context.Gastos
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<Gasto>> GetAllAsync()
        {
            return await _context.Gastos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(Gasto gasto)
        {
            _context.Gastos.Update(gasto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
