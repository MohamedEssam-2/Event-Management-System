using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.EventDTO
{
    public class ReadAllEventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public int? MaxAttendees { get; set; }
        public decimal Price { get; set; }
        public int Category_id { get; set; }
        public string Category_name { get; set; } = null!;
        public string? ImageUrl { get; set; }


    }
}
