using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.RegistrationDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.RegistrationExceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Implementation;
using Data_Access_Layer.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace Business_Logic_Layer.Service.Implementation
{
    public class RegistrationService(IUnitOfWork _unitOfWork , IMapper _mapper, UserManager<ApplicationUser>_userManager , ICurrentUserService _currentUser) : IRegistrationService
    {
        public async Task<ServiceResponse<int>> CreateRegistration(RegistrationDTO registrationDTO)
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
            registration.CreatedBy = _currentUser.FullName;
            registration.CreatedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Registration,int>().Create(registration);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = registration.Id,
                Success = true,
                Message = "Regisration Created Successfully"
            };
        }

      

        public async Task<ServiceResponse<List<ReadAllRegistrationDTO>>> GetAllRegistration()
        {
           var registration = await _unitOfWork.GetRepository<Registration,int>().GetAll();
            if (!registration.Any())
            {
                return new ServiceResponse<List<ReadAllRegistrationDTO>>
                {
                    Success = true,
                    Data = [],
                    Message = "No Registration Found"
                };
            }

            var registrationDTOs = _mapper.Map<List<ReadAllRegistrationDTO>>(registration);

            return new ServiceResponse<List<ReadAllRegistrationDTO>>
            {
                Data = registrationDTOs,
                Success = true,
                Message = "Registrations retrieved successfully"
            };

        }

        public async Task<ServiceResponse<ReadAllRegistrationDTO>> GetRegistrationById(int id)
        {
            var registration = await _unitOfWork.GetRepository<Registration,int>().GetById(id);
            if(registration is null)
            {
                throw new RegistrationNotFoundException(id);
            }
            var registrationDTO = _mapper.Map<ReadAllRegistrationDTO>(registration);
            return new ServiceResponse<ReadAllRegistrationDTO>
            {
                Data = registrationDTO,
                Success = true,
                Message = "Registration retrieved successfully"
            };
        }

        public async Task<ServiceResponse<bool>> UpdateRegistration(int id, UpdateRegistrationDTO registrationDTO)
        {
            var registration =await _unitOfWork.GetRepository<Registration, int>().GetById(id);
            if (registration is null)
            {
                throw new RegistrationNotFoundException(id);
            }
            if(registrationDTO.RegistrationDate != null)
            {
                registration.RegistrationDate = registrationDTO.RegistrationDate.Value;
            }
            registration.UpdatedBy = _currentUser.FullName;
            registration.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Registration, int>().Update(registration);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Success = true,
                Message = "Registration updated successfully"
            };

        }

        public async Task<ServiceResponse<bool>> DeleteRegistration(int id)
        {
            var registration = await _unitOfWork.GetRepository<Registration, int>().GetById(id);
            if (registration is null)
            {
                throw new RegistrationNotFoundException(id);
            }
            registration.DeletedBy = _currentUser.FullName;
            registration.DeletedDate = DateTime.Now;
           _unitOfWork.GetRepository<Registration, int>().Delete(registration);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Success = true,
                Message = "Registration deleted successfully"
            };

        }
    }
}
