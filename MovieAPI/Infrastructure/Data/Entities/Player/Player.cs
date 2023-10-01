using MovieAPI.Infrastructure.Data.Base;

namespace MovieAPI.Infrastructure.Data.Entities.Player;

public class Player : BaseEntity
{
	public string Name { get; set; }
    public int MovieId { get; set; }

    //Relationships
    public virtual Movie.Movie Movie { get; set; }
}

