using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.DTO.Category;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Category;
using MovieAPI.Infrastructure.Data.Entities.Movie;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class CategoryController : Controller
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
            Movies = c.Movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                CategoryId = m.Category.Id,
                CategoryName = m.Category.Name,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
                MovieTime = m.MovieTime.ToString("hh/mm")
            }).ToList()
        }).ToListAsync();


        return categories is not null
            ? Ok(categories)
            : NotFound("Categories not found");
    }

    [HttpGet("[action]{id}")]
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
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
                MovieTime = m.MovieTime.ToString("hh/mm")
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);


        return category is not null
            ? Ok(category)
            : NotFound();
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

        var addedResult = await _context.Categories.AddAsync(category);
        var saveChangesResult = await _context.SaveChangesAsync();

        if (addedResult.State == EntityState.Deleted && saveChangesResult > 0)
            return Ok("Category Added!");
        else
            return StatusCode(500, "Category not Added!");
    }

    [HttpPut("[action]{id}")]
    public async Task<ActionResult> UpdateCategory(int id, string categoryName)
    {
        var existingCategory = await _context.Categories.FindAsync(id);

        if (existingCategory is null)
            return NotFound("Category not found!");

        if (string.IsNullOrWhiteSpace(categoryName))
            return BadRequest("Category name cannot be empty!");

        existingCategory.Name = categoryName;

        return await _context.SaveChangesAsync() > 0
            ? Ok("Category Updated!")
            : StatusCode(500, "Category not Updated!");
    }


    [HttpDelete("[action]{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is null) return NotFound("Category Not Found!");

        var removeResult = _context.Categories.Remove(category);
        var saveChangesResult = await _context.SaveChangesAsync();

        if (removeResult.State == EntityState.Deleted && saveChangesResult > 0)
            return Ok("Category Deleted!");
        else
            return StatusCode(500, "Category not Deleted!");
    }
}
