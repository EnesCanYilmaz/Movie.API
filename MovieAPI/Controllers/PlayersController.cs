using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO.PlayerDTO;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Player;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : BaseAPIController
    {
        private readonly MovieAPIDbContext _context;

        public PlayersController(MovieAPIDbContext context)
        {
            _context = context;
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> CreatePlayers(int id, List<string> playerNames)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);

            if (movie is null)
                return NotFound("Movie not found!");

            var players = playerNames.Select(requestPlayer => new Player
            {
                MovieId = movie.Id,
                Name = requestPlayer
            }).ToList();

            await _context.Players.AddRangeAsync(players);

            return await _context.SaveChangesAsync() > 0
            ? OK(200, "Players added!", players)
                : StatusCode(500, "Players not added!");
        }

        [HttpGet("[action]/{movieId}")]
        public async Task<IActionResult> GetPlayersByMovieId(int movieId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var players = await _context.Players
                .Where(p => p.MovieId == movieId)
                .Select(p => new ListPlayerDTO
                {
                    Id = p.Id,
                    MovieId = movieId,
                    Name = p.Name
                })
            .ToListAsync();

            return players is not null
            ? OK(200, "Player listed by id!", players)
            : NotFound("Player Not Found");
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var player = await _context.Players
                .Where(p => p.Id == id).FirstOrDefaultAsync();

            if (player is null)
                NotFound("Player not found");

            _context.Players.Remove(player);

            return await _context.SaveChangesAsync() > 0
                ? OK(200, "Deleted player by id!", player)
                : StatusCode(500, "Player not deleted");
        }
    }
}

