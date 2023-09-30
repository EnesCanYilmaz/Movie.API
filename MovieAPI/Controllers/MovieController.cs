
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.DTO.Director;
using MovieAPI.DTO.Platform;
using MovieAPI.DTO.Player;
using MovieAPI.FileRename;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Category;
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

    [HttpGet("[action]")]
    public async Task<ActionResult> GetAll()
    {
        var movies = await _context.Movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            CategoryName = m.Category.Name,
            ReleaseDate = m.ReleaseDate.ToString("dd/mm/yyyy")

        }).ToListAsync();

        return movies is null ? Ok(movies) : StatusCode(500, "Movies not found");
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult> GetByMovieId(int id)
    {
        var movie = await _context.Movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            CategoryId = m.Category.Id,
            CategoryName = m.Category.Name,
            PlatformId = m.Platform.Id,
            PlatformName = m.Platform.Name,
            ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
            MovieTime = m.MovieTime.ToString("hh/mm"),
            Directors = m.Directors.Select(d => new DirectorDTO
            {
                Name = d.Name
            }).ToList(),
            Players = m.Players.Select(p => new PlayerDTO
            {
                Name = p.Name
            }).ToList()

        }).FirstOrDefaultAsync(m => m.Id == id);


        return movie is not null
            ? Ok(movie)
            : NotFound("Movie Id not found!");
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<Movie>> CreateMovie([FromBody] MovieDTO movieDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var movie = new Movie
        {
            Name = movieDTO.Name,
            Description = movieDTO.Description,
            ReleaseDate = Convert.ToDateTime(movieDTO.ReleaseDate),
            CategoryId = movieDTO.CategoryId,
            PlatformId = movieDTO.PlatformId,
            MovieTime = Convert.ToDateTime(movieDTO.MovieTime)
        };

        await _context.Movies.AddAsync(movie);
        return await _context.SaveChangesAsync() > 0 ? Ok("Movie Added") : StatusCode(500, "Movie not added");
    }


    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDTO updatedMovie)
    {
        var existingMovies = await _context.Movies.FindAsync(id);

        if (existingMovies is null)
            return NotFound("Movie not found!");

        existingMovies.Name = updatedMovie.Name;
        existingMovies.Description = updatedMovie.Description;
        existingMovies.ReleaseDate = Convert.ToDateTime(updatedMovie.ReleaseDate);
        existingMovies.CategoryId = updatedMovie.CategoryId;
        existingMovies.PlatformId = updatedMovie.PlatformId;
        existingMovies.MovieTime = Convert.ToDateTime(updatedMovie.MovieTime);

        return await _context.SaveChangesAsync() > 0
            ? Ok("Movie Updated!")
            : StatusCode(500, "Movie not Updated!");
    }


    [HttpDelete("[action]/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
            return NotFound("Movie not found");

        _context.Movies.Remove(movie);

        return await _context.SaveChangesAsync() > 0 ? Ok("Movie Deleted!") : StatusCode(500, "Movie not deleted");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UploadPhoto(int id, IFormFileCollection? Files)
    {

        List<(string fileName, string pathOrContainerName)>? result = await
            _fileService.UploadAsync("photo", Files);

        if (result is null)
            return BadRequest("Photo not upload");

        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
            return NotFound("Movie not found");

        await _context.ProductImages.AddRangeAsync(result.Select(r => new ProductImage
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Movies = new List<Movie>() { movie }
        }).ToList());

        return await _context.SaveChangesAsync() > 0 ? Ok("Movie photo added!") : StatusCode(500, "Movie photo not added!");
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

