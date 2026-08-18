using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Review :BaseEntity<int>
    {
        public int Rating { get; set; }

        public string? Comment { get; set; }

        // Foreign key to Event
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        // Foreign key to ApplicationUser
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
