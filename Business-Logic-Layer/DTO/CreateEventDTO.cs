using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO
{
    public class CreateEventDTO
    {
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public int? MaxAttendees { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string OrganizerId { get; set; } = null!;
    }
}
