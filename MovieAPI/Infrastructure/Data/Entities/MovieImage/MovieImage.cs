using MovieAPI.Infrastructure.Data.Entities.Base;

namespace MovieAPI.Infrastructure.Data.Entities.MovieImage;

public class MovieImage : BaseEntity
{
    public string? FileName { get; set; }
    public string? Path { get; set; }

    public int MovieId { get; set; }
    public virtual Movie.Movie? Movie { get; set; }
}