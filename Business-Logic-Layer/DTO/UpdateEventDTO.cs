using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO
{
    public class UpdateEventDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime? Date { get; set; }

        public string? Location { get; set; }

        public int? MaxAttendees { get; set; }

        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        public string? OrganizerId { get; set; }
    }
}
