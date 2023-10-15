namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class DirectorController : BaseAPIController
{
    private readonly MovieAPIDbContext _context;

    public DirectorController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateDirectors(CreateDirectorDTO createDirectorDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var movie = await _context.Movies.FindAsync(createDirectorDTO.Id);

        if (movie is null)
            return NotFound("Movie not found!");

        var directors = createDirectorDTO.DirectorNames.Select(request => new Director
        {
            MovieId = movie.Id,
            Name = request
        }).ToList();

        await _context.Directors.AddRangeAsync(directors);

        var directorAddedResult = await _context.SaveChangesAsync();

        var directorDto = await _context.Directors.Where(x => x.MovieId == createDirectorDTO.Id).Select(x => new ListDirectorDTO
        {
            Id = x.Id,
            Name = x.Name,
            MovieId = x.MovieId,
            CreatedDate = x.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = x.UpdatedDate.ToString("dd.MM.yyyy HH:mm")
        }).ToListAsync();

        return directorAddedResult > 0
            ? OK(200, "Directors added!", directorDto)
            : StatusCode(500, "Directors not added!");
    }

    [HttpGet("[action]/{movieId}")]
    public async Task<IActionResult> GetDirectorsByMovieId(int movieId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var directors = await _context.Directors
            .Where(p => p.MovieId == movieId)
            .Select(p => new ListDirectorDTO
            {
                Id = p.Id,
                MovieId = movieId,
                Name = p.Name
            })
        .ToListAsync();

        var directorDto = await _context.Directors.Where(x => x.MovieId == movieId).Select(x => new ListDirectorDTO
        {
            Id = x.Id,
            Name = x.Name,
            MovieId = x.MovieId,
            CreatedDate = x.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = x.UpdatedDate.ToString("dd.MM.yyyy HH:mm")
        }).ToListAsync();

        return directors is not null
            ? OK(200, "Director listed by id!", directorDto)
            : NotFound("Director Not Found");
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteDirector(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var director = await _context.Directors
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        if (director is null)
            NotFound("Player not found");

        _context.Directors.Remove(director);

        return await _context.SaveChangesAsync() > 0
                            ? OK(200, "Deleted!", "Director deleted!")
                            : StatusCode(500, "Director not deleted");
    }
}

