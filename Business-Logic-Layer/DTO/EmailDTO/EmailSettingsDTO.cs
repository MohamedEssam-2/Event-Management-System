using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.EmailDTO
{
    public class EmailSettingsDTO
    {
        public string SmtpHost { get; set; } = null!;
        public int SmtpPort { get; set; } 
        public bool SmtpUseSSL { get; set; }
        public string SmtpUser { get; set; } = null!;
        public string SmtpPassword { get; set; } = null!;
        public string FromName { get; set; } = null!;
    }
}
