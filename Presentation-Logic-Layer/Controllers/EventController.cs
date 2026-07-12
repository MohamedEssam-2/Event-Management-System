using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController(IEventService _service):ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ReadAllEventDTO>>> GetAll()
        {
            var events = await _service.GetAllEvents();

            return Ok(events);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ReadAllEventDTO>> GetById(int id)
        {
            var eventById = await _service.GetEventById(id);
            return Ok(eventById);
        }

        [HttpPost]
        public async Task<ActionResult<ReadAllEventDTO>> Create(CreateEventDTO eventDTO)
        {
            var createdEvent = await _service.CreateEvent(eventDTO);
            return Ok(createdEvent);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult<ReadAllEventDTO>> Update(UpdateEventDTO eventDTO)
        {
            var updatedEvent = await _service.UpdateEvent(eventDTO);
            return Ok(updatedEvent);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deleted = await _service.DeleteEvent(id);
            return Ok(deleted);
        }

    }
}
