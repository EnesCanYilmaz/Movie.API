namespace MovieAPI.DTO.MovieImage;

public class ListMovieImageDto
{
    public int MovieId { get; set; }
    public List<MovieImageDto>? Photos { get; set; }
    public string? CreatedDate { get; set; }
    public string? UpdatedDate { get; set; }
}

