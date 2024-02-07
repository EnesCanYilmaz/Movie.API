
namespace MovieAPI.Infrastructure.Data.Repositories.Base;

public partial interface IEfEntityRepository<T> where T : class, IEntity, new()
{
    IQueryable<T> GetAll(bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T?> GetByIdAsync(int id, bool tracking = true);
    Task<T> AddAsync(T model);
    Task<List<T>> AddRangeAsync(List<T> datas);
    bool Remove(T model);
    bool RemoveRange(List<T> datas);
    Task<bool> RemoveAsync(int id);
    bool Update(T model);
    Task<int> SaveAsync();
}