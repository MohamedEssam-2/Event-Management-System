using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.CategoryDTO
{
    public class CategoryDTO
    {
       [Required(ErrorMessage = "Name must be Enterd here .")]
        public string Name { get; set; } = null!;
    }
}
