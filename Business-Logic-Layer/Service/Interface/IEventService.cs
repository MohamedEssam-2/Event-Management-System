using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IEventService
    {
        public Task<List<ReadAllEventDTO>> GetAllEvents();
        public Task<ReadAllEventDTO> GetEventById(int id);
        public Task<ReadAllEventDTO> CreateEvent(CreateEventDTO eventDTO);
    }
}
