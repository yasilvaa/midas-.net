using System;
using Midas.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Midas.API.DTOs;

namespace Midas.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly MidasContext _context;

    public UsuarioRepository(MidasContext context)
    {
        _context = context;
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }

    public async Task<PagedResult<Usuario>> SearchAsync(UsuarioSearchParameters parameters)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrEmpty(parameters.Nome))
        {
            query = query.Where(u => u.Nome.Contains(parameters.Nome));
        }

        if (!string.IsNullOrEmpty(parameters.Email))
        {
            query = query.Where(u => u.Email.Contains(parameters.Email));
        }

        if (parameters.DataCriacaoInicio.HasValue)
        {
            query = query.Where(u => u.DataCriacao >= parameters.DataCriacaoInicio.Value);
        }

        if (parameters.DataCriacaoFim.HasValue)
        {
            query = query.Where(u => u.DataCriacao <= parameters.DataCriacaoFim.Value);
        }

        var totalRecords = await query.CountAsync();

        query = parameters.OrderBy.ToLower() switch
        {
            "nome" => parameters.Direction.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Nome)
                : query.OrderBy(u => u.Nome),
            "email" => parameters.Direction.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),
            "datacriacao" => parameters.Direction.ToLower() == "desc"
                ? query.OrderByDescending(u => u.DataCriacao)
                : query.OrderBy(u => u.DataCriacao),
            _ => parameters.Direction.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Id)
                : query.OrderBy(u => u.Id)
        };

        var data = await query
            .Skip(parameters.Skip)
            .Take(parameters.Size)
            .AsNoTracking()
            .ToListAsync();

        return new PagedResult<Usuario>(data, totalRecords, parameters.Page, parameters.Size);
    }
}
