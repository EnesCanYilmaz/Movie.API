using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.DTO.Category;
using MovieAPI.DTO.MovieDTO;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Category;

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
        var categories = await _context.Categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
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
            Movies = c.Movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                CategoryId = m.Category.Id,
                CategoryName = m.Category.Name,
                PlatformId = m.Platform.Id,
                PlatformName = m.Platform.Name,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
                MovieTime = m.MovieTime
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);

        return category is not null
            ? OK(200, "Category listed by id!", category)
            : NotFound("Category Id not found!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCategory(string categoryName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = new Category
        {
            Name = categoryName,
            CreatedDate = DateTime.Now
        };

        await _context.Categories.AddAsync(category);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Category added!", category)
            : StatusCode(500, "Category not Added");
    }

    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, string categoryName)
    {
        var existingCategory = await _context.Categories.FindAsync(id);

        if (existingCategory is null)
            return NotFound("Category not found!");

        if (string.IsNullOrWhiteSpace(categoryName))
            return BadRequest("Category name cannot be empty!");

        existingCategory.Name = categoryName;

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Category updated!", existingCategory)
            : StatusCode(500, "Category not Updated!");
    }


    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return NotFound("Category Not Found!");

        _context.Categories.Remove(category);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Category deleted by id!", category)
            : StatusCode(500, "Category not Deleted");
    }
}
