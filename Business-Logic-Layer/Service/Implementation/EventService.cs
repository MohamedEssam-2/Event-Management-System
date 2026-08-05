using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.CategoryDTO;
using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.CategoryExceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Business_Logic_Layer.Service.Implementation
{
    public class EventService(IUnitOfWork _unitOfWork, IMapper _mapper , UserManager<ApplicationUser> _userManager , ICurrentUserService _currentUser , IUploadService _uplaodService) : IEventService
    {
        public async Task<ServiceResponse<int>> CreateEvent(CreateEventDTO eventDTO)
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
            if (eventDTO.Image is not null)
            {
                var photo = await _uplaodService.UploadImageAsync(eventDTO.Image,"EventManagement/Events");
                entity.ImageUrl = photo.Url;
                entity.PublicId = photo.PublicId;
            }
            var CreateEntity = await _unitOfWork.GetRepository<Event, int>().Create(entity);
            CreateEntity.CreatedBy = _currentUser.FullName;
            CreateEntity.CreatedAt = DateTime.UtcNow;   
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = CreateEntity.Id,
                Success = true,
                Message = "Event created successfully"
            };
        }

        public async Task<bool> DeleteEvent(int id)
        {
           var entity=await _unitOfWork.GetRepository<Event,int>().GetById(id);
            if (entity is null) 
            {
                throw new EventNotFoundException(id);
            }
            if (!string.IsNullOrWhiteSpace(entity.PublicId))
            {
                await _uplaodService.DeleteImageAsync(entity.PublicId);
            }
            entity.IsDeleted = true;
            _unitOfWork.GetRepository<Event, int>().Delete(entity);
            entity.DeletedBy = _currentUser.FullName;
            entity.DeletedDate = DateTime.UtcNow;
            return await _unitOfWork.SaveChangesAsync() > 0;

        }

        public async Task<ServiceResponse<List<ReadAllEventDTO>>> GetAllEvents(string? Search)
        {
            var spec = new EventWithCategorySpecification(Search);
            var Events=await _unitOfWork.GetRepository<Event, int>().GetAll(spec);
            if (Events == null || !Events.Any())
            {
                return new ServiceResponse<List<ReadAllEventDTO>>
                {
                    Success = false,
                    Message = "Cant Find Any Event ."
                };
            }
            var EventDTO=_mapper.Map<List<ReadAllEventDTO>>(Events);
           
            return new ServiceResponse<List<ReadAllEventDTO>>
            {
                Success = true,
                Data = EventDTO,
                Message = "Events Found Successfully"
            };
        }

        

        public async Task <ServiceResponse<ReadAllEventDTO>> GetEventById(int id)
        {
            var spec = new EventWithCategorySpecification(id);
         var Evetns = await _unitOfWork.GetRepository<Event, int>().GetById(spec);
            if (Evetns is null)
            {
                throw new EventNotFoundException(id);
            }
            var EventDTO =  _mapper.Map<ReadAllEventDTO>(Evetns);
            return new ServiceResponse<ReadAllEventDTO>
            {
                Success = true,
                Data = EventDTO,
                Message = "Category Found Successfully"
            };
        }



        public async Task<ReadAllEventDTO> UpdateEvent(int id, UpdateEventDTO eventDTO)
        {
            var spec = new EventWithCategorySpecification(id);
            var entity = await _unitOfWork.GetRepository<Event, int>().GetById(spec);
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

            var oldPublicId = entity.PublicId;

            if (eventDTO.Image is not null)
            {
                var photo = await _uplaodService.UploadImageAsync(eventDTO.Image, "EventManagement/Events");

                entity.ImageUrl = photo.Url;
                entity.PublicId = photo.PublicId;
            }
            //entity.UpdatedBy

            _unitOfWork.GetRepository<Event, int>().Update(entity);
            entity.UpdatedBy = _currentUser.FullName;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            if (eventDTO.Image is not null &&  !string.IsNullOrWhiteSpace(oldPublicId))
            {
                await _uplaodService.DeleteImageAsync(oldPublicId);
            }

            return _mapper.Map<ReadAllEventDTO>(entity);
        }

        public async Task<List<ReadAllEventDTO>> GetAllEventsInCategory(int categoryid)
        {
            var spec = new EventsByCategorySpecification(categoryid);
            var entity = await _unitOfWork.GetRepository<Event, int>().GetAll(spec);
            if (!entity.Any())
            {
                throw new Exception("No Events Found");
            }
            var EventDTO = _mapper.Map<List<ReadAllEventDTO>>(entity);
            return EventDTO;
        }
    }
}
