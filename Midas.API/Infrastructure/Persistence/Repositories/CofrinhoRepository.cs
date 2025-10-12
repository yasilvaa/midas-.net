using Microsoft.EntityFrameworkCore;
using Midas.Infrastructure.Persistence.Entities;

namespace Midas.Infrastructure.Persistence.Repositories;

public class CofrinhoRepository : ICofrinhoRepository
{
    private readonly MidasContext _context;

    public CofrinhoRepository(MidasContext context)
    {
        _context = context;
    }

    public async Task<Cofrinho> AddAsync(Cofrinho cofrinho)
    {
        cofrinho.Aplicado = NormalizeAplicadoValue(cofrinho.Aplicado);
        await _context.Cofrinhos.AddAsync(cofrinho);
        await _context.SaveChangesAsync();
        return cofrinho;
    }

    public async Task<Cofrinho?> GetByIdAsync(int id)
    {
        return await _context.Cofrinhos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Cofrinho>> GetAllAsync()
    {
        return await _context.Cofrinhos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Cofrinho cofrinho)
    {
        cofrinho.Aplicado = NormalizeAplicadoValue(cofrinho.Aplicado);
        _context.Cofrinhos.Update(cofrinho);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cofrinho = await _context.Cofrinhos.FindAsync(id);
        if (cofrinho != null)
        {
            _context.Cofrinhos.Remove(cofrinho);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> AtualizarProgresso(int id, decimal valorAtingido)
    {
        var cofrinho = await _context.Cofrinhos.FindAsync(id);
        if (cofrinho == null) return false;

        cofrinho.Atingido = valorAtingido;
        await _context.SaveChangesAsync();
        return true;
    }
    private char NormalizeAplicadoValue(char aplicado)
    {
        return char.ToUpper(aplicado) == 'T' ? 'T' : 'F';
    }
}
