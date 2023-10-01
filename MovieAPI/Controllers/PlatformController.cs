using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.DTO.MovieDTO;
using MovieAPI.DTO.Platform;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Platform;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class PlatformController : Controller
{ 
    private readonly MovieAPIDbContext _context;

    public PlatformController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllPlatforms()
    {
        var platforms = await _context.Platforms.Select(c => new PlatformDTO
        {
            Id = c.Id,
            Name = c.Name,
        }).ToListAsync();

        return platforms is not null
            ? Ok(platforms)
            : NotFound("Platforms not found!");
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetByPlatformId(int id)
    {
        var platform = await _context.Platforms.Select(c => new PlatformDTO
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


        return platform is not null
         ? Ok(platform)
         : NotFound("Platform Id not found!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreatePlatform(string platformName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = new Platform
        {
            Name = platformName,
            CreatedDate = DateTime.Now
        };

        await _context.Platforms.AddAsync(platform);

        return await _context.SaveChangesAsync() > 0
            ? StatusCode(200, "Platform Added")
            : StatusCode(500, "Platform not Added!");
    }

    [HttpPut("[action]/{id}")]
    public async Task<ActionResult> UpdatePlatform(int id, string platformName)
    {
        var existingPlatform = await _context.Platforms.FindAsync(id);

        if (existingPlatform is null)
            return NotFound("Platform not found!");

        if (string.IsNullOrWhiteSpace(platformName))
            return BadRequest("Platform Name cannot be empty.");

        existingPlatform.Name = platformName;
        existingPlatform.UpdatedDate = DateTime.Now;

        return await _context.SaveChangesAsync() > 0
            ? Ok(existingPlatform)
            : NotFound("Category not Updated!");
    }


    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeletePlatform(int id)
    {
        var platform = await _context.Platforms.FirstOrDefaultAsync(c => c.Id == id);

        if (platform is null)
            return NotFound("Platform not found!");

        _context.Platforms.Remove(platform);

        return await _context.SaveChangesAsync() > 0
            ? Ok("Platform Deleted!")
            : StatusCode(500, "Platforn not Deleted");
    }
}