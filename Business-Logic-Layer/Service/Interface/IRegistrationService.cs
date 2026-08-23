using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.DTO.PaginationDTO;
using Business_Logic_Layer.DTO.RegistrationDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IRegistrationService
    {
        public Task<ServiceResponse<int>>CreateRegistration(RegistrationDTO registrationDTO);
        public Task<ServiceResponse<PagedResultDTO<ReadAllRegistrationDTO>>> GetAllRegistration(int PageIndex , int PageSize, string? sortBy);
        public Task<ServiceResponse<ReadAllRegistrationDTO>> GetRegistrationById(int id);
        //public Task<ServiceResponse<bool>> UpdateRegistration(int id, UpdateRegistrationDTO registrationDTO);
        public Task<ServiceResponse<bool>> DeleteRegistration(int id);
        public Task<ServiceResponse<List<ReadAllRegistrationDTO>>> GetMyRegistration();
        public Task<ServiceResponse<PagedResultDTO<ReadAllRegistrationDTO>>> GetRegistrationsByEventId(int eventId, int PageIndex, int PageSize);


    }
}
