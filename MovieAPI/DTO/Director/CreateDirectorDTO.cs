using System;
namespace MovieAPI.DTO.Director;

public class CreateDirectorDTO
{
    public int Id { get; set; }
    public List<string> DirectorNames { get; set; }
}

