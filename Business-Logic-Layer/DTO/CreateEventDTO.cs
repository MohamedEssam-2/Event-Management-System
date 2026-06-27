using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO
{
    public class CreateEventDTO
    {
        [Required(ErrorMessage="Name must be Enterd here .")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Date must be Enterd here .")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location must be Enterd here .")]
        public string Location { get; set; } = null!;

        public int? MaxAttendees { get; set; }

        [Required(ErrorMessage = "Price must be Enterd here .")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "CategoryId must be Enterd here .")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "OrganizerId must be Enterd here .")]
        public string OrganizerId { get; set; } = null!;
    }
}
