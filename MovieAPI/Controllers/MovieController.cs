using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTO;
using MovieAPI.Infrastructure.Data.Context;
using MovieAPI.Infrastructure.Data.Entities.Movie;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
public class MovieController : Controller
{
    private readonly MovieAPIDbContext _context;

    public MovieController(MovieAPIDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var movies = await _context.Movies.ToListAsync();
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
            VisionDate = Convert.ToDateTime(movieDTO.ReleaseDate),
            Director = movieDTO.Director,
            CategoryId = movieDTO.CategoryId,
        };

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return Ok(movie);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Movie updatedMovie)
    {
        if (id != updatedMovie.Id)
        {
            return BadRequest();
        }

        _context.Entry(updatedMovie).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
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
}

