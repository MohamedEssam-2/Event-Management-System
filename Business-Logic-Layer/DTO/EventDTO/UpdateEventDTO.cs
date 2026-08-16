using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;
using Microsoft.AspNetCore.Http;

namespace Business_Logic_Layer.DTO.EventDTO
{
    public class UpdateEventDTO
    {

        public string? Name { get; set; }

        public DateTime? Date { get; set; }

        public string? Location { get; set; }

        public int? MaxAttendees { get; set; }

        public decimal? Price { get; set; }
        public EventStatus? Status { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }



    }
}
