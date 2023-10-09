namespace MovieAPI.Infrastructure.Data.Entities.Category;

public class Category : BaseEntity
{
    public string? Name { get; set; }

    public ICollection<Movie.Movie>? Movies { get; set; }
}

