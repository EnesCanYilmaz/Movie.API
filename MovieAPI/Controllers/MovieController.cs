using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO.DirectorDTO;
using MovieAPI.DTO.MovieDTO;
using MovieAPI.DTO.PlayerDTO;
using MovieAPI.FileRename;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Movie;
using MovieAPI.Infrastructure.Data.Entities.MovieImage;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class MovieController : BaseAPIController
{
    private readonly MovieAPIDbContext _context;
    private readonly IFileService _fileService;

    public MovieController(MovieAPIDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _context.Movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            CategoryId = m.CategoryId,
            CategoryName = m.Category.Name,
            PlatformId = m.PlatformId,
            PlatformName = m.Platform.Name,
            ReleaseDate = m.ReleaseDate.ToString("dd/MM/yyyy"),
            MovieTime = m.MovieTime,
            Players = m.Players.Select(p => new PlayerDTO
            {
                Name = p.Name
            }).ToList(),
            Directors = m.Directors.Select(p => new DirectorDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToList()
        }).ToListAsync();

        return movies is not null
            ? OK(200,"Movies Listed!",movies)
            : StatusCode(500, "Movies not found");
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetByMovieId(int id)
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
            MovieTime = m.MovieTime,
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
            ? OK(200, "Movie list by id!", movie)
            : NotFound("Movie Id not found!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateMovie([FromBody] DTO.MovieDTO.CreateMovieDTO createMovieDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var movie = new Movie
        {
            Name = createMovieDTO.Name,
            Description = createMovieDTO.Description,
            ReleaseDate = Convert.ToDateTime(createMovieDTO.ReleaseDate),
            CategoryId = createMovieDTO.CategoryId,
            PlatformId = createMovieDTO.PlatformId,
            MovieTime = createMovieDTO.MovieTime
        };

        await _context.Movies.AddAsync(movie);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Movie Added!", movie)
            : StatusCode(500, "Movie not added!");
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
        existingMovies.MovieTime = updatedMovie.MovieTime;

        return await _context.SaveChangesAsync() > 0
            ? OK(200,"Movie Updated!", existingMovies)
            : StatusCode(500, "Movie not Updated!");
    }


    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
            return NotFound("Movie not found");

        _context.Movies.Remove(movie);

        return await _context.SaveChangesAsync() > 0
            ? OK(200,"Movie Deleted!",movie)
            : StatusCode(500, "Movie not deleted");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UploadPhoto(int id, IFormFileCollection? files)
    {

        List<(string fileName, string pathOrContainerName)>? result = await
            _fileService.UploadAsync("photo", files);

        if (result is null)
            return BadRequest("Photo not upload");

        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
            return NotFound("Movie not found");

        await _context.MovieImages.AddRangeAsync(result.Select(r => new MovieImage
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            MovieId = movie.Id
        }).ToList());

        return await _context.SaveChangesAsync() > 0
            ? OK(200,"Movie photo added!",movie)
            : StatusCode(500, "Movie photo not added!");
    }
}

