using MovieAPI.Infrastructure.Data.Base;

namespace MovieAPI.Infrastructure.Data.Entities.Director;

public class Director : BaseEntity
{
    public string? Name { get; set; }
    public int MovieId { get; set; }

    //Relationships
    public virtual Movie.Movie? Movie { get; set; }
}

