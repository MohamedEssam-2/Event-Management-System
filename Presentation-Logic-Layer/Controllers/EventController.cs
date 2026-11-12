
using Business_Logic_Layer.DTO.EventDTO;
using Business_Logic_Layer.DTO.PaginationDTO;
using Business_Logic_Layer.Service.Interface;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Logic_Layer.Attributes;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles event management operations, including retrieval,
    /// creation, updating, cancellation, and deletion of events.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventController(IEventService _service) : ControllerBase
    {
        /// <summary>
        /// Retrieves all events with pagination, searching, and sorting support.
        /// </summary>
        /// <param name="Search">Optional search term used to filter events.</param>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of events per page. Defaults to 5.</param>
        /// <param name="sortBy">Optional field used to sort the events.</param>
        /// <returns>A paginated list of events matching the specified filters.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// Results are cached using Redis.
        /// </remarks>
        [CacheRedis]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResultDTO<ReadAllEventDTO>>> GetAll(
            [FromQuery] string? Search,
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5,
            [FromQuery] string? sortBy = null)
        {
            var events = await _service.GetAllEvents(Search, PageIndex, PageSize, sortBy);
            return Ok(events);
        }

        /// <summary>
        /// Retrieves detailed information about a specific event.
        /// </summary>
        /// <param name="id">The unique identifier of the event.</param>
        /// <returns>The requested event information.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// Results are cached using Redis.
        /// </remarks>
        [CacheRedis]
        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ReadAllEventDTO>> GetById(int id)
        {
            var eventById = await _service.GetEventById(id);
            return Ok(eventById);
        }

        /// <summary>
        /// Retrieves all events belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>A list of events associated with the specified category.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// Results are cached using Redis.
        /// </remarks>
        [CacheRedis]
        [HttpGet("ByCategory/{categoryId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReadAllEventDTO>>> GetAllByCategory(int categoryId)
        {
            var events = await _service.GetAllEventsInCategory(categoryId);
            return Ok(events);
        }

        /// <summary>
        /// Retrieves the events associated with the currently authenticated organizer.
        /// </summary>
        /// <returns>A list of events managed by the current organizer.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// </remarks>
        [HttpGet("MyEvents")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<List<ReadAllEventDTO>>> GetMyEvents()
        {
            var events = await _service.GetMyEvents();
            return Ok(events);
        }

        /// <summary>
        /// Retrieves upcoming events with pagination support.
        /// </summary>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of events per page. Defaults to 5.</param>
        /// <returns>A paginated list of upcoming events.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// Results are cached using Redis.
        /// </remarks>
        [CacheRedis]
        [HttpGet("Upcoming")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReadAllEventDTO>>> GetUpcomingEvents(
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5)
        {
            var events = await _service.GetUpcomingEvents(PageIndex, PageSize);
            return Ok(events);
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="eventDTO">The event details, including any uploaded files.</param>
        /// <returns>The newly created event information.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// Accepts multipart/form-data.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ReadAllEventDTO>> Create([FromForm] CreateEventDTO eventDTO)
        {
            var createdEvent = await _service.CreateEvent(eventDTO);
            return Ok(createdEvent);
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="id">The unique identifier of the event to update.</param>
        /// <param name="eventDTO">The updated event information, including any uploaded files.</param>
        /// <returns>The updated event information.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// Accepts multipart/form-data.
        /// </remarks>
        [HttpPatch("Update")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ReadAllEventDTO>> Update(int id, [FromForm] UpdateEventDTO eventDTO)
        {
            var updatedEvent = await _service.UpdateEvent(id, eventDTO);
            return Ok(updatedEvent);
        }

        /// <summary>
        /// Cancels an existing event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event to cancel.</param>
        /// <returns>The event information after cancellation.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// </remarks>
        [HttpPatch("CancelEvent")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<ReadAllEventDTO>> CancelEvent(int eventId)
        {
            var canceledEvent = await _service.CancelEvent(eventId);
            return Ok(canceledEvent);
        }

        /// <summary>
        /// Deletes an existing event.
        /// </summary>
        /// <param name="id">The unique identifier of the event to delete.</param>
        /// <returns>True if the event was deleted successfully; otherwise, false.</returns>
        /// <remarks>
        /// Requires Admin or Organizer role authorization.
        /// </remarks>
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deleted = await _service.DeleteEvent(id);
            return Ok(deleted);
        }
    }
}
