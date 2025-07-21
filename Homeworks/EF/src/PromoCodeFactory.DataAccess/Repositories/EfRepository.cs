using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DataContext _datacontext;
    private readonly DbSet<T> Data;


    public EfRepository(DataContext context)
    {
        _datacontext = context;
        Data = _datacontext.Set<T>();

    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Data.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await Data.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
    {
        return await Data.Where(e => ids.Contains(e.Id)).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Data.AddAsync(entity);
        await _datacontext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        Data.Update(entity);
        await _datacontext.SaveChangesAsync();

    }

    public async Task DeleteAsync(Guid Id)
    {
        var entity = await GetByIdAsync(Id);

        Data.Remove(entity);
        await _datacontext.SaveChangesAsync();
    }

    public async Task<T> DeleteRangeAsync(IEnumerable<Guid> Ids)
    {
        var entities = await Data.FindAsync(Ids);
        Data.RemoveRange(entities);

        await _datacontext.SaveChangesAsync();
        return entities;
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await Data.FirstOrDefaultAsync(predicate);
    }
}
