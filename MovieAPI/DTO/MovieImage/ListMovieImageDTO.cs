namespace MovieAPI.DTO.MovieImage;

public class ListMovieImageDTO
{
    public int MovieId { get; set; }
    public List<MovieImageDTO>? Photos { get; set; }
    public string? CreatedDate { get; set; }
    public string? UpdatedDate { get; set; }
}

