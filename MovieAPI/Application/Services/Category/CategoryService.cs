using MovieAPI.Infrastructure.Data.Repositories.Category;

namespace MovieAPI.Application.Services.Category;

public abstract class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    protected CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public List<Infrastructure.Data.Entities.Category.Category> GetAllCategories()
    {
        return _categoryRepository.GetAll().ToList();
    }
}