using MovieAPI.DTO.MovieImage;

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
            CategoryName = m.Category.Name,
            PlatformName = m.Platform.Name,
            MovieTime = m.MovieTime,
            ReleaseDate = m.ReleaseDate.ToString("dd.MM.yyyy HH:mm"),
            CreatedDate = m.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = m.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
            Players = m.Players.Select(p => new PlayerDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToList(),
            Directors = m.Directors.Select(d => new DirectorDTO
            {
                Id = d.Id,
                Name = d.Name
            }).ToList(),
            MovieImages = m.MovieImages.Select(i => new MovieImageDTO
            {
                Id = i.Id,
                FileName = i.FileName,
                Path = i.Path
            }).ToList()
        }).ToListAsync();

        return movies is not null
            ? OK(200, "Movies Listed!", movies)
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
            CategoryName = m.Category.Name,
            PlatformName = m.Platform.Name,
            ReleaseDate = m.ReleaseDate.ToString("dd.MM.yyyy HH:mm"),
            CreatedDate = m.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = m.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
            MovieTime = m.MovieTime,
            Directors = m.Directors.Select(d => new DirectorDTO
            {
                Id = d.Id,
                Name = d.Name
            }).ToList(),
            Players = m.Players.Select(p => new PlayerDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToList(),
            MovieImages = m.MovieImages.Select(i => new MovieImageDTO
            {
                Id = i.Id,
                FileName = i.FileName,
                Path = i.Path
            }).ToList()
        }).FirstOrDefaultAsync(m => m.Id == id);


        return movie is not null
            ? OK(200, "Movie list!", movie)
            : NotFound("Movie not found!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDTO createMovieDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = await _context.Platforms.FindAsync(createMovieDTO.PlatformId);
        var category = await _context.Categories.FindAsync(createMovieDTO.CategoryId);

        if (platform == null || category == null)
            return BadRequest("Invalid platform or category ID.");

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

        var addedMovieResult = await _context.SaveChangesAsync();

        var movieDTO = new CreatedMovieListDTO
        {
            Id = movie.Id,
            Name = movie.Name,
            Description = movie.Description,
            PlatformName = platform.Name,
            CategoryName = category.Name,
            MovieTime = movie.MovieTime,
            ReleaseDate = movie.ReleaseDate.ToString("dd.MM.yyyy HH:mm"),
            CreatedDate = movie.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
        };

        return addedMovieResult > 0
            ? OK(200, "Movie added!", movieDTO)
            : StatusCode(500, "Movie not added!");
    }


    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieDTO updatedMovie)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var existingMovies = await _context.Movies.FindAsync(updatedMovie.Id);
        var platform = await _context.Platforms.FindAsync(updatedMovie.PlatformId);
        var category = await _context.Categories.FindAsync(updatedMovie.CategoryId);

        if (platform == null || category == null)
            return BadRequest("Invalid platform or category ID.");
        if (existingMovies is null)
            return NotFound("Movie not found!");

        existingMovies.Name = updatedMovie.Name;
        existingMovies.Description = updatedMovie.Description;
        existingMovies.ReleaseDate = Convert.ToDateTime(updatedMovie.ReleaseDate);
        existingMovies.CategoryId = updatedMovie.CategoryId;
        existingMovies.PlatformId = updatedMovie.PlatformId;
        existingMovies.MovieTime = updatedMovie.MovieTime;

        var updatedMovieResult = await _context.SaveChangesAsync();

        var movieDTO = new UpdatedMovieListDTO
        {
            Id = existingMovies.Id,
            Name = existingMovies.Name,
            Description = existingMovies.Description,
            PlatformName = platform.Name,
            CategoryName = category.Name,
            MovieTime = existingMovies.MovieTime,
            ReleaseDate = existingMovies.ReleaseDate.ToString("dd.MM.yyyy HH:mm"),
            CreatedDate = existingMovies.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = existingMovies.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
        };

        return updatedMovieResult > 0
            ? OK(200, "Movie updated!", movieDTO)
            : StatusCode(500, "Movie not updated!");
    }


    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
            return NotFound("Movie not found");

        _context.Movies.Remove(movie);

        return await _context.SaveChangesAsync() > 0
            ? OK(200, "Movie deleted!", "All actors, directors, photos related to the movies have been deleted")
            : StatusCode(500, "Movie not deleted!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UploadPhoto(CreateMovieImageDTO createMovieImageDTO)
    {
        List<(string fileName, string pathOrContainerName)>? result = await
            _fileService.UploadAsync("photo", createMovieImageDTO.Files);

        if (result is null)
            return BadRequest("Photo not upload!");

        var movie = await _context.Movies.FindAsync(createMovieImageDTO.Id);

        if (movie is null)
            return NotFound("Movie not found!");

        List<MovieImage> movieImagesToAdd = result.Select(r => new MovieImage
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            MovieId = movie.Id
        }).ToList();

        await _context.MovieImages.AddRangeAsync(movieImagesToAdd);
        var addedMovieImageResult = await _context.SaveChangesAsync();

        var movieImageDTO = new ListMovieImageDTO
        {
            MovieId = movie.Id,
            Photos = movie.MovieImages.Select(p => new MovieImageDTO
            {
                Id = p.Id,
                Path = p.Path,
                FileName = p.FileName
            }).ToList(),
            CreatedDate = movie.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = movie.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
        };

        return addedMovieImageResult > 0
            ? OK(200, "Movie photo added!", movieImageDTO)
            : StatusCode(500, "Movie photo not added!");
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetMoviePhotos(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var movie = await _context.Movies.FindAsync(id);

        var moviePhotos = await _context.Movies
            .Include(m => m.MovieImages)
            .FirstOrDefaultAsync(m => m.Id == id);

        var movieImageDTO = new ListMovieImageDTO
        {
            MovieId = moviePhotos.Id,
            Photos = movie.MovieImages.Select(p => new MovieImageDTO
            {
                Path = p.Path,
                FileName = p.FileName
            }).ToList()
        };

        return moviePhotos is not null
            ? OK(200, "Movie photos list!", movieImageDTO)
            : StatusCode(500, "Movie photos not found!");
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteMoviePhoto(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var movieImage = await _context.MovieImages.FindAsync(id);

        if (movieImage is null)
            return NotFound("Movie Photo not Found");

        _context.MovieImages.Remove(movieImage);

        var moviePhotoResult = await _context.SaveChangesAsync();

        return moviePhotoResult > 0
            ? OK(200, "Deleted!", "Movie photo deleted!")
            : StatusCode(500, "Movie photo not deleted!");
    }
}