using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.EventDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IEventService
    {
        public Task<ServiceResponse<List<ReadAllEventDTO>>> GetAllEvents(string ?Search);
        public Task<ServiceResponse<ReadAllEventDTO>> GetEventById(int id);
        public Task<ServiceResponse<int>> CreateEvent(CreateEventDTO eventDTO);
        public Task<bool> DeleteEvent(int id);
        public Task<ReadAllEventDTO> UpdateEvent(int id, UpdateEventDTO eventDTO);
        public Task<List<ReadAllEventDTO>> GetAllEventsInCategory(int categoryid);

    }
}
