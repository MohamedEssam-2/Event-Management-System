using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Business_Logic_Layer.DTO.RegistrationDTO
{
    public class ReadAllRegistrationDTO
    {
        public int Id { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}
