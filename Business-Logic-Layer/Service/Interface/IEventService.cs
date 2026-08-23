using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.AccountDTO;
using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.DTO.PaginationDTO;
using Microsoft.AspNetCore.Mvc;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IEventService
    {
        public Task<ServiceResponse<PagedResultDTO<ReadAllEventDTO>>> GetAllEvents(string ?Search , int PageIndex, int PageSize,string? sortBy);
        public Task<ServiceResponse<ReadAllEventDTO>> GetEventById(int id);
        public Task<ServiceResponse<int>> CreateEvent(CreateEventDTO eventDTO);
        public Task<bool> DeleteEvent(int id);
        public Task<ReadAllEventDTO> UpdateEvent(int id, UpdateEventDTO eventDTO);
        public Task<List<ReadAllEventDTO>> GetAllEventsInCategory(int categoryid);
        public Task<ServiceResponse<List<ReadAllEventDTO>>> GetMyEvents();
        public Task<ServiceResponse<ReadAllEventDTO>> CancelEvent(int eventId);
        public Task<ServiceResponse<PagedResultDTO<ReadAllEventDTO>>> GetUpcomingEvents(int PageIndex,int PageSize);


    }
}
