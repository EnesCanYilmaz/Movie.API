namespace MovieAPI.DTO.Movie;

public class ListMovieDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string MovieTime { get; set; }
    public string PlatformName { get; set; }
    public string CategoryName { get; set; }
}

