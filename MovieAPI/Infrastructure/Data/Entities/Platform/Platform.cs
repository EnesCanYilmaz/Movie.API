namespace MovieAPI.Infrastructure.Data.Entities.Platform;

public class Platform : BaseEntity
{
    public string? Name { get; set; }

    public ICollection<Movie.Movie>? Movies { get; set; }
}