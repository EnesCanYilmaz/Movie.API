namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class PlatformController : BaseApiController
{
    private readonly MovieAPIDbContext _context;

    public PlatformController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllPlatforms()
    {
        var platforms = await _context.Platforms.Select(c => new ListPlatformDto { Id = c.Id, Name = c.Name, CreatedDate = c.CreatedDate.ToString("dd.MM.yyyy HH:mm"), UpdatedDate = c.UpdatedDate.ToString("dd.MM.yyyy HH:mm") }).ToListAsync();

        return OK(200, "All platforms listed!", platforms);
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetByPlatformId(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = await _context.Platforms.Select(c => new PlatformDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedDate = c.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            UpdatedDate = c.UpdatedDate.ToString("dd.MM.yyyy HH:mm"),
            Movies = c.Movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                CategoryName = m.Category.Name,
                PlatformName = m.Platform.Name,
                ReleaseDate = m.ReleaseDate.ToString("dd.MM.yyyy HH:mm"),
                MovieTime = m.MovieTime,
                Players = m.Players.Select(p => new PlayerDto { Id = p.Id, Name = p.Name }).ToList(),
                Directors = m.Directors.Select(d => new DirectorDto { Id = d.Id, Name = d.Name }).ToList(),
                MovieImages = m.MovieImages.Select(i => new MovieImageDto { Id = i.Id, FileName = i.FileName, Path = i.Path }).ToList(),
                CreatedDate = m.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                UpdatedDate = m.UpdatedDate.ToString("dd.MM.yyyy HH:mm")
            }).ToList()
        }).FirstOrDefaultAsync(c => c.Id == id);

        return OK(200, "Platform listed!", platform);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreatePlatform(string platformName)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = new Platform { Name = platformName, CreatedDate = DateTime.UtcNow };

        await _context.Platforms.AddAsync(platform);

        var addedPlatformResult = await _context.SaveChangesAsync();

        var platformDto = new CreatePlatformDto { Id = platform.Id, Name = platform.Name, CreatedDate = platform.CreatedDate.ToString("dd.MM.yyyy HH:mm") };

        return OK(200, "Platform added!", platformDto);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdatePlatform([FromBody] UpdatePlatformDto updatePlatformDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var existingPlatform = await _context.Platforms.FindAsync(updatePlatformDTO.Id);

        if (existingPlatform is null)
            return NotFound("Platform not found!");

        if (string.IsNullOrWhiteSpace(updatePlatformDTO.Name))
            return BadRequest("Platform name cannot be empty.");

        existingPlatform.Name = updatePlatformDTO.Name;
        existingPlatform.UpdatedDate = DateTime.UtcNow;

        var updatedCategoryResult = await _context.SaveChangesAsync();

        var listCategoryDTO = new ListPlatformDto { Id = existingPlatform.Id, Name = existingPlatform.Name, CreatedDate = existingPlatform.CreatedDate.ToString("dd.MM.yyyy HH:mm"), UpdatedDate = existingPlatform.UpdatedDate.ToString("dd.MM.yyyy HH:mm") };

        return OK(200, "Platform updated!", listCategoryDTO);
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeletePlatform(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var platform = await _context.Platforms.FirstOrDefaultAsync(c => c.Id == id);

        if (platform is null)
            return NotFound("Platform not found!");

        _context.Platforms.Remove(platform);

        return OK(200, "Platform deleted!", "All movies, actors, directors, photos related to the platform have been deleted!");
    }
}