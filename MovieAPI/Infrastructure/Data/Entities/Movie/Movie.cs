using MovieAPI.Infrastructure.Data.Entities.Base;

namespace MovieAPI.Infrastructure.Data.Entities.Movie;

public class Movie : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? MovieTime { get; set; }
    public int CategoryId { get; set; }
    public int PlatformId { get; set; }

    //Relationships
    public virtual Category.Category? Category { get; set; }
    public virtual Platform.Platform? Platform { get; set; }

    public ICollection<Player.Player>? Players { get; set; }
    public ICollection<Director.Director>? Directors { get; set; }
    public ICollection<MovieImage.MovieImage>? MovieImages { get; set; }
}