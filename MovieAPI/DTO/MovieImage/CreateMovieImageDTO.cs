namespace MovieAPI.DTO.MovieImage;

public class CreateMovieImageDTO
{
    public int Id { get; set; }
    public IFormFileCollection? Files { get; set; }
}