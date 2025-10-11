using Midas.Infrastructure.Persistence.Entities;

namespace Midas.Infrastructure.Persistence.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria categoria);
        Task<Categoria?> GetByIdAsync(int id);
        Task<List<Categoria>> GetAllAsync();
        Task UpdateAsync(Categoria categoria);
        Task DeleteAsync(int id);
    }
}