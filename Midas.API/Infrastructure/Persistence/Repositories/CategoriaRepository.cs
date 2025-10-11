using Microsoft.EntityFrameworkCore;
using Midas.Infrastructure.Persistence.Entities;

namespace Midas.Infrastructure.Persistence.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly MidasContext _context;

        public CategoriaRepository(MidasContext context)
        {
            _context = context;
        }

        public async Task<Categoria> AddAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}