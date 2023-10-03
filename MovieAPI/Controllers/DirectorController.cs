using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO.DirectorDTO;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Director;
using MovieAPI.Infrastructure.Data.Entities.Player;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    public class DirectorController : BaseAPIController
    {
        private readonly MovieAPIDbContext _context;

        public DirectorController(MovieAPIDbContext context)
        {
            _context = context;
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> CreateDirectors(int id, List<string> directorNames)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);

            if (movie is null)
                return NotFound("Movie not found!");

            var directors = directorNames.Select(request => new Director
            {
                MovieId = movie.Id,
                Name = request
            }).ToList();

            await _context.Directors.AddRangeAsync(directors);

            return await _context.SaveChangesAsync() > 0
                ? OK(200, "Directors added!", directors)
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

            return directors is not null
                ? OK(200, "Director listed by id!", directors)
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
                                ? OK(200, "Deleted director by id!", director)
                                : StatusCode(500, "Director not deleted");
        }
    }
}

