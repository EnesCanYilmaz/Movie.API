using System;
using MovieAPI.Infrastructure.Data.Base;

namespace MovieAPI.Infrastructure.Data.Entities.Movie;

public class Movie : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Director { get; set; }
    public DateTime MovieTime { get; set; }
    public int CategoryId { get; set; }
    public int PlatformId { get; set; }

    //Relationships
    public virtual Category.Category Category { get; set; }
    public virtual Platform.Platform Platform { get; set; }
}