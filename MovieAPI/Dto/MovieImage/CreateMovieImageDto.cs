namespace MovieAPI.DTO.MovieImage;

public class CreateMovieImageDto
{
    public int Id { get; set; }
    public IFormFileCollection? Files { get; set; }
}