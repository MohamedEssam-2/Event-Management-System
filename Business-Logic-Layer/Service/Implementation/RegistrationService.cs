using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.RegistrationDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace Business_Logic_Layer.Service.Implementation
{
    public class RegistrationService(UnitOfWork _unitOfWork , IMapper _mapper, UserManager<ApplicationUser>_userManager ) : IRegistrationService
    {
        public async Task<ServiceResponse<int>> CreateRegistration(CreateRegistrationDTO registrationDTO)
        {
            var user= await _userManager.FindByIdAsync(registrationDTO.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(registrationDTO.UserId);
            }

            var eventRepo =  _unitOfWork.GetRepository<Event, int>();

            var eventEntity = await eventRepo.GetById(registrationDTO.EventId);

            if (eventEntity is null)
            {
                throw new EventNotFoundException(registrationDTO.EventId);
            }

            var registration = _mapper.Map<Registration>(registrationDTO);
            registration.CreatedAt = DateTime.Now;
            var registrationRepo=await _unitOfWork.GetRepository<Registration,int>().Create(registration);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = registration.Id,
                Success = true,
                Message = "Regisration Created Successfully"
            };
        }
    }
}
