
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.FileRename;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Movie;
using MovieAPI.Infrastructure.Data.Entities.ProductImage;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class MovieController : Controller
{
    private readonly MovieAPIDbContext _context;
    private readonly IFileService _fileService;

    public MovieController(MovieAPIDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var movies = await _context.Movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            MovieName = m.Name,
            Description = m.Description,
            CategoryName = m.Category.Name,
            Director = m.Director,
            ReleaseDate = m.ReleaseDate.ToString("yy-MM-dd")
            
        }).ToListAsync();
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> Get(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie is null)
        {
            return NotFound();
        }
        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> Post([FromBody] MovieDTO movieDTO)
    {
        var movie = new Movie
        {
            Name = movieDTO.MovieName,
            Description = movieDTO.Description,
            ReleaseDate = Convert.ToDateTime(movieDTO.ReleaseDate),
            Director = movieDTO.Director,
            CategoryId = movieDTO.CategoryId,
            MovieTime = Convert.ToDateTime(movieDTO.MovieTime)
        };

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return Ok(movie);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Movie updatedMovie)
    {
        if (id != updatedMovie.Id)
            return BadRequest();

        _context.Movies.Update(updatedMovie);
        await _context.SaveChangesAsync();

        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
  
    [HttpPost("action")]
    public async Task<IActionResult> UploadPhoto(int id, IFormFileCollection? Files)
    {
        
        List<(string fileName, string pathOrContainerName)>? result = await
            _fileService.UploadAsync("photo", Files);
        
        var movie = await _context.Movies.FindAsync(id);

        await _context.ProductImages.AddRangeAsync(result.Select(r => new ProductImage
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Movies = new List<Movie>() { movie }
        }).ToList());
        
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("getPhotos")]
    public async Task<IActionResult> GetPhotos(int id)
    {
        var photos = await _context.ProductImages
            .Where(pi => pi.Movies.Any(m => m.Id == id))
            .Select(pi => new
            {
                FileName = pi.FileName,
                Path = pi.Path
            })
            .ToListAsync();

        if (photos.Count == 0)
        {
            return NotFound();
        }

        return Ok(photos);
    }
}

