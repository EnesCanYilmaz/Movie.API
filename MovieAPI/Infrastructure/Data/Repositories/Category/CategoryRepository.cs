using MovieAPI.Infrastructure.Data.Repositories.Base;

namespace MovieAPI.Infrastructure.Data.Repositories.Category;

public class CategoryRepository : EfEntityRepository<Entities.Category.Category>, ICategoryRepository
{
    public CategoryRepository(DbContext context) : base(context)
    {
    }
}