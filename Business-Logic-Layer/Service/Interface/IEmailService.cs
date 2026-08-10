using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string To, string subject, string Msg);
    }
}
