namespace MovieAPI.DTO.Player;

public class ListPlayerDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string? Name { get; set; }
    public string? CreatedDate { get; set; }
    public string? UpdatedDate { get; set; }

}

