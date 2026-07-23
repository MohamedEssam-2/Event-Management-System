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
        public Task<ServiceResponse<int>>CreateRegistration(RegistrationDTO registrationDTO);
        public Task<ServiceResponse<List<ReadAllRegistrationDTO>>> GetAllRegistration();
        public Task<ServiceResponse<ReadAllRegistrationDTO>> GetRegistrationById(int id);
        public Task<ServiceResponse<bool>> UpdateRegistration(int id, UpdateRegistrationDTO registrationDTO);
        public Task<ServiceResponse<bool>> DeleteRegistration(int id);


    }
}
