using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public IFormFile? Image { get; set; }
   


    }
}
