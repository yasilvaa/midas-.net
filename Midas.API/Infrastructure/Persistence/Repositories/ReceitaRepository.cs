using Microsoft.EntityFrameworkCore;
using Midas.Infrastructure.Persistence.Entities;

namespace Midas.Infrastructure.Persistence.Repositories
{
    public class ReceitaRepository : IReceitaRepository
    {
        private readonly MidasContext _context;

        public ReceitaRepository(MidasContext context)
        {
            _context = context;
        }

        public async Task<Receita> CreateAsync(Receita receita)
        {
            await _context.Receitas.AddAsync(receita);
            await _context.SaveChangesAsync();
            return receita;
        }

        public async Task<Receita?> GetByIdAsync(int id)
        {
            return await _context.Receitas
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Receita>> GetAllAsync()
        {
            return await _context.Receitas
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Receita> UpdateAsync(Receita receita)
        {
            _context.Receitas.Update(receita);
            await _context.SaveChangesAsync();
            return receita;
        }

        public async Task DeleteAsync(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita != null)
            {
                _context.Receitas.Remove(receita);
                await _context.SaveChangesAsync();
            }
        }
    }
}
