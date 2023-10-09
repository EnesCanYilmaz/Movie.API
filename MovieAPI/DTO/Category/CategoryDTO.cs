namespace MovieAPI.DTO.Category;

public class CategoryDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CreatedDate { get; set; }
    public string? UpdatedDate { get; set; }
    public List<MovieDTO>? Movies { get; set; }
}

