using MovieAPI.Infrastructure.Data.Repositories.Base;

namespace MovieAPI.Infrastructure.Data.Repositories.Category;

public interface ICategoryRepository : IEfEntityRepository<Entities.Category.Category>
{
    
}