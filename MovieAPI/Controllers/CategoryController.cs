namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class CategoryController : BaseAPIController
{
    private readonly MovieAPIDbContext _context;

    public CategoryController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _context.Categories.Select(c => new ListCategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            CreatedDate = c.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = c.UpdatedDate.ToString("dd.MM.yyyy HH:mm") 
        }).ToListAsync();

        return categories is not null
            ? OK(200, "All categories listed!", categories)
            : NotFound("Categories not found!");
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetByCategoryId(int id)
    {
        var category = await _context.Categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            CreatedDate = c.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = c.UpdatedDate.ToString("dd.MM.yyyy HH:mm"), 
            Movies = c.Movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                CategoryName = m.Category.Name,
                PlatformName = m.Platform.Name,
                ReleaseDate = m.ReleaseDate.ToString("dd.MM.yyyy HH:mm"),
                MovieTime = m.MovieTime,
                CreatedDate = m.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                UpdatedDate = m.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
                Players = m.Players.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList(),
                Directors = m.Directors.Select(d => new DirectorDTO
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList(),
                MovieImages = m.MovieImages.Select(i => new MovieImageDTO
                {
                    Id = i.Id,
                    FileName = i.FileName,
                    Path = i.Path
                }).ToList(),
            
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);

        return category is not null
            ? OK(200, "Category listed!", category)
            : NotFound("Category not found!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCategory(string categoryName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = new Category
        {
            Name = categoryName,
            CreatedDate = DateTime.UtcNow
        };

        await _context.Categories.AddAsync(category);

        var addedCategoryResult = await _context.SaveChangesAsync();

        var categoryDto = new CreateCategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            CreatedDate = category.CreatedDate.ToString("dd.MM.yyyy HH:mm")
        };

        return addedCategoryResult > 0
            ? OK(200, "Category added!", categoryDto)
            : StatusCode(500, "Category not added!");
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO updateCategoryDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var existingCategory = await _context.Categories.FindAsync(updateCategoryDTO.Id);

        if (existingCategory is null)
            return NotFound("Category not found!");

        if (string.IsNullOrWhiteSpace(updateCategoryDTO.Name))
            return BadRequest("Category name cannot be empty!");

        existingCategory.Name = updateCategoryDTO.Name;
        existingCategory.UpdatedDate = DateTime.UtcNow;


        var updatedCategoryResult = await _context.SaveChangesAsync();

        var listCategoryDTO = new ListCategoryDTO
        {
            Id = existingCategory.Id,
            Name = existingCategory.Name,
            CreatedDate = existingCategory.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = existingCategory.UpdatedDate.ToString("dd.MM.yyyy HH:mm")
        };

        return updatedCategoryResult > 0
            ? OK(200, "Category updated!", listCategoryDTO)
            : StatusCode(500, "Category not updated!");
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return NotFound("Category not found!");

        _context.Categories.Remove(category);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Category deleted by id!", "All movies, actors, directors, photos related to the category have been deleted!")
            : StatusCode(500, "Category not deleted");
    }
}
