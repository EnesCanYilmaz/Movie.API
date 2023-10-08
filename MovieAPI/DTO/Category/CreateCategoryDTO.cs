using System;
namespace MovieAPI.DTO.Category
{
	public class CreateCategoryDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

