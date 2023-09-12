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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Movies = c.Movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                MovieName = m.Name,
                Description = m.Description,
                CategoryId = m.Category.Id,
                CategoryName = m.Category.Name,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
                MovieTime = m.MovieTime.ToString("t")
            }).ToList()
        }).ToListAsync();


        if (categories is null) return NotFound();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var categories = await _context.Categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Movies = c.Movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                MovieName = m.Name,
                Description = m.Description,
                CategoryId = m.Category.Id,
                CategoryName = m.Category.Name,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy")
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);


        if (categories is null) return NotFound();

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Post(string categoryName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = new Category
        {
            Name = categoryName,
            CreatedDate = DateTime.Now
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, string categoryName)
    {
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory is null) return NotFound();

        if (string.IsNullOrWhiteSpace(categoryName)) return BadRequest("Category name cannot be empty.");

        existingCategory.Name = categoryName;
        await _context.SaveChangesAsync();

        return Ok(existingCategory);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
}
