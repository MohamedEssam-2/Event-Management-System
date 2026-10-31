using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.RegistrationPdfDto
{
    public class RegistrationPdfDto
    {
        public int RegistrationId { get; set; }
        public string AttendeeName { get; set; } = null!;
        public string AttendeeEmail { get; set; } = null!;
        public string EventTitle { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; } = null!;
        public decimal TicketPrice { get; set; }
        public string? TicketCurrency { get; set; } 
        public string? Description { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegistrationStatus { get; set; } = null!;
        public string? RegistrationCreatedBy { get; set; }
    }
}
