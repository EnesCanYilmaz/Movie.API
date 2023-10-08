using System;
namespace MovieAPI.DTO.MovieImage
{
	public class ListMovieImageDTO
	{
		public int MovieId { get; set; }
		public List<string> FileNames { get; set; }
		public List<string> Paths { get; set; }
	}
}

