using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.RegistrationDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IRegistrationService
    {
        public Task<ServiceResponse<int>>CreateRegistration(CreateRegistrationDTO registrationDTO);
    }
}
