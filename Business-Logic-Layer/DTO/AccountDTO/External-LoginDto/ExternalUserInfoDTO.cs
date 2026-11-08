using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.AccountDTO.External_LoginDto
{
    public class ExternalUserInfoDTO
    {
        public string Email { get; set; } = null!;
        public bool EmailVerified { get; set; }
        public string FullName { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
    }
}
