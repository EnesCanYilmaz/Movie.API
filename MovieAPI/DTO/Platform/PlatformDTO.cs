namespace MovieAPI.DTO.Platform;

public class PlatformDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Movie.MovieDTO> Movies { get; set; }
}