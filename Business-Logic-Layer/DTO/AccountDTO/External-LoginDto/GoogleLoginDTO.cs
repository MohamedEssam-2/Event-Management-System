using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.AccountDTO.External_LoginDto
{
    public class GoogleLoginDTO
    {
        [Required]
        public string IdToken { get; set; } = null!;
    }
}
