using System;
namespace MovieAPI.DTO;

public class MovieDTO
{
    public int Id { get; set; }
    public string MovieName { get; set; }
    public string Description { get; set; }
    public string ReleaseDate { get; set; }
    public string Director { get; set; }
    public int CategoryId { get; set; }

}
