using System;
namespace MovieAPI.DTO;

public class MovieDTO
{
    public int Id { get; set; }
    public string MovieName { get; set; }
    public string Description { get; set; }
    public string ReleaseDate { get; set; }
    public string Director { get; set; }
    public string CategoryName { get; set; }
    public string MovieTime { get; set; }
    public int CategoryId { get; set; }
    public int PlatformId { get; set; }
    public string PlatformName { get; set; }
}
