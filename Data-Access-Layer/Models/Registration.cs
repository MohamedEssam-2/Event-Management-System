using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Registration : BaseEntity<int>
    {
   
        public DateTime? RegistrationDate { get; set; }

        // Foreign key to Event
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        // Foreign key to ApplicationUser (Attendee)
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
       
    }
}
