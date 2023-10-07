namespace MovieAPI.DTO.Category;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Movie.MovieDTO> Movies { get; set; }
}

