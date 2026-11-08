using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO.External_LoginDto;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IExternalLoginService
    {
        Task<ExternalUserInfoDTO?> ValidateGoogleTokenAsync(string idToken);
    }
}
