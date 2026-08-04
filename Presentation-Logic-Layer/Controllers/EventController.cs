using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController(IEventService _service):ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReadAllEventDTO>>> GetAll([FromQuery]string ?Search)
        {
            var events = await _service.GetAllEvents(Search);

            return Ok(events);
        }
        
        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ReadAllEventDTO>> GetById(int id)
        {
            var eventById = await _service.GetEventById(id);
            return Ok(eventById);
        }


        [HttpGet("ByCategory/{categoryId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReadAllEventDTO>>> GetAllByCategory(int categoryId)
        {
            var events = await _service.GetAllEventsInCategory(categoryId);

            return Ok(events);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ReadAllEventDTO>> Create([FromForm] CreateEventDTO eventDTO)
        {
            var createdEvent = await _service.CreateEvent(eventDTO);
            return Ok(createdEvent);
        }

        [HttpPatch("Update")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ReadAllEventDTO>> Update(int id, [FromForm] UpdateEventDTO eventDTO)
        {
            var updatedEvent = await _service.UpdateEvent(id,eventDTO);
            return Ok(updatedEvent);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deleted = await _service.DeleteEvent(id);
            return Ok(deleted);
        }

    }
}
