using System;
namespace MovieAPI.DTO.Category
{
	public class GetAllCategoryDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

