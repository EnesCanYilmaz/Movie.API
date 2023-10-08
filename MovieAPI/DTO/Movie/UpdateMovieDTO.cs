namespace MovieAPI.DTO.Movie;

public class UpdateMovieDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ReleaseDate { get; set; }
    public string MovieTime { get; set; }
    public int CategoryId { get; set; }
    public int PlatformId { get; set; }
}

