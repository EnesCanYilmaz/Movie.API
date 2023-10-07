using System.Text.Json.Serialization;
using MovieAPI.DTO.DirectorDTO;
using MovieAPI.DTO.Platform;

namespace MovieAPI.DTO.MovieDTO;

public class MovieDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ReleaseDate { get; set; }
    public string MovieTime { get; set; }
    public int CategoryId { get; set; }
    public int PlatformId { get; set; }
    public string PlatformName { get; set; }
    public string CategoryName { get; set; }
    public List<PlayerDTO.PlayerDTO> Players { get; set; }
    public List<DirectorDTO.DirectorDTO> Directors { get; set; }
    public List<MovieImageDTO.MovieImageDTO> MovieImages { get; set; }
}
