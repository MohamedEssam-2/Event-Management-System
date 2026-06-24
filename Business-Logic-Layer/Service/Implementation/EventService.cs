using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;

namespace Business_Logic_Layer.Service.Implementation
{
    public class EventService(IUnitOfWork _unitOfWork, IMapper _mapper) : IEventService
    {
        public async Task<ReadAllEventDTO> CreateEvent(CreateEventDTO eventDTO)
        {
            var entity = _mapper.Map<Event>(eventDTO);
            var CreateEntity = await _unitOfWork.GetRepository<Event, int>().Create(entity);
            if(CreateEntity is null)
            {
                throw new Exception("Event Not Created");
            }
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ReadAllEventDTO>(CreateEntity);
        }

        public async Task<List<ReadAllEventDTO>> GetAllEvents()
        {
            var Events=await _unitOfWork.GetRepository<Event, int>().GetAll();
            if(Events == null)
            {
                throw new Exception("No Events Found");
            }
            var EventDTO=_mapper.Map<List<ReadAllEventDTO>>(Events);
            return EventDTO;
        }

        public async Task<ReadAllEventDTO> GetEventById(int id)
        {
         var Evetns = await _unitOfWork.GetRepository<Event, int>().GetById(id);
            if (Evetns == null)
            {
                throw new Exception("Event Not Found");
            }
            var EventDTO =  _mapper.Map<ReadAllEventDTO>(Evetns);
            return EventDTO;
        }
    }
}
