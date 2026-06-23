using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Event : BaseEntity<int>
    {
       
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public int? MaxAttendees { get; set; }
        public decimal Price { get; set; }

        // Foreign key to Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Foreign key to ApplicationUser (Organizer)
        public string OrganizerId { get; set; }
        public ApplicationUser Organizer { get; set; } = null!;

        // Navigation property for registrations
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
