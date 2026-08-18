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
using Data_Access_Layer.Enum;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Implementation;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace Business_Logic_Layer.Service.Implementation
{
    public class RegistrationService(IUnitOfWork _unitOfWork , IMapper _mapper, UserManager<ApplicationUser>_userManager , ICurrentUserService _currentUser) : IRegistrationService
    {
        public async Task<ServiceResponse<int>> CreateRegistration(RegistrationDTO registrationDTO)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            var eventRepo =  _unitOfWork.GetRepository<Event, int>();

            var eventEntity = await eventRepo.GetById(registrationDTO.EventId);

            if (eventEntity is null)
            {
                throw new EventNotFoundException(registrationDTO.EventId);
            }
            if (eventEntity.IsDeleted)
            {
                throw new BadRequestException("Cannot register for a deleted event.");
            }
            //if (eventEntity.Date.Date < DateTime.UtcNow.Date)
            //{
            //    throw new BadRequestException("Cannot register for an event that has already occurred.");
            //}
            if (eventEntity.Status== EventStatus.Canceled)
            {
                throw new BadRequestException("Cannot register for an event that is not active.");
            }
            if (eventEntity.Status == EventStatus.Completed)
            {
                throw new BadRequestException("Cannot register for an event that is Completed.");
            }
            //if (eventEntity.Price > 0)
            //{
            //    throw new BadRequestException("Cannot register for a paid event.");
            //}
            var registrationSpec = new UserEventRegistrationSpecification(userId, registrationDTO.EventId);
            var existingUserRegistration = await _unitOfWork.GetRepository<Registration, int>().CountAsync(registrationSpec);
            if (existingUserRegistration > 0)
            {
                throw new BadRequestException("User is already registered for this event.");
            }

            var spec = new EventRegistrationsSpecification(registrationDTO.EventId);
            var existingRegistration = await _unitOfWork.GetRepository<Registration, int>().CountAsync(spec);
            if(eventEntity.MaxAttendees.HasValue  && existingRegistration >= eventEntity.MaxAttendees)
            {
                throw new BadRequestException("Event has reached its maximum number of attendees.");
            }

          
            var registration = _mapper.Map<Registration>(registrationDTO);
            registration.UserId = userId;
            registration.RegistrationDate = DateTime.UtcNow;
            registration.CreatedBy = _currentUser.FullName;
            registration.CreatedAt = DateTime.UtcNow;
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
            var spec = new AllRegistrationsSpecification();
            var registration = await _unitOfWork.GetRepository<Registration,int>().GetAll(spec);
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
            var spec = new RegistrationByIdSpecification(id);
            var registration = await _unitOfWork.GetRepository<Registration,int>().GetById(spec);
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

        //public async Task<ServiceResponse<bool>> UpdateRegistration(int id, UpdateRegistrationDTO registrationDTO)
        //{
        //    var registration =await _unitOfWork.GetRepository<Registration, int>().GetById(id);
        //    if (registration is null)
        //    {
        //        throw new RegistrationNotFoundException(id);
        //    }
        //    if(registrationDTO.RegistrationDate != null)
        //    {
        //        registration.RegistrationDate = registrationDTO.RegistrationDate.Value;
        //    }
        //    registration.UpdatedBy = _currentUser.FullName;
        //    registration.UpdatedAt = DateTime.Now;
        //    _unitOfWork.GetRepository<Registration, int>().Update(registration);
        //    await _unitOfWork.SaveChangesAsync();
        //    return new ServiceResponse<bool>
        //    {
        //        Data = true,
        //        Success = true,
        //        Message = "Registration updated successfully"
        //    };

        //}

        public async Task<ServiceResponse<bool>> DeleteRegistration(int id)
        {
            var registration = await _unitOfWork.GetRepository<Registration, int>().GetById(id);
            if (registration is null)
            {
                throw new RegistrationNotFoundException(id);
            }
            if (registration.UserId != _currentUser.UserId)
            {
                throw new UnauthorizedException(
                    "You are not authorized to cancel this registration.");
            }
            registration.DeletedBy = _currentUser.FullName;
            registration.DeletedDate = DateTime.UtcNow;
           _unitOfWork.GetRepository<Registration, int>().Delete(registration);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Data = true,
                Success = true,
                Message = "Registration deleted successfully"
            };

        }

        public async Task<ServiceResponse<List<ReadAllRegistrationDTO>>> GetMyRegistration()
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var spec = new UserRegistrationsSpecification(userId);
            var registrations = await _unitOfWork.GetRepository<Registration, int>().GetAll(spec);
            var registrationDTOs = _mapper.Map<List<ReadAllRegistrationDTO>>(registrations);

            return new ServiceResponse<List<ReadAllRegistrationDTO>>
            {
                Data = registrationDTOs,
                Success = true,
                Message = "Registrations retrieved successfully"
            };
        }
    }
}
