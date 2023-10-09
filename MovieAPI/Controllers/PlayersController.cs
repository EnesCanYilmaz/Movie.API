using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO.Player;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Movie;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePlayers(CreatePlayerDTO createPlayerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(createPlayerDTO.Id);

            if (movie is null)
                return NotFound("Movie not found!");

            List<Player> players = createPlayerDTO.PlayerNames.Select(players => new Player
            {
                MovieId = movie.Id,
                Name = players,
                CreatedDate = DateTime.UtcNow
            }).ToList();

            await _context.Players.AddRangeAsync(players);
            var playerAddedResult = await _context.SaveChangesAsync();

            var playersDto = await _context.Players.Where(x => x.MovieId == createPlayerDTO.Id).Select(x => new ListPlayerDTO
            {
                Id = x.Id,
                Name = x.Name,
                MovieId = x.MovieId,
                CreatedDate = x.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                UpdatedDate = x.UpdatedDate.ToString("dd.MM.yyyy HH:mm")
            }).ToListAsync();

            return playerAddedResult > 0
            ? OK(200, "Players added!", playersDto)
            : StatusCode(500, "Players not added!");
        }

        [HttpGet("[action]/{movieId}")]
        public async Task<IActionResult> GetPlayersByMovieId(int movieId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var players = await _context.Players.Where(x => x.MovieId == movieId).Select(x => new ListPlayerDTO
            {
                Id = x.Id,
                Name = x.Name,
                MovieId = x.MovieId,
                CreatedDate = x.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                UpdatedDate = x.UpdatedDate.ToString("dd.MM.yyyy HH:mm")
            }).ToListAsync();

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
                ? OK(200, "Deleted!", "Player deleted!")
                : StatusCode(500, "Player not deleted!");
        }
    }
}

