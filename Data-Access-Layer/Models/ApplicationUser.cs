using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data_Access_Layer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public int? Age { get; set; }
        public string? Phone_Number { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
