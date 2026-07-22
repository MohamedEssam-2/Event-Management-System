using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.CategoryExceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Service.Implementation
{
    public class EventService(IUnitOfWork _unitOfWork, IMapper _mapper , UserManager<ApplicationUser> _userManager) : IEventService
    {
        public async Task<ReadAllEventDTO> CreateEvent(CreateEventDTO eventDTO)
        {

            var user = await _userManager.FindByIdAsync(eventDTO.OrganizerId);
            if (user == null)
            {
                throw new UserNotFoundException(eventDTO.OrganizerId);
            }

            var Category =await _unitOfWork.GetRepository<Category,int>().GetById(eventDTO.CategoryId);
            if (Category == null)
            {
                throw new CategoryNotFoundException(eventDTO.CategoryId);
            }

            var entity = _mapper.Map<Event>(eventDTO);
            var CreateEntity = await _unitOfWork.GetRepository<Event, int>().Create(entity);
            //CreateEntity.CreatedBy = eventDTO.CreatedBy;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ReadAllEventDTO>(CreateEntity);
        }

        public async Task<bool> DeleteEvent(int id)
        {
           var entity=await _unitOfWork.GetRepository<Event,int>().GetById(id);
            if (entity is null) 
            {
                throw new EventNotFoundException(id);
            }
            entity.IsDeleted = true;
            _unitOfWork.GetRepository<Event, int>().Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }

        public async Task<List<ReadAllEventDTO>> GetAllEvents()
        {
            var Events=await _unitOfWork.GetRepository<Event, int>().GetAll();
            if (!Events.Any())
            {
                throw new Exception("No Events Found");
            }
            var EventDTO=_mapper.Map<List<ReadAllEventDTO>>(Events);
            return EventDTO;
        }

        public async Task<ReadAllEventDTO> GetEventById(int id)
        {
         var Evetns = await _unitOfWork.GetRepository<Event, int>().GetById(id);
            if (Evetns is null)
            {
                throw new EventNotFoundException(id);
            }
            var EventDTO =  _mapper.Map<ReadAllEventDTO>(Evetns);
            return EventDTO;
        }

        public async Task<ReadAllEventDTO> UpdateEvent(int id, UpdateEventDTO eventDTO)
        {
            var entity = await _unitOfWork.GetRepository<Event, int>().GetById(id);
            if (entity is null)
            {
                throw new EventNotFoundException(id);
            }
            if (entity.IsDeleted)
            {
                throw new EventDeletedException(id);
            }

            if (eventDTO.Name != null)
                entity.Name = eventDTO.Name;

            if (eventDTO.Date.HasValue)
                entity.Date = eventDTO.Date.Value;

            if (eventDTO.Location != null)
                entity.Location = eventDTO.Location;

            if (eventDTO.MaxAttendees.HasValue)
                entity.MaxAttendees = eventDTO.MaxAttendees.Value;

            if (eventDTO.Price.HasValue)
                entity.Price = eventDTO.Price.Value;

            //entity.UpdatedBy

            _unitOfWork.GetRepository<Event, int>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadAllEventDTO>(entity);
        }
    }
}
