using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.DTO.MovieDTO;
using MovieAPI.DTO.Platform;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Category;
using MovieAPI.Infrastructure.Data.Entities.Platform;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class PlatformController : BaseAPIController
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
            ? OK(200, "All platforms listed!", platforms)
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
            ? OK(200, "Platform listed by id!", platform)
         : NotFound("Platform Id not found!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreatePlatform(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = new Platform
        {
            Name = name,
            CreatedDate = DateTime.Now
        };

        await _context.Platforms.AddAsync(platform);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Category added!", platform)
            : StatusCode(500, "Platform not Added!");
    }

    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> UpdatePlatform(int id, string name)
    {
        var existingPlatform = await _context.Platforms.FindAsync(id);

        if (existingPlatform is null)
            return NotFound("Platform not found!");

        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Platform Name cannot be empty.");

        existingPlatform.Name = platformName;
        existingPlatform.UpdatedDate = DateTime.Now;

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Platform updated!", existingPlatform)
            : NotFound("Platform not Updated!");
    }


    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeletePlatform(int id)
    {
        var platform = await _context.Platforms.FirstOrDefaultAsync(c => c.Id == id);

        if (platform is null)
            return NotFound("Platform not found!");

        _context.Platforms.Remove(platform);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Platform deleted by id!", platform)
            : StatusCode(500, "Platform not Deleted");
    }
}