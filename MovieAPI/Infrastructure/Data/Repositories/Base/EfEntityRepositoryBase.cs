using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MovieAPI.Infrastructure.Data.Repositories.Base;

public class EfEntityRepository<T> : IEfEntityRepository<T> where T : class, IEntity, new()
{
    private readonly DbContext _context;

    public EfEntityRepository(DbContext context)
    {
        _context = context;
    }

    private DbSet<T> Table => _context.Set<T>();

    public IQueryable<T> GetAll(bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query;    
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.Where(method);
        if (!tracking)
            query = query.AsNoTracking();
        return query;
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(method) ?? throw new InvalidOperationException();
    }

    public async Task<T?> GetByIdAsync(int id, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        var entities = await query.FirstOrDefaultAsync(data => EF.Property<int>(data, "Id") == id);

        return entities ?? null;
    }

    public async Task<T> AddAsync(T model)
    {
        var entityEntry = await Table.AddAsync(model);
        if (entityEntry.State != EntityState.Added) 
            return null;
        await SaveAsync();
        
        return entityEntry.Entity;
    }

    public async Task<List<T>> AddRangeAsync(List<T> datas)
    {
        await Table.AddRangeAsync(datas);
        await SaveAsync();

        var addedEntities = datas.Select(entity => Table.Entry(entity).Entity).ToList();

        return addedEntities;
    }

    public bool Remove(T model)
    {
        var entityEntry = Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }

    public bool RemoveRange(List<T> datas)
    {
        Table.RemoveRange(datas);
        return true;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        T? model = await Table.FirstOrDefaultAsync(data => EF.Property<int>(data, "Id") == id);
        return Remove(model);
    }

    public bool Update(T model)
    {
        EntityEntry<T> entityEntry = Table.Update(model);
        return entityEntry.State == EntityState.Modified;
    }

    public Task<int> SaveAsync() => _context.SaveChangesAsync();

}