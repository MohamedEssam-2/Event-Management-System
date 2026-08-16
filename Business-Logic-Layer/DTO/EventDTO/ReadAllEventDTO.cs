using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;

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
        public EventStatus? Status { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }


    }
}
