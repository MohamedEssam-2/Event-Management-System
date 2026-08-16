using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business_Logic_Layer.DTO.EventDTO
{
    public class CreateEventDTO
    {
        [Required(ErrorMessage = "Name must be Enterd here .")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Date must be Enterd here .")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location must be Enterd here .")]
        public string Location { get; set; } = null!;

        [Range(1,100000)]
        public int? MaxAttendees { get; set; }

        [Required(ErrorMessage = "Price must be Enterd here .")]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "CategoryId must be Enterd here .")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "OrganizerId must be Enterd here .")]
        public string OrganizerId { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
