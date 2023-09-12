using MovieAPI.Infrastructure.Data.Base;

namespace MovieAPI.Infrastructure.Data.Entities.ProductImage;

public class ProductImage : BaseEntity
{
    public string? FileName { get; set; }
    public string? Path { get; set; }
    
    public ICollection<Movie.Movie> Movies { get; set; }
}