using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.DTO.Platform;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Platform;

namespace MovieAPI.Controllers;

public class PlatformController : Controller
{ 
    private readonly MovieAPIDbContext _context;

    public PlatformController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var platforms = await _context.Platforms.Select(c => new PlatformDTO
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
                PlatformName = m.Platform.Name,
                PlatformId = m.Platform.Id,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
                MovieTime = m.MovieTime.ToString("t")
            }).ToList()
        }).ToListAsync();


        if (platforms is null) return NotFound();

        return Ok(platforms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var platforms = await _context.Platforms.Select(c => new PlatformDTO
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
                PlatformName = m.Platform.Name,
                PlatformId = m.Platform.Id,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy")
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);


        if (platforms is null) return NotFound();

        return Ok(platforms);
    }

    [HttpPost]
    public async Task<IActionResult> Post(string platformName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = new Platform
        {
            Name = platformName,
            CreatedDate = DateTime.Now
        };

        await _context.Platforms.AddAsync(platform);
        await _context.SaveChangesAsync();

        return Ok(platform);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, string platformName)
    {
        var existingPlatform = await _context.Platforms.FindAsync(id);
        if (existingPlatform is null) return NotFound();

        if (string.IsNullOrWhiteSpace(platformName)) return BadRequest("Platform name cannot be empty.");

        existingPlatform.Name = platformName;
        await _context.SaveChangesAsync();

        return Ok(existingPlatform);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var platform = await _context.Platforms.FirstOrDefaultAsync(c => c.Id == id);
        if (platform is null) return NotFound();

        _context.Platforms.Remove(platform);
        await _context.SaveChangesAsync();

        return Ok(platform);
    }
}