using System;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
	public class CategoryModel
	{
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

