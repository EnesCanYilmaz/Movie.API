using System;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAPI.DTO.Category
{
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

