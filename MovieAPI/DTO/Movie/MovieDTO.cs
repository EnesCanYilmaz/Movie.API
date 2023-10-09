namespace MovieAPI.DTO.Movie;

public class MovieDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ReleaseDate { get; set; }
    public string? MovieTime { get; set; }
    public string? CreatedDate { get; set; }
    public string? UpdatedDate { get; set; }
    public string? PlatformName { get; set; }
    public string? CategoryName { get; set; }
    public List<PlayerDTO>? Players { get; set; }
    public List<DirectorDTO>? Directors { get; set; }
    public List<MovieImageDTO>? MovieImages { get; set; }
}
