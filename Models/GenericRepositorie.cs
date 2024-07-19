using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProyectoModulo.Models;

public class GenericRepositorie<TEntity> where TEntity : class
{
    private readonly VentasDbContext _context;

    public GenericRepositorie(VentasDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filtro)
    {
        try
        {
            var entidad = await _context.Set<TEntity>().FirstOrDefaultAsync(filtro);

            return entidad;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<TEntity> CreateAsync(TEntity entidad)
    {
        try
        {
            _context.Set<TEntity>().Add(entidad);
            await _context.SaveChangesAsync();

            return entidad;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> EditAsync(TEntity entidad)
    {
        try
        {
            _context.Set<TEntity>().Update(entidad);
            int resultado = await _context.SaveChangesAsync();

            return resultado > 0;
        }
        catch
        {
            throw;
        }
    }

    public async Task<TEntity?> DeleteAsync(TEntity entidad)
    {
        try
        {
            _context.Set<TEntity>().Remove(entidad);
            int resultado = await _context.SaveChangesAsync();

            return resultado > 0 ? entidad : null;
        }
        catch
        {
            throw;
        }
    }

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filtro = null)
    {
        try
        {
            IQueryable<TEntity> entidad = filtro is null
                ? _context.Set<TEntity>()
                : _context.Set<TEntity>().Where(filtro);

            return entidad;
        }
        catch
        {
            throw;
        }
    }
}
