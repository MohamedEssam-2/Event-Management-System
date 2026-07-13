using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.AccountDTO
{
    public class RegisterDTO
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!; 

        public int? Age { get; set; }
        //[Phone]
        //public string? Phone_Number { get; set; }
    }
}
