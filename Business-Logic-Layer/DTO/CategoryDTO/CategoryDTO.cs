using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business_Logic_Layer.DTO.CategoryDTO
{
    public class CategoryDTO
    {
       [Required(ErrorMessage = "Name must be Enterd here .")]
       [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
       [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
    }
}
