namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class CategoryController : BaseApiController
{
    private readonly MovieAPIDbContext _context;

    public CategoryController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _context.Categories.Select(c => new ListCategoryDto { Id = c.Id, Name = c.Name, CreatedDate = c.CreatedDate.ToString("dd.MM.yyyy HH:mm"), UpdatedDate = c.UpdatedDate.ToString("dd.MM.yyyy HH:mm") }).ToListAsync();

        return OK(200, "All categories listed!", categories);
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetByCategoryId(int id)
    {
        var category = await _context.Categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedDate = c.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = c.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
            Movies = (c.Movies).Select(m => new MovieDto
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
                Players = m.Players.Select(p => new PlayerDto { Id = p.Id, Name = p.Name }).ToList(),
                Directors = m.Directors.Select(d => new DirectorDto { Id = d.Id, Name = d.Name }).ToList(),
                MovieImages = m.MovieImages.Select(i => new MovieImageDto { Id = i.Id, FileName = i.FileName, Path = i.Path }).ToList(),
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);

        return OK(200, "Category listed!", category);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCategory(string categoryName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = new Category { Name = categoryName, CreatedDate = DateTime.UtcNow };

        await _context.Categories.AddAsync(category);

        await _context.SaveChangesAsync();

        var categoryDto = new CreateCategoryDto { Id = category.Id, Name = category.Name, CreatedDate = category.CreatedDate.ToString("dd.MM.yyyy HH:mm") };

        return OK(200, "Category added!", categoryDto);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto updateCategoryDTO)
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

        await _context.SaveChangesAsync();

        var listCategoryDto = new ListCategoryDto { Id = existingCategory.Id, Name = existingCategory.Name, CreatedDate = existingCategory.CreatedDate.ToString("dd.MM.yyyy HH:mm"), UpdatedDate = existingCategory.UpdatedDate.ToString("dd.MM.yyyy HH:mm") };

        return OK(200, "Category updated!", listCategoryDto);
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
        await _context.SaveChangesAsync();

        return OK(200, "Category deleted by id!", "All movies, actors, directors, photos related to the category have been deleted!");
    }
}