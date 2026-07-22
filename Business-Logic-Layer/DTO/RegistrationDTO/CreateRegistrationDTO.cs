using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.RegistrationDTO
{
    public class CreateRegistrationDTO
    {
       
        public DateTime? RegistrationDate { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
    }
}
